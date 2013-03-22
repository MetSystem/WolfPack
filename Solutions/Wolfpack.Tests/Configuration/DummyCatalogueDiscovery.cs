using System.Collections.Generic;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Tests.Configuration
{
    public class DummyCatalogueDiscovery : ISupportConfigurationDiscovery
    {
        public ConfigurationEntry GetConfigurationMetadata()
        {
            return new ConfigurationEntry
                       {
                           Name = "Test",
                           ConcreteType = "TestConcreteType",
                           InterfaceType = "TestInterfaceType",
                           Data = "TestSerialisation",
                           Tags = new List<string> {"Test"}
                       };
        }
    }
}