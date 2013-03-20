using System.Diagnostics;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;
using System.Linq;

namespace Wolfpack.Core.Schedulers
{
    public class HealthCheckTwentyFourSevenSchedulerConfig : TwentyFourSevenTimerConfig
    {
        public int IntervalInSeconds { get; set; }

        public HealthCheckTwentyFourSevenSchedulerConfig()
        {
            IntervalInSeconds = 60;
        }
    }

    [DebuggerDisplay("{Identity.Name}")]
    public class HealthCheckTwentyFourSevenScheduler : HealthCheckIntervalScheduler
    {
        protected TwentyFourSevenTimer _timer;

        public HealthCheckTwentyFourSevenScheduler(IHealthCheckPlugin check,
            HealthCheckTwentyFourSevenSchedulerConfig config) 
            : base(check, new HealthCheckIntervalSchedulerConfig
                              {
                                  IntervalInSeconds = config.IntervalInSeconds
                              })
        {
            _identity = new PluginDescriptor
                             {
                                 TypeId = check.Identity.TypeId,
                                 Description = check.Identity.Description,
                                 Name = check.Identity.Name,
                                 ScheduleDescription = "24/7 Scheduler"
                             };

            _timer = new TwentyFourSevenTimer(config);
        }

        /// <summary>
        /// Executes the health check plugin
        /// </summary>
        protected override void Execute()
        {
            if (!_timer.Triggered().Any())
                return;

            base.Execute();
        }
    }
}