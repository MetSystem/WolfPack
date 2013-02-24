using Wolfpack.Contrib.BuildAnalytics.Parsers;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Testing.Domains;

namespace Wolfpack.Contrib.BuildAnalytics.Tests
{
    public class NCoverSummaryReportParserDomainConfig
    {
        public string TargetHealthCheckName { get; set; }
        public string ReportFile { get; set; }
    }

    public class NCoverSummaryReportParserDomain : SystemDomainBase
    {
        protected NCoverHtmlSummaryReportParser _parser;
        private readonly NCoverSummaryReportParserDomainConfig _config;

        public NCoverSummaryReportParserDomain(NCoverSummaryReportParserDomainConfig config)
        {
            _config = config;
        }

        public override void Dispose()
        {
            
        }

        public void TheParserComponent()
        {
            _parser = new NCoverHtmlSummaryReportParser(new NCoverHtmlSummaryReportParserConfig
                                                             {
                                                                 TargetHealthCheckName = _config.TargetHealthCheckName,
                                                                 FriendlyId = "Automation-NCoverHtmlSummaryReportParser",
                                                                 Enabled = true,
                                                                 ReportFileTemplate = _config.ReportFile
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
                                                     Result = true
                                                 }
                                 });
        }
    }
}