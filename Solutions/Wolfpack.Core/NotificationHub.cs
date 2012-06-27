using System.Collections.Generic;
using Magnum.Pipeline;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;
using Castle.Core;

namespace Wolfpack.Core
{
    public class NotificationHub : INotificationHub, IConsumer<HealthCheckData>
    {
        protected AgentInfo myAgentInfo;

        protected Dictionary<string, INotificationModeFilter> myFilters;

        protected readonly IGeoLocator myGeoLocator;

        public NotificationHub(IGeoLocator geoLocator)
        {
            myGeoLocator = geoLocator;
        }

        public void Initialise(AgentInfo info)
        {
            myAgentInfo = info;

            LoadFilters();

            // listen for result messages being published
            // by the checks & activities - route to the
            // publisher-hub so it can decide how to handle them
            Messenger.Subscribe(this);
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadFilters()
        {
            myFilters = new Dictionary<string, INotificationModeFilter>();
            var filters = Container.ResolveAll<INotificationModeFilter>();

            // could use filters.ToDictionary() but I want a much richer
            // error message to aide debugging in the case of duplicate
            // filters..

            filters.ForEach(filter =>
                                {
                                    if (myFilters.ContainsKey(filter.Mode))
                                    {
                                        Logger.Info("");
                                        return;
                                    }

                                    myFilters.Add(filter.Mode.ToLower(), filter);
                                });

            Logger.Info("Loaded {0} Notification filters...", myFilters.Count);
            myFilters.ForEach(filter => Logger.Debug("\t'{0}' => {1}.{2}", filter.Key, 
                filter.Value.GetType().Namespace,
                filter.Value.GetType().Name));
        }

        public void ForwardToPublishers(HealthCheckData message)
        {
            // wrap the data with the agent information
            // to produce a "result"
            var result = new HealthCheckResult
            {
                Agent = myAgentInfo,
                Check = message
            };

            var checkId = "[Unknown]";
            var requiredFilter = string.Empty;

            // if the geo data has not already been set by the health check
            if (result.Check != null)
            {
                checkId = result.Check.Identity.Name;
                requiredFilter = result.Check.NotificationMode;

                if (result.Check.Geo == null)
                    result.Check.Geo = myGeoLocator.Locate();
            }

            if (string.IsNullOrWhiteSpace(requiredFilter))
            {
                Logger.Info("Check '{0}' has no NotificationMode set...publishing anyway!", checkId);
                Messenger.Publish(result);
                return;
            }

            INotificationModeFilter filter;

            if (!SelectFilter(requiredFilter, out filter))
            {
                Logger.Info("Check '{0}' has NotificationMode '{1}' but no matching filter is loaded, publishing anyway!", checkId, requiredFilter);
                Messenger.Publish(result);
                return;
            }

            Logger.Debug("Check '{0}' matched Notification Filter '{1}'", checkId, requiredFilter);
            filter.Execute(result);
        }

        public void Consume(HealthCheckData message)
        {
            ForwardToPublishers(message);
        }

        private bool SelectFilter(string filterId, out INotificationModeFilter filter)
        {
            filter = myFilters.ContainsKey(filterId.ToLower()) ? myFilters[filterId.ToLower()] : null;
            return (filter != null);
        }
    }
}