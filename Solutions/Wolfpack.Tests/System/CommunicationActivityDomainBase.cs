using NUnit.Framework;
using Wolfpack.Core;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Tests.Bdd;
using Wolfpack.Tests.Drivers;

namespace Wolfpack.Tests.System
{
    public abstract class CommunicationActivityDomainConfig
    {
        public HealthCheckAgentStart SessionMessage { get; set; }
        public HealthCheckResult ResultMessage { get; set; }        
    }

    /// <summary>
    /// This provides a base class for all test domains related to communication activities
    /// </summary>
    public abstract class CommunicationActivityDomainBase<T> : BddTestDomain
        where T : CommunicationActivityDomainConfig
    {
        protected AutomationProfile myAutomatedAgent;
        protected T myConfig;
        protected AutomationSessionPublisher mySessionPublisher;
        protected AutomationResultPublisher myResultPublisher;

        protected CommunicationActivityDomainBase(T config)
        {
            myConfig = config;
            myAutomatedAgent = AutomationProfile.Configure();
            mySessionPublisher = new AutomationSessionPublisher();
            myResultPublisher = new AutomationResultPublisher();
        }

        public override void Dispose()
        {
            // override this to perform something at the end of the test
        }

        public abstract void TheActivityIsCorrectlyConfigured();

        public void ThereShouldBe_SessionMessagesReceived(int expected)
        {
            Assert.That(mySessionPublisher.SessionMessagesReceived.Count, Is.EqualTo(expected));
        }


        public abstract void TheSessionStartMessageIsSent();

        public void TheAgentIsStarted()
        {
            myAutomatedAgent.Start();
        }

        public void TheSessionMessageAtIndex_ShouldExactlyMatchTheOneSent(int index)
        {
            var received = mySessionPublisher.SessionMessagesReceived[index];
            var rXml = SerialisationHelper<HealthCheckAgentStart>.DataContractSerialize(received);
            var eXml = SerialisationHelper<HealthCheckAgentStart>.DataContractSerialize(myConfig.SessionMessage);
            Assert.That(rXml, Is.EqualTo(eXml));
        }
    }
}