using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Growl.Connector;
using Wolfpack.Core.Growl.Formatters;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Interfaces.Magnum;
using Wolfpack.Core.Publishers;

namespace Wolfpack.Core.Growl
{
    public class GrowlPublisher : PublisherBase, INotificationEventPublisher
    {
        public const string DEFAULT_ICON = @"growl\growl.monitor.png";


        private readonly IEnumerable<IGrowlNotificationFormatter> _formatters;
        private readonly GrowlConfiguration _config;

        private GrowlConnector _growl;

        private static IEnumerable<IGrowlNotificationFormatter> _defaultFormatters;

        public GrowlPublisher(IEnumerable<IGrowlNotificationFormatter> formatters,
            GrowlConfiguration config)
        {
            _config = config;
            _formatters = formatters;
            _defaultFormatters = new IGrowlNotificationFormatter[]
                                     {
                                         new DefaultGrowlAgentStartFormatter(), 
                                         new DefaultGrowlHealthCheckFormatter()
                                     };


            Enabled = config.Enabled;
            FriendlyId = config.FriendlyId;            
        }

        public override void Initialise()
        {
            if (!string.IsNullOrEmpty(_config.Password))
            {
                _growl = !string.IsNullOrEmpty(_config.Hostname)
                                  ? new GrowlConnector(_config.Password, _config.Hostname, _config.Port)
                                  : new GrowlConnector(_config.Password);
            }
            else
                _growl = new GrowlConnector();

            var application = new Application(_config.AppId);

            // use the default icon if no override set
            if (string.IsNullOrEmpty(_config.IconFile))
                _config.IconFile = DEFAULT_ICON;
            var icon = new SmartLocation(_config.IconFile);
            if (File.Exists(icon.Location))
                application.Icon = icon.Location;

            var healthCheck = new NotificationType(_config.NotificationId, _config.NotificationTitle);
            _growl.Register(application, new[] { healthCheck });
        }

        public void Consume(NotificationEvent message)
        {
            var notification = new global::Growl.Connector.Notification(_config.AppId,
                                                                        _config.NotificationId,
                                                                        null,
                                                                        string.Format("{0} Notification", message.EventType),
                                                                        "Notification Text Not Set!");

            SetDefaults(message, notification);
            RunAdditionalFormatters(message, notification);

            // remove CR from text as Growl doesn't like these...
            notification.Text = notification.Text.Replace("\r", string.Empty);
            _growl.Notify(notification);            
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