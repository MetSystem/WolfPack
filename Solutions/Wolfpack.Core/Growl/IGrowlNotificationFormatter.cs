using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Growl
{
    public interface IGrowlNotificationFormatter
    {
        string EventType { get; set; }
        string Check { get; set; }
        void Format(NotificationEvent notificationEvent, global::Growl.Connector.Notification growlNotification);
    }
}