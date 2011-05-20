using Wolfpack.Core;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Interfaces.Magnum;
using Wolfpack.Core.Loaders;

namespace Wolfpack.Tests.Drivers
{
    /// <summary>
    /// This role profile allows you to programmatically add implementations to the IoC
    /// container which are later used by the role component
    /// </summary>
    public class AutomationProfile : IRoleProfile
    {
        public string Name
        {
            get { return GetType().Name; }
        }

        public IRolePlugin Role
        {
            get 
            {
                return new Agent.Roles.Agent(new AgentConfiguration
                                                 {
                                                     SiteId = "Test"
                                                 },
                                             new ContainerLoader<IHealthCheckSessionPublisher>(),
                                             new ContainerLoader<IHealthCheckResultPublisher>(),
                                             new ContainerLoader<IHealthCheckSchedulerPlugin>(),
                                             new ContainerLoader<IActivityPlugin>());
            }
        }

        public static AutomationProfile Configure()
        {
           return new AutomationProfile(); 
        }

        public AutomationProfile AddHealthCheck<T>() where T : IHealthCheckPlugin
        {
            Container.RegisterAsTransient(typeof(T));
            return this;
        }

        public AutomationProfile AddPublisher<T>() where T : IHealthCheckSessionPublisher, IHealthCheckResultPublisher
        {
            Container.RegisterAsTransient(typeof(T));
            return this;
        }
    }
}