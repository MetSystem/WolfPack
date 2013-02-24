using System;
using System.Linq;
using System.Text;
using Growl.Connector;
using ServiceStack.Text;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Growl.Formatters
{
    /// <summary>
    /// This formatter provides the default format to any HealthCheck event notifications
    /// </summary>
    public class DefaultGrowlHealthCheckFormatter : IGrowlNotificationFormatter
    {
        public string EventType { get; set; }

        public string Check { get; set; }

        public DefaultGrowlHealthCheckFormatter()
        {
            Check = "*";
            EventType = NotificationEventHealthCheck.EventTypeName;
        }

        public void Format(NotificationEvent notificationEvent, global::Growl.Connector.Notification growlNotification)
        {
            string resultLine1;
            var criticalFailure = false;

            var message = Serialiser.FromJson<NotificationEventHealthCheck>(notificationEvent.Data);

            if (!notificationEvent.Result.HasValue)
                resultLine1 = "Result is Unknown!";
            else if (notificationEvent.Result.Value)
                resultLine1 = "Passed!";
            else
                resultLine1 = "** Failed **";

            string resultLine2;
            string resultLine3 = null;
            string resultLine4 = null;

            if (notificationEvent.CriticalFailure)
            {
                resultLine2 = "*** CRITICAL FAILURE ***";

                if (notificationEvent.CriticalFailureDetails != null)
                {
                    resultLine3 = string.Format("Wolfpack Log Ref:={0}", notificationEvent.CriticalFailureDetails.Id);
                }
                resultLine4 = message.Message;
                criticalFailure = true;
            }
            else
            {
                resultLine2 = message.Message;
            }

            var textBuilder = new StringBuilder(string.Format("{0}\n{1}\n", notificationEvent.CheckId,
                DateTime.Now));
            textBuilder.AppendIf(!string.IsNullOrEmpty(resultLine1), "{0}\n", resultLine1);
            textBuilder.AppendIf(!string.IsNullOrEmpty(resultLine2), "{0}\n", resultLine2);
            textBuilder.AppendIf(!string.IsNullOrEmpty(resultLine3), "{0}\n", resultLine3);
            textBuilder.AppendIf(!string.IsNullOrEmpty(resultLine4), "{0}\n", resultLine4);

            textBuilder.AppendIf(notificationEvent.ResultCount.HasValue, "Count:={0}\n",
                notificationEvent.ResultCount.GetValueOrDefault());

            if (message.Properties != null)
            {
                message.Properties.ToList().ForEach(
                    p => textBuilder.AppendFormat("{0}:={1}\n", p.Key, p.Value));
            }

            growlNotification.Title = string.Format("{0} HealthCheck Result", notificationEvent.SiteId);
            growlNotification.Text = textBuilder.ToString();
            growlNotification.Priority = criticalFailure ? Priority.Emergency : Priority.Normal;
            growlNotification.Sticky = criticalFailure;
        }
    }
}