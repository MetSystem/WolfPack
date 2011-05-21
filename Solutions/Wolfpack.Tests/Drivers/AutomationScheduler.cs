using System;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Tests.Drivers
{
    public class AutomationScheduler : IHealthCheckSchedulerPlugin
    {
        public bool StartCalled { get; set; }
        public bool StopCalled { get; set; }
        public bool InitialiseCalled { get; set; }
        public IHealthCheckPlugin Check { get; set; }

        public Status Status { get; set;}

        public AutomationScheduler(IHealthCheckPlugin check)
        {
            Check = check;
        }

        public void Initialise()
        {
            InitialiseCalled = true;
        }

        public PluginDescriptor Identity
        {
            get { return Check.Identity; }
        }

        public void Start()
        {
            StartCalled = true;
        }

        public void Stop()
        {
            StopCalled = true;
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Continue()
        {
            throw new NotImplementedException();
        }
    }
}