using System;
using Growl.Connector;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Growl.Formatters
{
    /// <summary>
    /// This formatter will set the Growl Notification Priority based on the Result.ResultCount property
    /// value. You can preset the notification priority should the result count value be lower or higher
    /// than the threshold value you also set
    /// </summary>
    public class GrowlCountFormatter : IGrowlNotificationFormatter
    {
        /// <summary>
        /// Set this number to split the priority - Higher and Lower
        /// </summary>
        public double Threshold { get; set; }
        /// <summary>
        /// The priority to set when the result count is higher or
        /// equal to the <see cref="Threshold"/> value
        /// </summary>
        public string HigherPriority { get; set; }
        /// <summary>
        /// The text to replace the default
        /// </summary>
        public string HigherMessage { get; set; }
        /// <summary>
        /// If you want the notification to stay put on 
        /// screen then set this to true
        /// </summary>
        public bool HigherIsSticky { get; set; }
        /// <summary>
        /// The priority to set when the result count is lower
        /// than the <see cref="Threshold"/> value
        /// </summary>
        public string LowerPriority { get; set; }
        /// <summary>
        /// The text to replace the default
        /// </summary>
        public string LowerMessage { get; set; }
        /// <summary>
        /// If you want the notification to stay put on 
        /// screen then set this to true
        /// </summary>
        public bool LowerIsSticky { get; set; }

        public string EventType { get; set; }

        public string Check { get; set; }

        public void Format(NotificationEvent notificationEvent, global::Growl.Connector.Notification growlNotification)
        {
            if (!notificationEvent.ResultCount.HasValue)
                return;

            var rc = notificationEvent.ResultCount;

            if (rc >= Threshold)
            {
                if (!string.IsNullOrEmpty(HigherPriority))
                    growlNotification.Priority = (Priority)Enum.Parse(typeof(Priority), HigherPriority, true);
                if (!string.IsNullOrEmpty(HigherMessage))
                    growlNotification.Text = HigherMessage;
                growlNotification.Sticky = HigherIsSticky;
            }
            else
            {
                if (!string.IsNullOrEmpty(LowerPriority))
                    growlNotification.Priority = (Priority)Enum.Parse(typeof(Priority), LowerPriority, true);
                if (!string.IsNullOrEmpty(LowerMessage))
                    growlNotification.Text = LowerMessage;
                growlNotification.Sticky = LowerIsSticky;
            }
        }
    }
}