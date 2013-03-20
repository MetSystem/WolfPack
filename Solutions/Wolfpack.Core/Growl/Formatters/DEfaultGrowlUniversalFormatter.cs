using Growl.Connector;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Growl.Formatters
{
    /// <summary>
    /// This formatter provides the default format to ANY event notification
    /// </summary>
    public class DefaultGrowlUniversalFormatter : IGrowlNotificationFormatter
    {
        public string EventType { get; set; }

        public string Check { get; set; }

        public DefaultGrowlUniversalFormatter()
        {
            Check = "*";
            EventType = "*";
        }

        public void Format(NotificationEvent notificationEvent, global::Growl.Connector.Notification growlNotification)
        {
            growlNotification.Title = string.Format("{0} Notification ({1})", notificationEvent.EventType, notificationEvent.SiteId);
            growlNotification.Text = notificationEvent.Message;
            growlNotification.Priority = notificationEvent.CriticalFailure ? Priority.Emergency : Priority.Normal;
            growlNotification.Sticky = notificationEvent.CriticalFailure;
        }
    }
}
