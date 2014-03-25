using System;
using Wolfpack.Periscope.Core.Interfaces;
using Wolfpack.Periscope.Core.Interfaces.Entities;

namespace Wolfpack.Periscope.Core.Repositories
{
    public class FileSystemDashboardRepository : IDashboardConfigurationRepository
    {
        public DashboardConfiguration Load()
        {
            throw new NotImplementedException();
        }

        public void Save(DashboardConfiguration configuration)
        {
            throw new NotImplementedException();
        }
    }
}