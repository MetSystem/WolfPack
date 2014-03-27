using System;
using Wolfpack.Periscope.Core.Infrastructure;
using Wolfpack.Periscope.Core.Interfaces;
using Wolfpack.Periscope.Core.Interfaces.Entities;
using Wolfpack.Periscope.Core.Interfaces.Events;

namespace Wolfpack.Periscope.Core.Widgets
{
    public class DataPumpBootstrapper<T> : IWidgetBootstrapper
        where T: IWidgetDataPump
    {
        private TinyMessageSubscriptionToken _subscriptionToken;

        private readonly IDashboardInfrastructure _infrastructure;
        private readonly T _pump;
        private readonly WidgetConfiguration _targetMetadata;
        private readonly Action _callback;

        public DataPumpBootstrapper(IDashboardInfrastructure infrastructure,
            T pump, 
            WidgetConfiguration targetMetadata,
            Action callback)
        {
            _infrastructure = infrastructure;
            _pump = pump;
            _targetMetadata = targetMetadata;
            _callback = callback;
        }


        public void Execute()
        {            
            // only start the pump if there is a callback
            if (_callback == null)
                return;

            _subscriptionToken = _infrastructure.MessageBus.Subscribe<WidgetUpdateRequestEvent>(WidgetUpdateRequestHandler,
                MatchOnWidgetName);

            _pump.Initialise(_targetMetadata);
            _pump.Start();
        }

        public void Dispose()
        {
            if (_subscriptionToken != null)
                _subscriptionToken.Dispose();
        }

        private void WidgetUpdateRequestHandler(WidgetUpdateRequestEvent message)
        {
            _callback();
        }

        private bool MatchOnWidgetName(WidgetUpdateRequestEvent message)
        {
            return message.Content.Target.Equals(_targetMetadata.Name, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}