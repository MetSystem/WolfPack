using System;
using Wolfpack.Core;
using Wolfpack.Core.Growl;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Castle;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Interfaces.Magnum;
using Wolfpack.Core.Loaders;

namespace Wolfpack.Agent.Profiles
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultAgentProfile : ProfileBase
    {
        public override string Name
        {
            get { return "DefaultAgent"; }
        }

        public virtual Type DefineRole()
        {
            return typeof (Roles.Agent);
        }

        public override void CustomiseRole()
        {            
            Container.RegisterAllWithInterception<IGrowlNotificationFinaliser, IGrowlNotificationFinaliserInterceptor>()
                .RegisterAll<IHealthCheckSessionPublisher>()
                .RegisterAllWithInterception<IHealthCheckResultPublisher, IPublisherFilter>()
                .RegisterAll<IActivityPlugin>()
                .RegisterAsSingleton<ILoader<BindingConfiguration>>(typeof(DefaultBindingConfigurationLoader))
                .RegisterAsSingleton<ILoader<IHealthCheckSchedulerPlugin>>(typeof(HealthCheckLoader))
                .RegisterAsSingleton<ILoader<IActivityPlugin>>(typeof(ContainerPluginLoader<IActivityPlugin>))
                .RegisterAsSingleton<ILoader<IHealthCheckSessionPublisher>>(typeof(ContainerPluginLoader<IHealthCheckSessionPublisher>))
                .RegisterAsSingleton<ILoader<IHealthCheckResultPublisher>>(typeof(ContainerPluginLoader<IHealthCheckResultPublisher>))
                .RegisterAsSingleton<IRolePlugin>(DefineRole());

            Messenger.Initialise(new MagnumMessenger());
        }
    }
}