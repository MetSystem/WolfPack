using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wolfpack.Periscope.Core.Infrastructure;
using Wolfpack.Periscope.Core.Interfaces;
using Wolfpack.Periscope.Core.Interfaces.Entities;
using System.Linq;
using Wolfpack.Periscope.Core.Extensions;
using Wolfpack.Periscope.Core.Interfaces.Events;

namespace Wolfpack.Periscope.Core
{    
    public class Dashboard : IDashboard
    {
        private readonly IDashboardInfrastructure _infrastructure;
        private readonly IEnumerable<IWidgetGallery> _galleries;

        private TinyMessageSubscriptionToken _subscriptionToken;
        private IDashboardPanel _currentPanel;

        public Dashboard(IDashboardInfrastructure infrastructure,
         IEnumerable<IWidgetGallery> galleries)
        {
            _infrastructure = infrastructure;
            _galleries = galleries;            
        }

        public IDashboardInfrastructure Infrastructure { get { return _infrastructure; }}

        public IDashboardPanel CurrentPanel { get { return _currentPanel; }}

        public IEnumerable<WidgetDefinition> SupportedWidgets
        {
            get { return _galleries.SelectMany(g => g.Get())
                .Concat(_infrastructure.SupportedWidgets)
                .Distinct((a,b) => a.Type.Equals(b.Type, StringComparison.OrdinalIgnoreCase)); }
        }

        public void Start()
        {
            _subscriptionToken = _infrastructure.MessageBus.Subscribe<PanelChangeRequestEvent>(HandlePanelChangeEvent);
            Parallel.ForEach(_infrastructure.Plugins, plugin => plugin.Initialise());
            Parallel.ForEach(_infrastructure.Plugins, plugin => plugin.Start());
        }

        private void HandlePanelChangeEvent(PanelChangeRequestEvent args)
        {
            _currentPanel = args.Content;
            _infrastructure.MessageBus.PublishAsync(new PanelChangedEvent(this, _currentPanel));
        }

        public void Dispose()
        {
            _subscriptionToken.Dispose();
            _infrastructure.Dispose();
        }
    }
}