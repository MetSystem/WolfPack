using Wolfpack.Contrib.BuildAnalytics.Parsers;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Testing.Domains;

namespace Wolfpack.Contrib.BuildAnalytics.Tests
{
    public class SpecFlowHtmlReportParserDomainConfig
    {
        public string TargetHealthCheckName { get; set; }
        public string ReportFileTemplate { get; set; }
        public string ZipFileTemplate { get; set; }
        public string BuildId { get; set; }
    }

    public class SpecFlowHtmlReportParserDomain : SystemDomainBase
    {
        protected SpecFlowHtmlReportParser _parser;
        private readonly SpecFlowHtmlReportParserDomainConfig _config;

        public SpecFlowHtmlReportParserDomain(SpecFlowHtmlReportParserDomainConfig config)
        {
            _config = config;
        }

        public override void Dispose()
        {
            
        }

        public void TheParserComponent()
        {
            _parser = new SpecFlowHtmlReportParser(new SpecFlowHtmlReportParserConfig
                                                             {
                                                                 TargetHealthCheckName = _config.TargetHealthCheckName,
                                                                 FriendlyId = "Automation-SpecFlowHtmlReportParser",
                                                                 Enabled = true,
                                                                 ReportFileTemplate = _config.ReportFileTemplate,
                                                                 ZipFileTemplate = _config.ZipFileTemplate
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
                                                     Result = true,
                                                     Tags = _config.BuildId
                                                 }
                                 });
        }
    }
}