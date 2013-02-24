using System;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Notification;

namespace Wolfpack.Core.Schedulers
{
    public class HealthCheckIntervalSchedulerConfig : IntervalSchedulerConfig
    {
        
    }

    public class HealthCheckIntervalScheduler : IntervalSchedulerBase, IHealthCheckSchedulerPlugin
    {
        protected IHealthCheckPlugin _healthCheck;
        protected PluginDescriptor _identity;

        public HealthCheckIntervalScheduler(IHealthCheckPlugin check,
            HealthCheckIntervalSchedulerConfig config) 
            : base(config)
        {
            _healthCheck = check;
            _identity = new PluginDescriptor
                             {
                                 TypeId = check.Identity.TypeId,
                                 Description = check.Identity.Description,
                                 Name = check.Identity.Name,
                                 ScheduleDescription = string.Format("Every {0} Minutes", myInterval.TotalMinutes)
                             };
        }

        public override PluginDescriptor Identity
        {
            get { return _identity; }
        }

        /// <summary>
        /// Initialises the HealthCheck plugin
        /// </summary>
        public override void Initialise()
        {
            _healthCheck.Initialise();
        }

        /// <summary>
        /// Executes the health check plugin
        /// </summary>
        protected override void Execute()
        {
            try
            {
                _healthCheck.Execute();
            }
            catch (Exception ex)
            {
                var incidentCorrelationId = Guid.NewGuid();
                var msg = string.Format("Wolfpack Component Failure. IncidentId:={0}; Name:={1}; Details:={2}",
                    incidentCorrelationId,
                    _healthCheck.Identity.Name,
                    ex);

                Logger.Error(msg);

                // Broadcast a failure message
                Messenger.Publish(NotificationRequestBuilder.AlwaysPublish(new HealthCheckData
                                          {
                                              CriticalFailure = true,
                                              CriticalFailureDetails = new CriticalFailureDetails
                                                                           {
                                                                               Id = incidentCorrelationId
                                                                           },
                                              GeneratedOnUtc = DateTime.UtcNow,
                                              Identity = _healthCheck.Identity
                                          }).Build());
            }            
        }
    }
}