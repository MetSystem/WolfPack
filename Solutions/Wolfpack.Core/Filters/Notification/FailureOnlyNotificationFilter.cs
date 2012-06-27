using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Filters.Notification
{
    public class FailureOnlyNotificationFilter : NotificationFilterBase
    {
        public override string Mode
        {
            get { return "FailureOnly"; }
        }

        public override void Execute(HealthCheckResult result)
        {
            if (result.Check.Result.HasValue && (result.Check.Result.Value == false))
            {
                Messenger.Publish(result);
            }
        }
    }
}