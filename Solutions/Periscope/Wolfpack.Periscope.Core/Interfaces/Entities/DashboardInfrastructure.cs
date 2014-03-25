using System;
using System.Collections.Generic;
using Wolfpack.Periscope.Core.Infrastructure;
using System.Linq;
using Wolfpack.Periscope.Core.Interfaces.Infrastructure;

namespace Wolfpack.Periscope.Core.Interfaces.Entities
{
    public class DashboardInfrastructure : IConfigurableInfrastructure, IDashboardInfrastructure
    {
        private readonly List<Type> _pluginTypes;
        private readonly List<IDashboardPlugin> _plugins;
        private readonly List<Property> _includes;
        private readonly List<WidgetDefinition> _supportedWidgets;

        private List<IDashboardPlugin> _pluginCache;

        public DashboardInfrastructure(ILogger logger,
            IContainer container,
            ITinyMessengerHub messagebus)
        {
            Logger = logger;
            Container = container;
            MessageBus = messagebus;

            _plugins = new List<IDashboardPlugin>();
            _pluginTypes = new List<Type>();
            _includes = new List<Property>();
            _supportedWidgets = new List<WidgetDefinition>();
        }

        // add other config options here like theme etc...
        public ILogger Logger { get; private set; }
        public IContainer Container { get; private set; }
        public ITinyMessengerHub MessageBus { get; private set; }

        public List<Property> Includes { get { return _includes; } }
        public List<WidgetDefinition> SupportedWidgets { get { return _supportedWidgets; } }

        public void ResolvePlugins()
        {
            if (_pluginCache != null) 
                return;

            _pluginCache = new List<IDashboardPlugin>(_plugins);
            Container.RegisterMultiple<IDashboardPlugin>(_pluginTypes);
            _pluginCache = _pluginCache.Concat(Container.ResolveAll<IDashboardPlugin>()).ToList();
        }

        public IEnumerable<IDashboardPlugin> Plugins
        {
            get { return _pluginCache; }
        }

        public IConfigurableInfrastructure RegisterPlugin<T>(T instance) where T : class, IDashboardPlugin
        {
            _plugins.Add(instance);
            return this;
        }

        public IConfigurableInfrastructure RegisterPlugin<T>() where T : class, IDashboardPlugin
        {
            _pluginTypes.Add(typeof(T));
            return this;
        }

        public void Dispose()
        {
            _pluginCache.ForEach(p => p.Dispose());
        }
    }
}