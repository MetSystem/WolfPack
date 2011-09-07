using System.Collections.Generic;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Interfaces.Magnum;

namespace Wolfpack.Core.Testing.Drivers
{
    public class AutomationSessionPublisher : IHealthCheckSessionPublisher
    {
        public AutomationSessionPublisher()
        {            
            SessionMessagesReceived = new List<HealthCheckAgentStart>();

            FriendlyId = "AutomationResultPublisher";
            Status = Status.For("AutomationSessionPublisher").StateIsSuccess();
            Enabled = true;
        }

        public List<HealthCheckAgentStart> SessionMessagesReceived { get; set; }
        public Status Status { get; set; }

        public void Initialise()
        {
            
        }

        public bool Enabled { get; set; }

        public void Consume(HealthCheckAgentStart message)
        {
            SessionMessagesReceived.Add(message);
        }

        public string FriendlyId { get; set; }
    }
}