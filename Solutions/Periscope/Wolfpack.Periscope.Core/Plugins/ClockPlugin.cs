using Wolfpack.Periscope.Core.Infrastructure;
using Wolfpack.Periscope.Core.Interfaces;
using Wolfpack.Periscope.Core.Interfaces.Events;
using Wolfpack.Periscope.Core.Interfaces.Infrastructure;

namespace Wolfpack.Periscope.Core.Plugins
{
    public class ClockPlugin : IDashboardPlugin
    {
        private readonly IClock _clock;
        private readonly IDashboardInfrastructure _infrastructure;
        
        public ClockPlugin(IDashboardInfrastructure infrastructure)
        {
            _infrastructure = infrastructure;
            _clock = new DefaultClock();
        }

        public void Initialise()
        {
        }

        public void Start()
        {
            _clock.Start(RaiseTickEvent);
        }

        private void RaiseTickEvent()
        {
            _infrastructure.MessageBus.Publish(new ClockTickEvent(this));
        }

        public void Dispose()
        {            
        }
    }
}