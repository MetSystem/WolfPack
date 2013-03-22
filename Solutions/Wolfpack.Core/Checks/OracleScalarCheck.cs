using System;
using Wolfpack.Core.Database.Oracle;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Checks
{
    public class OracleScalarCheckConfig : ScalarCheckConfigBase
    {
        public string ConnectionString { get; set; }
    }

    public class OracleScalarCheck : ScalarCheckBase<OracleScalarCheckConfig>
    {
        /// <summary>
        /// default ctor
        /// </summary>
        public OracleScalarCheck(OracleScalarCheckConfig config)
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

            using (var cmd = OracleAdhocCommand.UsingSmartConnection(_config.ConnectionString)
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
                           Description = "Oracle Scalar Check",
                           TypeId = new Guid("FF47074E-798B-4e29-B098-D306EC5B5666"),
                           Name = _baseConfig.FriendlyId
                       };
        }
    }
}