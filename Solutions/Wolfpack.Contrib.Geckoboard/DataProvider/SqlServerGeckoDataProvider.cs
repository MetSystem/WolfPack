using Wolfpack.Core.Database;
using Wolfpack.Core.Database.SqlServer;

namespace Wolfpack.Contrib.Geckoboard.DataProvider
{
    public class SqlServerGeckoDataProviderConfig
    {
        public string ConnectionString { get; set; }
    }

    public class SqlServerGeckoDataProvider : SqlGeckoDataProviderBase
    {
        protected SqlServerGeckoDataProviderConfig _config;

        /// <summary>
        /// Default ctor
        /// </summary>
        public SqlServerGeckoDataProvider()
        {
            FriendlyId = string.Format("{0}SqlServer", GeckoboardDataService.GeckoboardDataProviderPrefix);
            _config = new SqlServerGeckoDataProviderConfig
                           {
                               ConnectionString = "Wolfpack"
                           };
        }

        /// <summary>
        /// Allows external configuration (config object must be in IoC Container
        /// or created by hand)
        /// </summary>
        /// <param name="config"></param>
        public SqlServerGeckoDataProvider(SqlServerGeckoDataProviderConfig config)
            : this()
        {
            _config = config;
        }

        protected override AdhocCommandBase GetMapDataForCheckCommand(MapArgs args)
        {
            var outcome = ConvertOutcome(args.Outcome);

            return SqlServerAdhocCommand.UsingSmartConnection(_config.ConnectionString)
                .WithSql(SqlServerStatement.Create(
                    "SELECT DISTINCT Longitude, Latitude FROM AgentData WHERE CheckId=")
                             .InsertParameter("@pCheckId", args.Check)
                             .AppendIf(() => !string.IsNullOrEmpty(outcome), "AND {0}", outcome)
                             .AppendIfSupplied("AND SiteId=", "@pSiteId", args.Site)
                             .AppendIfSupplied("AND AgentId=", "@pAgentId", args.Agent)
                             .AppendIfSupplied("AND Tags=", "@pTags", args.Tag)
                             .Append("AND (Longitude IS NOT NULL) AND (Latitude IS NOT NULL)"));
        }

        protected override AdhocCommandBase GetPieChartDataForAllSitesCommand()
        {
            return SqlServerAdhocCommand.UsingSmartConnection(_config.ConnectionString)
                .WithSql(SqlServerStatement.Create(
                        "SELECT SiteId [SegmentId], COUNT(*) [Count] FROM AgentData WHERE Result = 0 AND EventType = 'Result' GROUP BY SiteId"));
        }

        protected override AdhocCommandBase GetPieChartDataForSiteCommand(string site)
        {
            return SqlServerAdhocCommand.UsingSmartConnection(_config.ConnectionString)
                .WithSql(SqlServerStatement.Create(
                    "SELECT CheckId [SegmentId], COUNT(*) [Count] FROM AgentData WHERE (Result = 0) AND (EventType = 'Result') AND (lower(SiteId)=")
                             .InsertParameter("@pSiteId", site.ToLower())
                             .Append(") GROUP BY CheckId"));
        }

        protected override AdhocCommandBase GetGeckoboardPieChartForCheckCommand(PieChartArgs args)
        {
            string outcome;
            string operation;

            switch (args.DataOperation)
            {
                case DataOperationType.Average:
                    operation = "AVG(ResultCount)";
                    break;
                case DataOperationType.Sum:
                    operation = "SUM(ResultCount)";
                    break;
                default:
                    operation = "COUNT(*)";
                    break;
            }

            switch (args.Outcome)
            {
                case OutcomeType.Failure:
                    outcome = "(Result = 0)";
                    break;
                case OutcomeType.Success:
                    outcome = "(Result = 1)";
                    break;
                default:
                    outcome = string.Empty;
                    break;
            }
            
            return SqlServerAdhocCommand.UsingSmartConnection(_config.ConnectionString)
                .WithSql(SqlServerStatement.Create(
                    "SELECT Tags [SegmentId], {0} [Count] FROM AgentData WHERE CheckId=", operation)
                    .InsertParameter("@pCheckId", args.Check)
                    .AppendIf(() => !string.IsNullOrEmpty(outcome), "AND {0}", outcome)
                    .AppendIf(() => !string.IsNullOrEmpty(args.Site), "AND SiteId=")
                    .InsertParameterIf(() => !string.IsNullOrEmpty(args.Site), "@pSiteId", args.Site)
                    .AppendIf(() => !string.IsNullOrEmpty(args.Agent), "AND AgentId=")
                    .InsertParameterIf(() => !string.IsNullOrEmpty(args.Agent), "@pAgentId", args.Agent)
                    .Append("GROUP BY Tags"));
        }

