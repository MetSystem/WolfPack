using System;
using System.Collections.Generic;
using Wolfpack.Periscope.Core.Interfaces.Entities;

namespace Wolfpack.Periscope.Core.Interfaces
{
    public interface IDashboard : IDisposable
    {
        IDashboardInfrastructure Infrastructure { get; }
        IDashboardPanel CurrentPanel { get; }
        IEnumerable<WidgetDefinition> SupportedWidgets { get; }
        void Start();
    }
}