
using Wolfpack.Contrib.BuildAnalytics.Parsers;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Testing.Domains;

namespace Wolfpack.Tests.BuildAnalytics
{
    public class NCoverSummaryReportParserDomainConfig
    {
        public string TargetHealthCheckName { get; set; }
        public string ReportFile { get; set; }
    }

    public class NCoverSummaryReportParserDomain : MessengerEnabledDomain
    {
        protected NCoverHtmlSummaryReportParser myParser;
        private readonly NCoverSummaryReportParserDomainConfig myConfig;

        public NCoverSummaryReportParserDomain(NCoverSummaryReportParserDomainConfig config)
        {
            myConfig = config;
        }

        public override void Dispose()
        {
            
        }

        public void TheParserComponent()
        {
            myParser = new NCoverHtmlSummaryReportParser(new NCoverHtmlSummaryReportParserConfig
                                                             {
                                                                 TargetHealthCheckName = myConfig.TargetHealthCheckName,
                                                                 FriendlyId = "Automation-NCoverHtmlSummaryReportParser",
                                                                 Enabled = true,
                                                                 ReportFileTemplate = myConfig.ReportFile
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
                                                     Result = true
                                                 }
                                 });
        }
    }
}