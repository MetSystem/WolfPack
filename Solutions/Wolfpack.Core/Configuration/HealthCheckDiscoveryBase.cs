using System;
using System.Collections.Generic;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Configuration
{
    public abstract class HealthCheckDiscoveryBase<T> : PluginDiscoveryBase<T>
    {
        public override List<string> GetTags()
        {
            return new List<string>{ PluginTypes.HealthCheck };
        }

        public override Properties GetRequiredProperties()
        {
            var props = base.GetRequiredProperties();
            props.AddIfMissing(Tuple.Create(ConfigurationEntry.RequiredPropertyNames.Scheduler, "** CHANGEME ** (connects this check with an existing schedule)"));
            return props;
        }
    }
}