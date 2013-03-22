using System.Collections.Generic;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Configuration
{
    public abstract class PluginDiscoveryBase<T> : ISupportConfigurationDiscovery
    {
        protected abstract T GetConfiguration();
        protected abstract void Configure(ConfigurationEntry entry);

        public virtual Properties GetRequiredProperties()
        {
            return new Properties
                       {
                           {ConfigurationEntry.RequiredPropertyNames.Name, typeof (T).Name}
                       };
        }

        public virtual List<string> GetTags()
        {
            return new List<string>();
        }

        public virtual ConfigurationEntry GetConfigurationMetadata()
        {
            var entry = new ConfigurationEntry
                            {
                                Tags = GetTags(),
                                RequiredProperties = GetRequiredProperties(),
                                Data = Serialiser.ToJson(GetConfiguration()),
                                ConcreteType = typeof (T).AssemblyQualifiedName
                            };

            Configure(entry);
            return entry;
        }
    }
}