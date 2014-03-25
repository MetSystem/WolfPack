using System;
using Wolfpack.Periscope.Core.Interfaces.Entities;

namespace Wolfpack.Periscope.Core.Interfaces
{
    public interface IPanelSelector
    {
        DashboardPanelConfiguration Next(Guid? current);
    }
}