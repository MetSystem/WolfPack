using NUnit.Framework;
using StoryQ;
using Wolfpack.Tests.Bdd;

namespace Wolfpack.Tests.BuildAnalytics
{
    [TestFixture]
    public class StoryQXmlReportParserSpecs : BddFeature
    {
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
            using (var domain = new StoryQXmlReportParserDomain(new StoryQXmlReportParserDomainConfig
                                                                        {
                                                                            TargetHealthCheckName = "test",
                                                                            ReportFile = @"C:\Projects\Wolfpack\Trunk\Solutions\Wolfpack.Tests\bin\Debug\StoryQ_Report\storyq.xml"
                                                                        }))
            {
                Feature.WithScenario("A valid NCover Summry Html report file is available")
                    .Given(domain.TheParserComponent)
                    .When(domain.TheParserIsInvoked)
                    .Then(domain.TheCoverageSummaryValuesShouldBeAvailable)
                    .ExecuteWithReport();
            }
        }        
    }
}