using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Interfaces
{
    public interface INotificationModeFilter
    {
        string Mode { get; }
        void Execute(HealthCheckResult result);
    }
}