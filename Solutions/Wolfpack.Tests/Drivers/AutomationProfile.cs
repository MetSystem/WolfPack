using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Interfaces.Magnum;

namespace Wolfpack.Tests.Drivers
{
    /// <summary>
    /// This role profile allows you to programmatically add implementations to the IoC
    /// container which are later used by the role component
    /// </summary>
    public class AutomationProfile : IRoleProfile
    {
        protected IRolePlugin myRole;
        protected AutomationLoader<IHealthCheckSessionPublisher> mySessionPublisherLoader;
        protected AutomationLoader<IHealthCheckResultPublisher> myResultPublisherLoader;
        protected AutomationLoader<IHealthCheckSchedulerPlugin> myCheckLoader;
        protected AutomationLoader<IActivityPlugin> myActivityLoader;

        public string Name
        {
            get { return GetType().Name; }
        }

        public IRolePlugin Role
        {
            get { return myRole; }
        }

        private AutomationProfile()
        {
            myActivityLoader = new AutomationLoader<IActivityPlugin>();
            myCheckLoader = new AutomationLoader<IHealthCheckSchedulerPlugin>();
            myResultPublisherLoader = new AutomationLoader<IHealthCheckResultPublisher>();
            mySessionPublisherLoader = new AutomationLoader<IHealthCheckSessionPublisher>();
        }

        public static AutomationProfile Configure()
        {
           return new AutomationProfile(); 
        }

        public AutomationProfile Run(IHealthCheckSchedulerPlugin plugin)
        {
            myCheckLoader.Add(plugin);
            return this;
        }

        public AutomationProfile Run(IActivityPlugin plugin)
        {
            myActivityLoader.Add(plugin);
            return this;
        }

        public AutomationProfile Run(IHealthCheckSessionPublisher plugin)
        {
            mySessionPublisherLoader.Add(plugin);
            return this;
        }

        public AutomationProfile Run(IHealthCheckResultPublisher plugin)
        {
            myResultPublisherLoader.Add(plugin);
            return this;
        }

        public AutomationProfile Start()
        {
            myRole = new Agent.Roles.Agent(new AgentConfiguration
                                               {
                                                   SiteId = "Test"
                                               },
                                           mySessionPublisherLoader,
                                           myResultPublisherLoader,
                                           myCheckLoader,
                                           myActivityLoader);

            return this;
        }
    }
}