        protected override AdhocCommandBase GetLineChartDataForCheckCommand(LineChartArgs args)
        {
            var outcome = ConvertOutcome(args.Outcome);

            return SqlServerAdhocCommand.UsingSmartConnection(_config.ConnectionString)
                .WithSql(SqlServerStatement.Create(
                    "SELECT TOP {0} ResultCount, GeneratedOnUtc FROM AgentData WHERE CheckId=", args.Limit)
                             .InsertParameter("@pCheckId", args.Check)
                             .AppendIf(() => !string.IsNullOrEmpty(outcome), "AND {0}", outcome)
                             .AppendIfSupplied("AND SiteId=", "@pSiteId", args.Site)
                             .AppendIfSupplied("AND AgentId=", "@pAgentId", args.Agent)
                             .AppendIfSupplied("AND Tags=", "@pTags", args.Tag)
                             .OrderBy("GeneratedOnUtc"));
        }


        protected override AdhocCommandBase GetLineChartDataForCheckRateCommand(LineChartArgs args)
        {
            var outcome = ConvertOutcome(args.Outcome);
            var operation = ConvertOperation(args.DataOperation);

            return SqlServerAdhocCommand.UsingSmartConnection(_config.ConnectionString)
                .WithSql(SqlServerStatement.Create(
                    "SELECT TOP {0} ({1}*{2}) [MinutesBucket], {3} [ResultCount] FROM AgentData WHERE CheckId=",
                    args.Limit,
                    args.Bucket,
                    args.Multiplier,
                    operation)
                             .InsertParameter("@pCheckId", args.Check)
                             .AppendIf(() => !string.IsNullOrEmpty(outcome), "AND {0}", outcome)
                             .AppendIfSupplied("AND SiteId=", "@pSiteId", args.Site)
                             .AppendIfSupplied("AND AgentId=", "@pAgentId", args.Agent)
                             .AppendIfSupplied("AND Tags=", "@pTags", args.Tag)
                             .Append("GROUP BY {0}", args.Bucket)
                             .OrderBy(args.Bucket));
        }

        protected override AdhocCommandBase GetGeckoMeterDataForCheckAverageCommand(GeckometerArgs args)
        {
            return SqlServerAdhocCommand.UsingSmartConnection(_config.ConnectionString)
                .WithSql(SqlServerStatement.Create(
                    "SELECT MIN(ResultCount) [Min], MAX(ResultCount) [Max], AVG(ResultCount) [Avg] FROM AgentData WHERE (ResultCount IS NOT NULL) AND (EventType = 'Result') AND CheckId=")
                             .InsertParameter("@pCheckId", args.Check)
                             .AppendIf(() => !string.IsNullOrEmpty(args.Site), "AND SiteId=")
                             .InsertParameterIf(() => !string.IsNullOrEmpty(args.Site), "@pSiteId", args.Site)
                             .AppendIf(() => !string.IsNullOrEmpty(args.Agent), "AND AgentId=")
                             .InsertParameterIf(() => !string.IsNullOrEmpty(args.Agent), "@pAgentId", args.Agent)
                             .AppendIf(() => !string.IsNullOrEmpty(args.Tag), "AND tags=")
                             .InsertParameterIf(() => !string.IsNullOrEmpty(args.Tag), "@pTag", args.Tag));
        }

        protected override AdhocCommandBase GetGeckoMeterDataForCheckCommand(GeckometerArgs args)
        {
            return SqlServerAdhocCommand.UsingSmartConnection(_config.ConnectionString)
                .WithSql(SqlServerStatement.Create(
                    "SELECT TOP 1 ResultCount [Last] FROM AgentData WHERE (ResultCount IS NOT NULL) AND (EventType = 'Result') AND CheckId=")
                             .InsertParameter("@pCheckId", args.Check)
                             .AppendIf(() => !string.IsNullOrEmpty(args.Site), "AND SiteId=")
                             .InsertParameterIf(() => !string.IsNullOrEmpty(args.Site), "@pSiteId", args.Site)
                             .AppendIf(() => !string.IsNullOrEmpty(args.Agent), "AND AgentId=")
                             .InsertParameterIf(() => !string.IsNullOrEmpty(args.Agent), "@pAgentId", args.Agent)
                             .AppendIf(() => !string.IsNullOrEmpty(args.Tag), "AND tags=")
                             .InsertParameterIf(() => !string.IsNullOrEmpty(args.Tag), "@pTag", args.Tag)
                             .OrderBy("GeneratedOnUtc"));
        }

        protected override AdhocCommandBase GetComparisonDataForSiteCheckCommand(ComparisonArgs args)
        {
            return SqlServerAdhocCommand.UsingSmartConnection(_config.ConnectionString)
                .WithSql(SqlServerStatement.Create(
                    "SELECT TOP 2 ResultCount FROM AgentData WHERE (ResultCount IS NOT NULL) AND (EventType = 'Result') AND (SiteId=")
                             .InsertParameter("@pSiteId", args.Site)
                             .Append(") AND (CheckId=")
                             .InsertParameter("@pCheckId", args.Check).Append(")")
                             .AppendIf(() => !string.IsNullOrEmpty(args.Tag), "AND (tags='{0}')", args.Tag)
                             .OrderBy("GeneratedOnUtc"));
        }
    }
}