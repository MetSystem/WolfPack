using System;
using System.Collections.Generic;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Configuration
{
    public abstract class PluginDiscoveryBase<TConfig, TPlugin> : ISupportConfigurationDiscovery
    {
        protected abstract TConfig GetConfiguration();
        protected abstract void Configure(ConfigurationEntry entry);

        public virtual Properties GetRequiredProperties()
        {
            return new Properties
                       {
                           {ConfigurationEntry.RequiredPropertyNames.Name, typeof (TConfig).Name}
                       };
        }

        public virtual List<string> GetTags()
        {
            return new List<string>();
        }

        public virtual string GetLink()
        {
            return string.Empty;
        }

        public virtual ConfigurationEntry GetConfigurationMetadata()
        {
            var entry = new ConfigurationEntry
                            {
                                Tags = GetTags(),
                                Link = GetLink(),
                                RequiredProperties = GetRequiredProperties(),
                                Data = Serialiser.ToJson(GetConfiguration()),
                                ConfigurationType = BuildTypeName<TConfig>(),
                                PluginType = BuildTypeName<TPlugin>()
                            };

            Configure(entry);
            return entry;
        }

        private static string BuildTypeName<T>()
        {
            var type = typeof (T);
            return string.Format("{0}, {1}", type.FullName, type.Assembly.GetName().Name);
        }
    }
}