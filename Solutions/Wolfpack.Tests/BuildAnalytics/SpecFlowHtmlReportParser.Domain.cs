
using Wolfpack.Contrib.BuildAnalytics.Parsers;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Testing.Domains;

namespace Wolfpack.Tests.BuildAnalytics
{
    public class SpecFlowHtmlReportParserDomainConfig
    {
        public string TargetHealthCheckName { get; set; }
        public string ReportFileTemplate { get; set; }
        public string ZipFileTemplate { get; set; }
        public string BuildId { get; set; }
    }

    public class SpecFlowHtmlReportParserDomain : MessengerEnabledDomain
    {
        protected SpecFlowHtmlReportParser myParser;
        private readonly SpecFlowHtmlReportParserDomainConfig myConfig;

        public SpecFlowHtmlReportParserDomain(SpecFlowHtmlReportParserDomainConfig config)
        {
            myConfig = config;
        }

        public override void Dispose()
        {
            
        }

        public void TheParserComponent()
        {
            myParser = new SpecFlowHtmlReportParser(new SpecFlowHtmlReportParserConfig
                                                             {
                                                                 TargetHealthCheckName = myConfig.TargetHealthCheckName,
                                                                 FriendlyId = "Automation-SpecFlowHtmlReportParser",
                                                                 Enabled = true,
                                                                 ReportFileTemplate = myConfig.ReportFileTemplate,
                                                                 ZipFileTemplate = myConfig.ZipFileTemplate
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
                                                     Result = true,
                                                     Tags = myConfig.BuildId
                                                 }
                                 });
        }
    }
}