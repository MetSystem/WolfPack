using System;
using System.Collections.Generic;
using Wolfpack.Periscope.Core.Infrastructure;
using Wolfpack.Periscope.Core.Interfaces.Entities;
using Wolfpack.Periscope.Core.Interfaces.Infrastructure;

namespace Wolfpack.Periscope.Core.Interfaces
{
    public interface IDashboardInfrastructureCore
    {
        ILogger Logger { get; }
        IContainer Container { get; }
        ITinyMessengerHub MessageBus { get; }

        List<Property> Includes { get; }
        List<WidgetDefinition> SupportedWidgets { get; }
    }

    public interface IDashboardInfrastructure : IDashboardInfrastructureCore, IDisposable
    {
        IEnumerable<IDashboardPlugin> Plugins { get; } 
    }

    public interface IConfigurableInfrastructure : IDashboardInfrastructureCore
    {
        IConfigurableInfrastructure RegisterPlugin<T>(T instance) where T: class, IDashboardPlugin;
        IConfigurableInfrastructure RegisterPlugin<T>() where T: class, IDashboardPlugin;
    }
}