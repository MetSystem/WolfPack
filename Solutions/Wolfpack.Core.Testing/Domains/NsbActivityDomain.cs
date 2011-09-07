using NServiceBus;
using Wolfpack.Core.Bus;

namespace Wolfpack.Core.Testing.Domains
{
    public class NsbActivityDomainConfig : CommunicationActivityDomainConfig
    {
        public string Uri { get; set; }
        public string ErrorQueue { get; set; }
        public string InputQueue { get; set; }

        public NsbActivityDomainConfig()
        {
            ErrorQueue = "WolfpackAutomationErrors";
            InputQueue = "WolfpackAutomationInput";
        }
    }

    public class NsbActivityDomain : CommunicationActivityDomainBase<NsbActivityDomainConfig>
    {
        public NsbActivityDomain(NsbActivityDomainConfig config) 
            : base(config)
        {
        }

        public override void TheActivityIsCorrectlyConfigured()
        {
            myAutomatedAgent.Run(new BusBridgeStartup(new BusConfig
                                                          {
                                                             Enabled = true,
                                                             ErrorQueue = myConfig.ErrorQueue,
                                                             InputQueue = myConfig.InputQueue,
                                                             FriendlyId = "AutomationNsbBusBridge"
                                                          }))
                .Run(mySessionPublisher)
                .Run(myResultPublisher);
        }

        public override void TheSessionStartMessageIsSent()
        {
            var bus = Container.Resolve<IBus>();
            var publisher = new BusHealthCheckSessionPublisher(new BusPublisherConfig
                                                                   {
                                                                       Enabled = true,
                                                                       OutputQueue = myConfig.InputQueue,
                                                                       FriendlyId = "AutomationNsbPublisher"
                                                                   }, bus);
            publisher.Publish(myConfig.SessionMessage);

            myAutomatedAgent.WaitUntil("Session msg is received", 10, () => mySessionPublisher.
                                                                                  SessionMessagesReceived.Exists(
                                                                                      msg => (msg.Id.CompareTo(
                                                                                          myConfig.SessionMessage
                                                                                              .Id) == 0)));
        }
    }
}