using Wolfpack.Contrib.BuildAnalytics.Parsers;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Testing.Domains;

namespace Wolfpack.Contrib.BuildAnalytics.Tests
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

    public class StoryQXmlReportParserDomain : SystemDomainBase
    {
        protected StoryQXmlReportParser _parser;
        private readonly StoryQXmlReportParserDomainConfig _config;

        public StoryQXmlReportParserDomain(StoryQXmlReportParserDomainConfig config)
        {
            _config = config;
        }

        public override void Dispose()
        {
            
        }

        public void TheParserComponent()
        {
            _parser = new StoryQXmlReportParser(new StoryQXmlReportParserConfig
                                                             {
                                                                 TargetHealthCheckName = _config.TargetHealthCheckName,
                                                                 FriendlyId = "Automation-StoryQXmlReportParser",
                                                                 Enabled = true,
                                                                 ReportFileTemplate = _config.ReportFileTemplate,
                                                                 ZipFileTemplate = _config.ZipFileTemplate,
                                                                 MatchBuildResult = _config.ParserMatchesToResult
                                                             });
        }

        public void TheParserIsInvoked()
        {
            _parser.Consume(new HealthCheckResult
                                 {
                                     Check = new HealthCheckData
                                                 {
                                                     Identity = new PluginDescriptor
                                                                    {
                                                                        Name = _config.TargetHealthCheckName
                                                                    },
                                                     Result = _config.BuildResult,
                                                     Tags = _config.BuildId
                                                 }
                                 });
        }
    }
}