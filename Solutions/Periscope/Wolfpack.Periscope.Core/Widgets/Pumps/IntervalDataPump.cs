using System;
using Wolfpack.Periscope.Core.Infrastructure;
using Wolfpack.Periscope.Core.Interfaces;
using Wolfpack.Periscope.Core.Interfaces.Entities;
using Wolfpack.Periscope.Core.Interfaces.Events;

namespace Wolfpack.Periscope.Core.Widgets.Pumps
{
    public class IntervalDataPump : IWidgetDataPump
    {
        private readonly IDashboardInfrastructure _infrastructure;
        private readonly TimeSpan _interval;

        private WidgetConfiguration _target;
        private TinyMessageSubscriptionToken _subscriptionToken;
        private DateTime _lastFired;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="infrastructure"></param>
        /// <param name="interval"></param>
        public IntervalDataPump(IDashboardInfrastructure infrastructure, 
            TimeSpan interval)
        {
            _infrastructure = infrastructure;
            _interval = interval;
        }

        public void Initialise(WidgetConfiguration target)
        {
            _target = target;            
            _lastFired = DateTime.UtcNow;
        }

        public void Start()
        {
            _subscriptionToken = _infrastructure.MessageBus.Subscribe<ClockTickEvent>(ClockTickHandler);
        }

        public void Dispose()
        {
            _subscriptionToken.Dispose();
        }

        private void ClockTickHandler(ClockTickEvent args)
        {
            var now = args.Content;

            if (now.Subtract(_lastFired) < _interval)
                return;

            _infrastructure.Logger.LogDebug("IntervalPump firing: {0}", _target.Name, now.ToLongTimeString());
            
            _infrastructure.MessageBus.PublishAsync(new WidgetUpdateRequestEvent(this, new WidgetUpdateRequest
            {
                Target = _target.Name,
                Timestamp = now
            }));

            _lastFired = now;
        }
    }
}