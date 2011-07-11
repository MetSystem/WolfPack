
using Wolfpack.Contrib.BuildAnalytics.Parsers;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Tests.Bdd;

namespace Wolfpack.Tests.BuildAnalytics
{
    public class StoryQXmlReportParserDomainConfig
    {
        public string TargetHealthCheckName { get; set; }
        public string ReportFileTemplate { get; set; }
        public string ZipFileTemplate { get; set; }
        public string BuildId { get; set; }
        public bool? BuildResult { get; set; }
        public bool? ParserMatchesToResult { get; set; }

        public StoryQXmlReportParserDomainConfig()
        {
            BuildResult = true;
        }
    }

    public class StoryQXmlReportParserDomain : MessengerEnabledDomain
    {
        protected StoryQXmlReportParser myParser;
        private readonly StoryQXmlReportParserDomainConfig myConfig;

        public StoryQXmlReportParserDomain(StoryQXmlReportParserDomainConfig config)
        {
            myConfig = config;
        }

        public override void Dispose()
        {
            
        }

        public void TheParserComponent()
        {
            myParser = new StoryQXmlReportParser(new StoryQXmlReportParserConfig
                                                             {
                                                                 TargetHealthCheckName = myConfig.TargetHealthCheckName,
                                                                 FriendlyId = "Automation-StoryQXmlReportParser",
                                                                 Enabled = true,
                                                                 ReportFileTemplate = myConfig.ReportFileTemplate,
                                                                 ZipFileTemplate = myConfig.ZipFileTemplate,
                                                                 MatchBuildResult = myConfig.ParserMatchesToResult
                                                             });
        }

        public void TheParserIsInvoked()
        {
            myParser.Consume(new HealthCheckResult
                                 {
                                     Check = new HealthCheckData
                                                 {
                                                     Identity = new PluginDescriptor
                                                                    {
                                                                        Name = myConfig.TargetHealthCheckName
                                                                    },
                                                     Result = myConfig.BuildResult,
                                                     Tags = myConfig.BuildId
                                                 }
                                 });
        }
    }
}