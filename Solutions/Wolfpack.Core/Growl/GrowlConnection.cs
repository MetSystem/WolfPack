using System.IO;
using Growl.Connector;

namespace Wolfpack.Core.Growl
{
    public class GrowlConnection : IGrowlConnection
    {
        public const string DEFAULT_ICON = @"growl\growl.monitor.png";

        protected readonly GrowlConfiguration _config;
        protected readonly GrowlConnector _connector;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public GrowlConnection(GrowlConfiguration config)
        {
            _config = config;

            // create the growl connection and register it            
            if (!string.IsNullOrEmpty(config.Password))
            {
                _connector = !string.IsNullOrEmpty(config.Hostname)
                                  ? new GrowlConnector(config.Password, config.Hostname, config.Port)
                                  : new GrowlConnector(config.Password);
            }
            else
                _connector = new GrowlConnector();

            var application = new Application(config.AppId);

            // use the defaul icon if no override set
            if (string.IsNullOrEmpty(config.IconFile))
                config.IconFile = DEFAULT_ICON;
            var icon = new SmartLocation(config.IconFile);
            if (File.Exists(icon.Location))
                application.Icon = icon.Location;

            var healthCheck = new NotificationType(config.NotificationId, config.NotificationTitle);
            _connector.Register(application,
                                 new[]
                                         {
                                             healthCheck
                                         });            

        }

        public GrowlConnector Connection
        {
            get { return _connector; }
        }

        public GrowlConfiguration Config
        {
            get { return _config; }
        }
    }
}