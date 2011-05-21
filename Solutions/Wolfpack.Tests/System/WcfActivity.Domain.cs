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
        }

        public override void Dispose()
        {
        }

        public void TheActivityIsCorrectlyConfigured()
        {
            myAutomatedAgent.Run(new WcfServiceHost(new WcfServiceHostConfig
                                                        {
                                                            Enabled = true,
                                                        }))
                .Run(myMockSessionPublisher.Object);
        }

        public void TheSessionMessageShouldBeReceived()
        {
            myMockSessionPublisher.Verify(target => target.Consume(myConfig.SessionMessage), Times.Once());
        }

        public void TheSessionStartMessageIsSent()
        {
        }

        public void TheAgentIsStarted()
        {
            myAutomatedAgent.Start();
        }
    }
}