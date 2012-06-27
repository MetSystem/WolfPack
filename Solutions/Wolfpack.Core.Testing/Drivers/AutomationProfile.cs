using System;
using System.Threading;
using Wolfpack.Core.Geo;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Interfaces.Magnum;

namespace Wolfpack.Core.Testing.Drivers
{
    /// <summary>
    /// This role profile allows you to programmatically add implementations to the IoC
    /// container which are later used by the role component
    /// </summary>
    public class AutomationProfile : IRoleProfile
    {
        protected IRolePlugin myRole;
        protected INotificationHub myNotificationHub;
        protected IGeoLocator myGeoLocator;
        protected AutomationLoader<IStartupPlugin> myStartupLoader;
        protected AutomationLoader<IHealthCheckSessionPublisher> mySessionPublisherLoader;
        protected AutomationLoader<IHealthCheckResultPublisher> myResultPublisherLoader;
        protected AutomationLoader<IHealthCheckSchedulerPlugin> myCheckLoader;
        protected AutomationLoader<IActivityPlugin> myActivityLoader;
        
        protected ManualResetEventSlim myWaitGate;

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
            myWaitGate = new ManualResetEventSlim(false);
            myStartupLoader = new AutomationLoader<IStartupPlugin>();
            myActivityLoader = new AutomationLoader<IActivityPlugin>();
            myCheckLoader = new AutomationLoader<IHealthCheckSchedulerPlugin>();
            myResultPublisherLoader = new AutomationLoader<IHealthCheckResultPublisher>();
            mySessionPublisherLoader = new AutomationLoader<IHealthCheckSessionPublisher>();
            myNotificationHub = new NotificationHub(new StaticGeoLocator(new AgentConfiguration
                                                                             {
                                                                                 Latitude = "12.34", 
                                                                                 Longitude = "56.78",
                                                                                 SiteId = "AutomationProfile"
                                                                             }));
        }

        public static AutomationProfile Configure()
        {
           return new AutomationProfile(); 
        }

        public AutomationProfile Run(IStartupPlugin plugin)
        {
            myStartupLoader.Add(plugin);
            return this;
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

        public AutomationProfile Run(INotificationHub plugin)
        {
            myNotificationHub = plugin;
            return this;
        }

        public AutomationProfile Start()
        {
            RunStartupPlugins();

            Messenger.Initialise(new MagnumMessenger());

            myRole = new Agent.Roles.Agent(new AgentConfiguration
                                               {
                                                   SiteId = "Test"
                                               },
                                           mySessionPublisherLoader,
                                           myResultPublisherLoader,
                                           myCheckLoader,
                                           myActivityLoader,
                                           myNotificationHub);
            myRole.Start();
            return this;
        }

        protected virtual void RunStartupPlugins()
        {
            IStartupPlugin[] plugins;

            if (!myStartupLoader.Load(out plugins, p => p.InitialiseIfEnabled()))
                return;
        }

        public void WaitUntil(string description, int seconds, Func<bool> check)
        {
            for (var i = 0; i < seconds; i++)
            {
                myWaitGate.Wait(1000);
                if (check())
                    return;
            }

            throw new TimeoutException(string.Format("Timed out after {0}s waiting for event '{1}'", seconds, description));
        }
    }
}