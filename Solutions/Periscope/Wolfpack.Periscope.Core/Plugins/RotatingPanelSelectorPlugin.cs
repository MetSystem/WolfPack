using System;
using Wolfpack.Periscope.Core.Infrastructure;
using Wolfpack.Periscope.Core.Interfaces;
using Wolfpack.Periscope.Core.Interfaces.Entities;
using Wolfpack.Periscope.Core.Interfaces.Events;
using System.Linq;

namespace Wolfpack.Periscope.Core.Plugins
{
    public class RotatingPanelSelectorPlugin : IDashboardPlugin
    {
        private readonly IDashboardInfrastructure _infrastructure;
        private readonly IDashboardConfigurationRepository _repository;

        private int _panelIndex;
        private IDashboardPanel _currentPanel; 
        private TinyMessageSubscriptionToken _subscriptionToken;
        private DashboardConfiguration _config;
        private DateTime _lastChangeTimestamp;

        public RotatingPanelSelectorPlugin(IDashboardInfrastructure infrastructure,
            IDashboardConfigurationRepository repository)
        {
            _infrastructure = infrastructure;
            _repository = repository;
        }

        public void Initialise()
        {
            _subscriptionToken = _infrastructure.MessageBus.Subscribe<ClockTickEvent>(TestForPanelRotate);
        }

        public void Start()
        {
            _config = _repository.Load();
            SetNextPanel();
            RaisePanelChangeEvent();
        }

        private void TestForPanelRotate(ClockTickEvent tick)
        {
            if (DateTime.UtcNow.Subtract(_lastChangeTimestamp).TotalSeconds < _currentPanel.DwellInSeconds)
                return;

            SetNextPanel();
            RaisePanelChangeEvent();            
        }

        private void RaisePanelChangeEvent()
        {            
            _infrastructure.MessageBus.Publish(new PanelChangeRequestEvent(this, _currentPanel));
            _lastChangeTimestamp = DateTime.UtcNow;
        }

        private void SetNextPanel()
        {
            var previousIndex = _panelIndex;
            _panelIndex = (_currentPanel == null) ? 0 : _config.Panels.Count() % ++_panelIndex;

            if ((previousIndex == _panelIndex) && (_currentPanel != null))
                return;

            _currentPanel = _config.Panels.OrderBy(p => p.Sequence).ElementAt(_panelIndex);            
        }

        public void Dispose()
        {
            if (_subscriptionToken != null)
                _subscriptionToken.Dispose();
        }
    }
}