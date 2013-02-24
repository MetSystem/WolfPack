using System;
using System.Data;
using MSUtil;
using Wolfpack.Core;
using Wolfpack.Core.Checks;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Notification;

namespace Wolfpack.Contrib.LogParser
{
    public abstract class LogParserCheckBase<TConfig> : HealthCheckBase<TConfig>
        where TConfig : LogParserConfigBase
    {
        protected LogParserCheckBase(TConfig config) : base(config)
        {
        }

        public override void Execute()
        {
            var tblData = RunQuery(_config.Query);

            var data = HealthCheckData.For(Identity, DescribeNotification())
                .ResultIs(DecideResult(tblData.Rows.Count))
                .ResultCountIs(tblData.Rows.Count);

            ArtifactDescriptor artifactDescriptor = null;

            if (_config.GenerateArtifacts.GetValueOrDefault(false))
                artifactDescriptor = ArtifactManager.Save(Identity, ArtifactManager.KnownContentTypes.TabSeparated, tblData);

            Messenger.Publish(NotificationRequestBuilder.For(_config.NotificationMode, data)
                .AssociateArtifact(artifactDescriptor)
                .Build());
        }

        protected virtual string DescribeNotification()
        {
            return string.Format("{0} logparser based check", GetType().Name);
        }

        protected DataTable RunQuery(string query)
        {
            var logQuery = new LogQueryClass();
            var oRecordSet = logQuery.Execute(query, GetInputContext());
            return ConvertToDataTable(oRecordSet);
        }

        /// <summary>
        /// Returns the correct Input Context class for this query.
        /// </summary>
        /// <returns></returns>
        protected abstract object GetInputContext();

        protected DataTable ConvertToDataTable(ILogRecordset lpRecordset)
        {
            var dtTemp = new DataTable();

            for (var i = 0; i < lpRecordset.getColumnCount() - 1; i++)
            {
                //Console.WriteLine("{0}={1}", lpRecordset.getColumnName(i), lpRecordset.getColumnType(i));
                dtTemp.Columns.Add(GetColumn(lpRecordset.getColumnName(i), lpRecordset.getColumnType(i)));
            }

            while (!lpRecordset.atEnd())
            {
                var dr = dtTemp.NewRow();
                var objRow = lpRecordset.getRecord();

                for (var i = 0; i < dtTemp.Columns.Count - 1; i++)
                {
                    if (dtTemp.Columns[i].DataType == typeof(string))
                    {
                        var data = objRow.getValue(i).ToString();
                        dr[i] = string.Format("\"{0}\"", data.Replace(Environment.NewLine, "\n"));
                    }
                    else
                    {
                        dr[i] = objRow.getValue(i);
                    }
                }

                dtTemp.Rows.Add(dr);
                lpRecordset.moveNext();
            }

            return dtTemp;
        }

        private static DataColumn GetColumn(string name, int lpType)
        {
            switch (lpType)
            {
                case 1:
                    return new DataColumn(name, typeof(int));
                case 2:
                    return new DataColumn(name, typeof (double));
                case 3:
                    return new DataColumn(name, typeof (string));
                case 4:
                    return new DataColumn(name, typeof (DateTime));
                default:
                    throw new NotSupportedException(string.Format("LogParser column type {0} is not supported (yet!)", lpType));
            }
        }

        /// <summary>
        /// Implements logic to decide whether this query produces a successful or not result
        /// </summary>
        /// <param name="rowcount"></param>
        /// <returns></returns>
        protected virtual bool DecideResult(int rowcount)
        {
            bool result;

            if (_config.InterpretZeroRowsAsAFailure)
            {
                result = (rowcount > 0);
            }
            else
            {
                result = (rowcount == 0);
            }

            return result;
        }
    }
}