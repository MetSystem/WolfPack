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
            return new Story("Be able to stream data and optionally set a threshold that triggers an alert")
                .InOrderTo("Capture data and receive alerts")
                .AsA("user")
                .IWant("The component to behave correctly");
        }

        [Test]
        public void StreamDisabledNoAlertThreshold()
        {
            using (var domain = new StreamThresholdBasedDomain(StreamThresholdBasedDomainConfig.FiresValues(1)))
            {
                Feature.WithScenario("Streaming disabled and no alert threshold set")
                    .Given(domain.TheCheckComponent)
                    .When(domain.TheHealthCheckIsInvoked)
                    .Then(domain.ShouldHavePublished_Messages, 0)
                    .ExecuteWithReport();
            }
        }
        
        [Test]
        public void StreamEnabledNoAlertThreshold()
        {
            using (var domain = new StreamThresholdBasedDomain(StreamThresholdBasedDomainConfig.FiresValues(1)
                .StreamingIsEnabled()))
            {
                Feature.WithScenario("Streaming enabled and no alert threshold set")
                    .Given(domain.TheCheckComponent)
                    .When(domain.TheHealthCheckIsInvoked)
                    .Then(domain.ShouldHavePublished_Messages, 1)
                    .ExecuteWithReport();
            }
        }

        [Test]
        public void StreamDisabledAlertThresholdSetNoTrigger()
        {
            using (var domain = new StreamThresholdBasedDomain(StreamThresholdBasedDomainConfig.FiresValues(1)
                .ThresholdIs(10)))
            {
                Feature.WithScenario("Streaming disabled and an alert threshold has been set but not triggered")
                    .Given(domain.TheCheckComponent)
                    .When(domain.TheHealthCheckIsInvoked)
                    .Then(domain.ShouldHavePublished_Messages, 0)
                    .ExecuteWithReport();
            }
        }        

        [Test]
        public void StreamDisabledAlertThresholdSetLowHighValues()
        {
            using (var domain = new StreamThresholdBasedDomain(StreamThresholdBasedDomainConfig.FiresValues(1,11)
                .ThresholdIs(10)))
            {
                Feature.WithScenario("Streaming disabled and an alert threshold has been set with a low and a high value received")
                    .Given(domain.TheCheckComponent)
                    .When(domain.TheHealthCheckIsInvoked)
                    .Then(domain.ShouldHavePublished_Messages, 1)
                        .And(domain.TheDataMessageShouldIndicateFailure)
                    .ExecuteWithReport();
            }
        }        
    }
}