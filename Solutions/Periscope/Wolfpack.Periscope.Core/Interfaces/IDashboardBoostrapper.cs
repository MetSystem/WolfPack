using System;

namespace Wolfpack.Periscope.Core.Interfaces
{
    public interface IDashboardBoostrapper
    {
        IDashboard Configure(Action<IConfigurableInfrastructure> configuratron);
    }
}