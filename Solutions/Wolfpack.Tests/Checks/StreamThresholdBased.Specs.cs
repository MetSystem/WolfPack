using NUnit.Framework;
using StoryQ;
using Wolfpack.Tests.Bdd;

namespace Wolfpack.Tests.Checks
{
    [TestFixture]
    public class StreamThresholdBasedSpecs : BddFeature
    {
        protected override Feature DescribeFeature()
        {
            return new Story("")
                .InOrderTo("")
                .AsA("")
                .IWant("");
        }

        [Test]
        public void NoStreamPublishNoAlertThresholdSetLowData()
        {
            using (var domain = new StreamThresholdBasedDomain())
            {
                Feature.WithScenario("No stream or threshold set")
                    .Given(domain.TheCheckComponent)
                    .When(domain.TheHealthCheckIsInvoked)
                    .Then(domain.ShouldHavePublished_Messages, 0);
            }
        }

        // no stream, no threshold, generated > threshold
    }
}