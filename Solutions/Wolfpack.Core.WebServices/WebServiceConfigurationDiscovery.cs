using System.Collections.Generic;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.WebServices.Interfaces.Entities;

namespace Wolfpack.Core.WebServices
{
    public class WebServiceConfigurationDiscovery : ISupportConfigurationDiscovery
    {
        public ConfigurationEntry GetConfigurationMetadata()
        {
            var template = new WebServiceActivityConfig
                               {
                                   BaseUrl = "http://*:802/",
                                   Enabled = true
                               };

            return new ConfigurationEntry
                       {
                           Name = "WebServiceActivity",
                           Description = "This activity provides the Wolfpack Web Interface. This is a ServiceStack powered api that provides a REST interface and Razor view engine pages.",
                           ConcreteType = template.GetType().AssemblyQualifiedName,
                           Data = Serialiser.ToJson(template),
                           Tags = new List<string> { "Activity" } 
                       };
        }        
    }

    public class WebServicePublisherConfigurationDiscovery : ISupportConfigurationDiscovery
    {
        public ConfigurationEntry GetConfigurationMetadata()
        {
            var template = new WebServicePublisherConfig
                               {
                                   BaseUrl = "http://*:802/",
                                   Enabled = true,
                                   BaseFolder = "_outbox"
                               };

            return new ConfigurationEntry
                       {
                           Name = "WebServicePublisher",
                           Description = "This activity is used to publish notifications to another Wolfpack instance.",
                           ConcreteType = template.GetType().AssemblyQualifiedName,
                           Data = Serialiser.ToJson(template),
                           Tags = new List<string> { "Activity", "Publisher" }
                       };
        }
    }
}