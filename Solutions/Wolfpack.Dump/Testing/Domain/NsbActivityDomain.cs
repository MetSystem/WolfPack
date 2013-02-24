namespace Wolfpack.Dump.Testing.Domain
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

    public class NsbActivityDomain : SystemDomainBase<NsbActivityDomainConfig>
    {
        public NsbActivityDomain(NsbActivityDomainConfig config) 
            : base(config)
        {
        }

        public override void TheActivityIsCorrectlyConfigured()
        {
            _automatedAgent.Run(new BusBridgeStartup(new BusConfig
                                                          {
                                                             Enabled = true,
                                                             ErrorQueue = _config.ErrorQueue,
                                                             InputQueue = _config.InputQueue,
                                                             FriendlyId = "AutomationNsbBusBridge"
                                                          }))
                .Run(_sessionPublisher)
                .Run(_resultPublisher);
        }

        public override void TheSessionStartMessageIsSent()
        {
            var bus = Container.Resolve<IBus>();
            var publisher = new BusHealthCheckSessionPublisher(new BusPublisherConfig
                                                                   {
                                                                       Enabled = true,
                                                                       OutputQueue = _config.InputQueue,
                                                                       FriendlyId = "AutomationNsbPublisher"
                                                                   }, bus);
            publisher.Publish(_config.SessionMessage);

            _automatedAgent.WaitUntil("Session msg is received", 10, () => _sessionPublisher.
                                                                                  SessionMessagesReceived.Exists(
                                                                                      msg => (msg.Id.CompareTo(
                                                                                          _config.SessionMessage
                                                                                              .Id) == 0)));
        }
    }
}