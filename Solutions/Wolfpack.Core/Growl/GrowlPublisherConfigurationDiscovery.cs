using Wolfpack.Core.Configuration;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Growl
{
    public class GrowlPublisherConfigurationDiscovery : PluginDiscoveryBase<GrowlConfiguration>
    {
        protected override GrowlConfiguration GetConfiguration()
        {
            return new GrowlConfiguration
                       {
                           AppId = "Wolfpack",
                           Enabled = true,
                           FriendlyId = "CHANGEME!",
                           IconFile = "growl\agent.png",
                           NotificationId = "HealthCheck",
                           NotificationTitle = "Platform HealthCheck",
                           Hostname = "",
                           Password = "",
                           Port = 23053
                       };
        }

        protected override void Configure(ConfigurationEntry entry)
        {
            entry.Name = "GrowlPublisher";
            entry.Description = "This will publish Notifications to Growl.";
            entry.Tags.AddIfMissing(PluginTypes.Publisher, "Growl");
        }
    }
}