using System.Collections.Generic;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Configuration
{
    public abstract class HealthCheckDiscoveryBase<T> : ISupportConfigurationDiscovery
    {
        protected abstract T GetConfiguration();
        protected abstract void Configure(ConfigurationEntry entry);

        public ConfigurationEntry GetConfigurationMetadata()
        {
            var requiredProps = new Properties
                                    {
                                        {ConfigurationEntry.RequiredPropertyNames.Name, typeof(T).Name },
                                        {ConfigurationEntry.RequiredPropertyNames.Scheduler, "** CHANGEME ** (connects this check with an existing schedule)"}
                                    };

            var entry = new ConfigurationEntry
                            {
                                Tags = new List<string>
                                           {
                                               ConfigurationEntry.Types.HealthCheck
                                           },
                                RequiredProperties = requiredProps,
                                Data = Serialiser.ToJson(GetConfiguration()),
                                ConcreteType = typeof (T).AssemblyQualifiedName
                            };

            Configure(entry);
            return entry;
        }
    }
}