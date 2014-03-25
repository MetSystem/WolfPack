using Wolfpack.Periscope.Core.Interfaces.Entities;

namespace Wolfpack.Periscope.Core.Interfaces
{
    public interface IDashboardConfigurationRepository
    {
        DashboardConfiguration Load();
        void Save(DashboardConfiguration configuration);
    }
}