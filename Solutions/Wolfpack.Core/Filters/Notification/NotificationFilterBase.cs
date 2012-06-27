using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Filters.Notification
{
    public abstract class NotificationFilterBase : INotificationModeFilter
    {
        public abstract string Mode { get; }
        public abstract void Execute(HealthCheckResult result);

        protected string GetKey(HealthCheckResult result)
        {
            return result.Check.Identity.Name.ToLower();
        }
    }
}