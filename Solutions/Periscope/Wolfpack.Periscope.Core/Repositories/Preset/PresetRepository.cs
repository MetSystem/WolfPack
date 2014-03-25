using Wolfpack.Periscope.Core.Interfaces;
using Wolfpack.Periscope.Core.Interfaces.Entities;

namespace Wolfpack.Periscope.Core.Repositories.Preset
{
    public class PresetRepository : IDashboardConfigurationRepository
    {
        private readonly IDashboardInfrastructure _infrastructure;
        private readonly DashboardConfigurationBuilder _builder;

        public PresetRepository(IDashboardInfrastructure infrastructure,
            DashboardConfigurationBuilder builder)
        {
            _infrastructure = infrastructure;
            _builder = builder;
        }

        public DashboardConfiguration Load()
        {
            return _builder.Build(_infrastructure);
        }

        public void Save(DashboardConfiguration configuration)
        {
            throw new System.NotImplementedException();
        }
    }
}