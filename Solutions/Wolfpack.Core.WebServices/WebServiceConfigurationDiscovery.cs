using System.Collections.Generic;
using Wolfpack.Core.Configuration;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.WebServices.Interfaces.Entities;

namespace Wolfpack.Core.WebServices
{
    public class WebServiceConfigurationDiscovery : PluginDiscoveryBase<WebServiceActivityConfig>
    {
        protected override WebServiceActivityConfig GetConfiguration()
        {
            return new WebServiceActivityConfig
                       {
                           BaseUrl = "http://*:802/",
                           Enabled = true,
                           ApiKeys = new List<string> { "Leave blank to disable" }
                       };
        }

        protected override void Configure(ConfigurationEntry entry)
        {
            entry.Name = "WebServiceActivity";
            entry.Description = "This activity provides the Wolfpack Web Interface. This is a ServiceStack " + 
                "powered api that provides a REST interface and Razor view engine pages.";
            entry.Tags.AddIfMissing("Activity", "Api", "WebUI");
        }
    }

    public class WebServicePublisherConfigurationDiscovery : PluginDiscoveryBase<WebServicePublisherConfig>
    {
        protected override WebServicePublisherConfig GetConfiguration()
        {
            return new WebServicePublisherConfig
                       {
                           BaseUrl = "http://localhost:802/",
                           Enabled = true,
                           BaseFolder = "_outbox",
                           ApiKey = "Leave blank to disable",
                           FriendlyId = "CHANGEME!",
                           SendIntervalInSeconds = 10,
                           UserAgent = string.Empty
                       };
        }

        protected override void Configure(ConfigurationEntry entry)
        {
            entry.Name = "WebServicePublisher";
            entry.Description = "This activity is used to publish notifications to another Wolfpack instance via the Web REST Api.";
            entry.Tags.AddIfMissing("Activity", "WebService");
        }
    }
}