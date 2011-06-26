using System;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Interfaces.Magnum;

namespace Wolfpack.Core.Publishers
{
    public abstract class FilteredResultPublisherBase<T> : PublisherBase, IHealthCheckResultPublisher
        where T: PluginConfigBase
    {
        private readonly string myMatchFriendlyName;
        private readonly Func<HealthCheckResult, bool> myFilter;
        protected abstract void Publish(HealthCheckResult buildResult);

        protected T Config { get; private set; }

        protected FilteredResultPublisherBase(T config,
            string friendlyName)
        {
            Config = config;
            Enabled = config.Enabled;
            FriendlyId = config.FriendlyId;

            myMatchFriendlyName = friendlyName;
            myFilter = MatchOnFriendlyName;
        }

        protected FilteredResultPublisherBase(T config,
            Func<HealthCheckResult, bool> filter)
        {
            Config = config;
            myFilter = filter;
        }

        public void Consume(HealthCheckResult message)
        {
            if (!myFilter(message))
                return;
            Publish(message);
        }

        protected virtual bool MatchOnFriendlyName(HealthCheckResult message)
        {
            return (string.Compare(message.Check.Identity.Name, myMatchFriendlyName,
                               StringComparison.InvariantCultureIgnoreCase) == 0);
        }
    }
}