using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Filters.Notification
{
    public class SuccessOnlyNotificationFilter : NotificationFilterBase
    {
        public override string Mode
        {
            get { return "SuccessOnly"; }
        }

        public override void Execute(HealthCheckResult result)
        {
            if (result.Check.Result.HasValue && (result.Check.Result.Value == true))
            {
                Messenger.Publish(result);
            }
        }
    }
}