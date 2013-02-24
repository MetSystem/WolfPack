using System;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Database.SqlServer;

namespace Wolfpack.Core.Checks
{
    public class SqlScalarCheckConfig : ScalarCheckConfigBase
    {
        public string ConnectionString { get; set; }
    }

    public class SqlScalarCheck : ScalarCheckBase<SqlScalarCheckConfig>
    {
        /// <summary>
        /// default ctor
        /// </summary>
        public SqlScalarCheck(SqlScalarCheckConfig config)
            : base(config)
        {
        }

        protected override void ValidateConfig()
        {
            base.ValidateConfig();

            if (_config.FromQuery.Contains(";"))
                throw new FormatException("Semi-colons are not accepted in Sql from-query statements");
        }

        protected override int RunQuery(string query)
        {
            int rowcount;

            using (var cmd = SqlServerAdhocCommand.UsingSmartConnection(_config.ConnectionString)
                .WithSql(query))
            {
                rowcount = (int)cmd.ExecuteScalar();
            }

            return rowcount;
        }

        protected override PluginDescriptor BuildIdentity()
        {
            return new PluginDescriptor
            {
                Description = "Sql Scalar Check",
                TypeId = new Guid("7BFF8D1C-93EB-4f66-8719-5E6DDDED1E97"),
                Name = _baseConfig.FriendlyId
            };
        }
    }
}