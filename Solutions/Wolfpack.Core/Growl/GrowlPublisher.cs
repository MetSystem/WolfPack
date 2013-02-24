using System;
using System.Collections.Generic;
using System.Linq;
using Wolfpack.Core.Growl.Formatters;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Interfaces.Magnum;
using Wolfpack.Core.Publishers;

namespace Wolfpack.Core.Growl
{
    public class GrowlPublisher : PublisherBase, INotificationEventPublisher
    {
        protected readonly GrowlConfiguration _config;
        protected readonly IGrowlConnection _growler;
        private readonly IEnumerable<IGrowlNotificationFormatter> _formatters;
        private static IEnumerable<IGrowlNotificationFormatter> _defaultFormatters;

        public GrowlPublisher(IEnumerable<IGrowlNotificationFormatter> formatters,
            GrowlConfiguration config,
            IGrowlConnection connection           
            )
        {
            _config = config;
            _growler = connection;
            _formatters = formatters;
            //_formatters = new IGrowlNotificationFormatter[0];
            _defaultFormatters = new IGrowlNotificationFormatter[]
                                     {
                                         new DefaultGrowlAgentStartFormatter(), 
                                         new DefaultGrowlHealthCheckFormatter()
                                     };


            Enabled = config.Enabled;
            FriendlyId = config.FriendlyId;            
        }

        public void Consume(NotificationEvent message)
        {
            var notification = new global::Growl.Connector.Notification(_growler.Config.AppId,
                                                                        _growler.Config.NotificationId,
                                                                        null,
                                                                        string.Format("{0} Notification", message.EventType),
                                                                        "Notification Text Not Set!");

            SetDefaults(message, notification);
            RunAdditionalFormatters(message, notification);

            // remove CR from text as Growl doesn't like these...
            notification.Text = notification.Text.Replace("\r", string.Empty);
            _growler.Connection.Notify(notification);            
        }

        private void SetDefaults(NotificationEvent message, global::Growl.Connector.Notification notification)
        {
            var formatter = _defaultFormatters.FirstOrDefault(f => string.Equals(f.EventType, message.EventType, 
                StringComparison.OrdinalIgnoreCase)) ?? new DefaultGrowlUniversalFormatter();
            formatter.Format(message, notification);
        }

        private void RunAdditionalFormatters(NotificationEvent message, global::Growl.Connector.Notification notification)
        {
            // no ordering (no requirement yet)
            var formatters = _formatters.Where(f =>
                                               (string.Equals(f.EventType, "*", StringComparison.OrdinalIgnoreCase) ||
                                                string.Equals(f.EventType, message.EventType, StringComparison.OrdinalIgnoreCase)) &&
                                               (string.Equals(f.Check, "*", StringComparison.OrdinalIgnoreCase) ||
                                                string.Equals(f.Check, message.CheckId, StringComparison.OrdinalIgnoreCase)));


            foreach (var formatter in formatters)
            {
                formatter.Format(message, notification);
            }
        }
    }
}