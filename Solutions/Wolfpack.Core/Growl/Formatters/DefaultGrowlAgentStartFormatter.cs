using System.Text;
using Growl.Connector;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Growl.Formatters
{
    /// <summary>
    /// This formatter provides the default format to any AgentStart event notifications
    /// </summary>
    public class DefaultGrowlAgentStartFormatter : IGrowlNotificationFormatter
    {
        public string EventType { get; set; }

        public string Check { get; set; }

        public DefaultGrowlAgentStartFormatter()
        {
            Check = "*";
            EventType = NotificationEventAgentStart.EventTypeName;
        }

        public void Format(NotificationEvent notificationEvent, global::Growl.Connector.Notification growlNotification)
        {
            var isSticky = false;
            var textBuilder = new StringBuilder(string.Format("Agent {0} on {1} started\n",
                notificationEvent.AgentId, notificationEvent.SiteId));

            var message = Serialiser.FromJson<NotificationEventAgentStart>(notificationEvent.Data);

            if ((message.Activities != null) && (message.Activities.Count > 0))
            {
                textBuilder.AppendFormat("{0} Activities loaded...\n", message.Activities.Count);
                message.Activities.ForEach(c => textBuilder.AppendFormat("{0}\n", c.Name));
            }
            if ((message.UnhealthyActivities != null) && (message.UnhealthyActivities.Count > 0))
            {
                textBuilder.Append("*** ATTENTION ***\n");
                textBuilder.AppendFormat("{0} Unheathly Activities...\n", message.UnhealthyActivities.Count);
                message.UnhealthyActivities.ForEach(c => textBuilder.AppendFormat("{0}\n", c.Name));
                isSticky = true;
            }
            if ((message.Checks != null) && (message.Checks.Count > 0))
            {
                textBuilder.AppendFormat("{0} Checks loaded...\n", message.Checks.Count);
                message.Checks.ForEach(c => textBuilder.AppendFormat("{0}\n", c.Name));
            }
            if ((message.UnhealthyChecks != null) && (message.UnhealthyChecks.Count > 0))
            {
                textBuilder.Append("*** ATTENTION ***\n");
                textBuilder.AppendFormat("{0} Unheathly Checks...\n", message.UnhealthyChecks.Count);
                message.UnhealthyChecks.ForEach(c => textBuilder.AppendFormat("{0}\n", c.Name));
                isSticky = true;
            }

            growlNotification.Title = string.Format("{0} Notification ({1})", notificationEvent.EventType, notificationEvent.SiteId);
            growlNotification.Text = textBuilder.ToString();
            growlNotification.Priority = isSticky ? Priority.Emergency : Priority.Normal;
            growlNotification.Sticky = isSticky;
        }
    }
}