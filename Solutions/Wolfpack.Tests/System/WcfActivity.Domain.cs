using Moq;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Interfaces.Magnum;
using Wolfpack.Core.Wcf;
using Wolfpack.Tests.Bdd;
using Wolfpack.Tests.Drivers;

namespace Wolfpack.Tests.System
{
    public class WcfActivityDomainConfig
    {
        public string Uri { get; set; }
        public HealthCheckAgentStart SessionMessage { get; set; }
        public HealthCheckResult ResultMessage { get; set; }
    }

    public class WcfActivityDomain : BddTestDomain
    {
        private readonly AutomationProfile myAutomatedAgent;
        private readonly WcfActivityDomainConfig myConfig;
        private readonly Mock<IHealthCheckSessionPublisher> myMockSessionPublisher;

        public WcfActivityDomain(WcfActivityDomainConfig config)
        {
            myConfig = config;
            myAutomatedAgent = AutomationProfile.Configure();
            myMockSessionPublisher = new Mock<IHealthCheckSessionPublisher>();
            myMockSessionPublisher.SetupProperty(target => target.Status, Status.For("Test").StateIsSuccess());
        }

        public override void Dispose()
        {
        }

        public void TheActivityIsCorrectlyConfigured()
        {
            myAutomatedAgent.Run(new WcfServiceHost(new WcfServiceHostConfig
                                                        {
                                                            Enabled = true,
                                                            Uri = myConfig.Uri
                                                        })
                                     {
                                         Status = Status.For("WcfServiceHost").StateIsSuccess()
                                     })
                .Run(myMockSessionPublisher.Object);
        }

        public void TheSessionMessageShouldBeReceived()
        {
            myMockSessionPublisher.Verify(target => target.Consume(myConfig.SessionMessage), Times.Once());
        }

        public void TheSessionStartMessageIsSent()
        {
            var publisher = new WcfSessionPublisher(new WcfPublisherConfiguration
                                                        {
                                                            Enabled = true,
                                                            FriendlyId = "TestWcfPublisher",
                                                            Uri = myConfig.Uri
                                                        });
            publisher.Publish(myConfig.SessionMessage);
        }

        public void TheAgentIsStarted()
        {
            myAutomatedAgent.Start();
        }
    }
}