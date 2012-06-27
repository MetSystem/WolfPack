using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Interfaces
{
    public interface INotificationHub
    {
        void Initialise(AgentInfo info);
        void ForwardToPublishers(HealthCheckData result);
    }
}