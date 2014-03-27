using System;
using Wolfpack.Periscope.Core.Infrastructure;
using Wolfpack.Periscope.Core.Interfaces;
using Wolfpack.Periscope.Core.Interfaces.Entities;
using Wolfpack.Periscope.Core.Interfaces.Infrastructure;
using Wolfpack.Periscope.Core.Plugins;
using Wolfpack.Periscope.Core.Repositories;


namespace Wolfpack.Periscope.Core
{
    public class DashboardBootstrapper : IDashboardBoostrapper
    {
        protected virtual IContainer CreateContainer()
        {
            return new TinyContainer();
        }

        protected virtual ILogger CreateLogger()
        {
            return new TraceLogger();
        }

        protected virtual ITinyMessengerHub CreateMessageBus()
        {
            return new TinyMessengerHub();
        }

        protected virtual void RegisterPluginDependencies(IContainer container)
        {
            // hook if you need to register any "primary" infrastructure components
        }

        protected virtual void RegisterDefaultPlugins(IConfigurableInfrastructure infrastructure)
        {
            infrastructure.RegisterPlugin<ClockPlugin>();
        }

        public IDashboard Configure(Action<IConfigurableInfrastructure> configuratron)
        {
            // create infrastructure (configurable)
            var infrastructure = new DashboardInfrastructure(CreateLogger(), 
                CreateContainer(), 
                CreateMessageBus());
            RegisterDefaultPlugins(infrastructure);
            configuratron(infrastructure);            


            // create default registrations...
            infrastructure.Container.RegisterInstance<IDashboardInfrastructure>(infrastructure);
            infrastructure.Container.RegisterType<IDashboard, Dashboard>(
                () => !infrastructure.Container.IsRegistered<IDashboard>());
            infrastructure.Container.RegisterType<IDashboardConfigurationRepository, FileSystemDashboardRepository>(
                            () => !infrastructure.Container.IsRegistered<IDashboardConfigurationRepository>());

            // finally resolve plugins & the dashboard itself...
            infrastructure.ResolvePlugins();
            return infrastructure.Container.Resolve<IDashboard>();
        }
    }
}