using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Wcf;

namespace Wolfpack.Tests.System
{
    public class WcfActivityDomainConfig : CommunicationActivityDomainConfig
    {
        public string Uri { get; set; }
    }

    public class WcfActivityDomain : CommunicationActivityDomainBase<WcfActivityDomainConfig>
    {
        public WcfActivityDomain(WcfActivityDomainConfig config)
            : base(config)
        {
        }

        public override void TheActivityIsCorrectlyConfigured()
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

        public override void TheSessionStartMessageIsSent()
        {
            var publisher = new WcfSessionPublisher(new WcfPublisherConfiguration
                                                        {
                                                            Enabled = true,
                                                            FriendlyId = "TestWcfPublisher",
                                                            Uri = myConfig.Uri
                                                        });
            publisher.Publish(myConfig.SessionMessage);
            myAutomatedAgent.WaitUntil("Session msg is received", 10, () => mySessionPublisher.
                                                                               SessionMessagesReceived.Exists(
                                                                                   msg => (msg.Id.CompareTo(
                                                                                       myConfig.SessionMessage
                                                                                           .Id) == 0)));
        }
    }
}