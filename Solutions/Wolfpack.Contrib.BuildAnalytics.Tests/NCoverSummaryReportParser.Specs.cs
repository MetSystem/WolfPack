using NUnit.Framework;
using StoryQ;
using Wolfpack.Core.Testing.Bdd;
using log4net.Config;

namespace Wolfpack.Contrib.BuildAnalytics.Tests
{
    [TestFixture]
    public class NCoverSummaryReportParserSpecs : BddFeature
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            XmlConfigurator.Configure();
        }

        protected override Feature DescribeFeature()
        {
            return new Story("Be able to stream data and optionally set a threshold that triggers an alert")
                .InOrderTo("Capture data and receive alerts")
                .AsA("user")
                .IWant("The component to behave correctly");
        }

        [Test]
        public void HappyPath()
        {
            using (var domain = new NCoverSummaryReportParserDomain(new NCoverSummaryReportParserDomainConfig
                                                                        {
                                                                            TargetHealthCheckName = "test",
                                                                            ReportFile = @"testdata\ncover_summary_report.html"
                                                                        }))
            {
                Feature.WithScenario("A valid NCover Summry Html report file is available")
                    .Given(domain.TheParserComponent)
                        .And(domain.TheAgentIsStarted)
                    .When(domain.TheParserIsInvoked)
                    .Then(domain.ThereShouldBe_HealthCheckResultMessagesPublished, 2)
                    .ExecuteWithReport();
            }
        }        
    }
}