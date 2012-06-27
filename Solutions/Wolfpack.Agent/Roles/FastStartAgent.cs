using System;
using System.Threading;
using Wolfpack.Core;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Interfaces.Magnum;

namespace Wolfpack.Agent.Roles
{
    /// <summary>
    /// Wraps the standard agent with an new start() method implementation that 
    /// runs the default start() method in another thread thus returning control
    /// back to the (Windows) Service Control Manager (SCM) quicker - hopefully 
    /// stopping SCM timeouts & service startup failures
    /// </summary>
    public class FastStartAgent : Agent
    {
        protected ManualResetEvent myStartingGate;

        public FastStartAgent(AgentConfiguration config,
            ILoader<IHealthCheckSessionPublisher> sessionPublisherLoader,
            ILoader<IHealthCheckResultPublisher> resultPublisherLoader,
            ILoader<IHealthCheckSchedulerPlugin> checksLoader,
            ILoader<IActivityPlugin> activitiesLoader,
            INotificationHub hub)
            : base(config, sessionPublisherLoader, resultPublisherLoader, checksLoader, activitiesLoader, hub)
        {
            myIdentity = new PluginDescriptor
                             {
                                 Description = "Fast Startup Agent",
                                 Name = "FastStartAgent",
                                 TypeId = new Guid("F90773BA-C659-4964-B22D-A998719CB1FD")
                             };

            myStartingGate = new ManualResetEvent(false);
        }

        public override void Start()
        {      
            var startup = new Thread(AsyncStart);
            startup.Start();

            Logger.Debug("Startup proceeding aysnchronously...");
        }

        public override void Stop()
        {
            // cannot stop until async start has completed
            Logger.Debug("Waiting for any startup activity to complete so Agent can stop...");
            myStartingGate.WaitOne();
            base.Stop();
        }

        protected void AsyncStart()
        {
            myStartingGate.Reset();
            base.Start();
            myStartingGate.Set();

            Logger.Debug("Startup complete!");
        }
    }
}