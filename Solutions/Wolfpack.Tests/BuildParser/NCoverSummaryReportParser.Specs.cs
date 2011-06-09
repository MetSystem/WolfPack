using NUnit.Framework;
using StoryQ;
using Wolfpack.Tests.Bdd;

namespace Wolfpack.Tests.BuildParser
{
    [TestFixture]
    public class NCoverSummaryReportParserSpecs : BddFeature
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
            using (var domain = new NCoverSummaryReportParserDomain())
            {
                Feature.WithScenario("test")
                    .Given(domain.TheParserComponent)
                    .When(domain.TheParserIsInvoked)
                    .Then(domain.TheCoverageSummaryValuesShouldBeAvailable)
                    .ExecuteWithReport();
            }
        }        
    }
}