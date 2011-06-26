
using System.Collections.Generic;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Interfaces.Magnum;

namespace Wolfpack.Tests.Drivers
{
    public class AutomationResultPublisher : IHealthCheckResultPublisher
    {
        public AutomationResultPublisher()
        {            
            ResultMessagesReceived = new List<HealthCheckResult>();

            FriendlyId = "AutomationResultPublisher";
            Status = Status.For("AutomationResultPublisher").StateIsSuccess();
            Enabled = true;
        }

        public List<HealthCheckResult> ResultMessagesReceived { get; set; }
        public Status Status { get; set; }

        public void Initialise()
        {
            
        }

        public bool Enabled { get; set; }

        public void Consume(HealthCheckResult message)
        {
            ResultMessagesReceived.Add(message);
        }

        public string FriendlyId { get; set; }
    }
}