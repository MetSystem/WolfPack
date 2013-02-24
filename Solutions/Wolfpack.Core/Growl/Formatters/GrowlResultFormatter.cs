using System;
using Growl.Connector;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Growl.Formatters
{
    /// <summary>
    /// This formatter will alter the Growl Notification Priority based on the result state.
    /// You can tune the Priority for the success and failure state of a result.
    /// </summary>
    public class GrowlResultFormatter : IGrowlNotificationFormatter
    {
        /// <summary>
        /// The priorty to set if the result is "success"
        /// </summary>
        public string OnSuccess { get; set; }
        /// <summary>
        /// If you want the notification to stay put on 
        /// screen then set this to true
        /// </summary>
        public bool SuccessIsSticky { get; set; }
        /// <summary>
        /// The text to override the default
        /// </summary>
        public string SuccessMessage { get; set; }
        /// <summary>
        /// The priority to set if the result is "failure"
        /// </summary>
        public string OnFailure { get; set; }
        /// <summary>
        /// If you want the notification to stay put on 
        /// screen then set this to true
        /// </summary>
        public bool FailureIsSticky { get; set; }
        /// <summary>
        /// The text to override the default
        /// </summary>
        public string FailureMessage { get; set; }

        public string EventType { get; set; }

        public string Check { get; set; }

        public void Format(NotificationEvent notificationEvent, global::Growl.Connector.Notification growlNotification)
        {
            if (!notificationEvent.Result.HasValue)
                return;
            if (notificationEvent.Result.Value)
            {
                if (!string.IsNullOrEmpty(OnSuccess))
                    growlNotification.Priority = (Priority)Enum.Parse(typeof(Priority), OnSuccess, true);
                if (!string.IsNullOrEmpty(SuccessMessage))
                    growlNotification.Text = SuccessMessage;
                growlNotification.Sticky = SuccessIsSticky;
            }
            else
            {
                if (!string.IsNullOrEmpty(OnFailure))
                    growlNotification.Priority = (Priority)Enum.Parse(typeof(Priority), OnFailure, true);
                if (!string.IsNullOrEmpty(FailureMessage))
                    growlNotification.Text = FailureMessage;
                growlNotification.Sticky = FailureIsSticky;
            }
        }
    }
}