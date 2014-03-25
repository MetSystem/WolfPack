using Wolfpack.Periscope.Core;
using Wolfpack.Periscope.Core.Interfaces;
using Wolfpack.Periscope.Core.Repositories.Preset;

namespace Wolfpack.Periscope.Tests.Shims
{
    public class AutomationBootstrapper : DashboardBootstrapper
    {
        public DashboardConfigurationBuilder Builder { get; private set; }

        public AutomationBootstrapper()
        {
            Builder = DashboardConfigurationBuilder.New();
        }

        protected override void RegisterRepository(IConfigurableInfrastructure infrastructure)
        {
            infrastructure.Container.RegisterInstance(Builder);
            infrastructure.Container.RegisterType<IDashboardConfigurationRepository, PresetRepository>();
        }
    }
}