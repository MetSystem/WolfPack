using System;
using Wolfpack.Contrib.BuildAnalytics.Parsers;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Tests.Bdd;

namespace Wolfpack.Tests.BuildAnalytics
{
    public class StoryQXmlReportParserDomainConfig
    {
        public string TargetHealthCheckName { get; set; }
        public string ReportFile { get; set; }
    }

    public class StoryQXmlReportParserDomain : BddTestDomain
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
            myParser = new StoryQXmlReportParser(new StoryQXmlReportParserConfig()
                                                             {
                                                                 TargetHealthCheckName = myConfig.TargetHealthCheckName,
                                                                 FriendlyId = "Automation-StoryQXmlReportParser",
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

        public void TheCoverageSummaryValuesShouldBeAvailable()
        {
            throw new NotImplementedException();
        }
    }
}