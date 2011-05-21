using System.Threading;
using NUnit.Framework;
using Wolfpack.Core.Interfaces.Entities;
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
        private readonly AutomationSessionPublisher mySessionPublisher;
        private readonly AutomationResultPublisher myResultPublisher;

        ManualResetEvent myWait = new ManualResetEvent(false);

        public WcfActivityDomain(WcfActivityDomainConfig config)
        {
            myConfig = config;
            myAutomatedAgent = AutomationProfile.Configure();
            mySessionPublisher = new AutomationSessionPublisher();
            myResultPublisher = new AutomationResultPublisher();
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
                .Run(mySessionPublisher)
                .Run(myResultPublisher);
        }

        public void ThereShouldBe_SessionMessagesReceived(int expected)
        {
            Assert.That(mySessionPublisher.SessionMessagesReceived.Count, Is.EqualTo(expected));
        }

        public void TheSessionMessageAtIndex_ShouldExactlyMatchTheOneSent(int index)
        {
            var received = mySessionPublisher.SessionMessagesReceived[index];

            Assert.That(received.Id, Is.EqualTo(myConfig.SessionMessage.Id));
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
            myAutomatedAgent.WaitUntil("Session msg is received", 5, () => mySessionPublisher.
                                                                                  SessionMessagesReceived.Exists(
                                                                                      msg => (msg.Id.CompareTo(
                                                                                          myConfig.SessionMessage
                                                                                              .Id) == 0)));
        }

        public void TheAgentIsStarted()
        {
            myAutomatedAgent.Start();
        }
    }
}