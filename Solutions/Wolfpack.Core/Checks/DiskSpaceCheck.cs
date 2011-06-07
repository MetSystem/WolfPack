using System;
using System.Diagnostics;
using System.Threading;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Checks
{
    public class DiskSpaceCheckConfig : StreamThresholdCheckConfigBase
    {
        public string MachineName { get; set; }
        public string Drive { get; set; }
    }

    public class DiskSpaceCheck : StreamThresholdCheckBase<DiskSpaceCheckConfig>
    {
        protected PerformanceCounter myCounter;

        /// <summary>
        /// default ctor
        /// </summary>
        public DiskSpaceCheck(DiskSpaceCheckConfig config)
        {
            myConfig = config;
        }

        public override void  Initialise()
        {
            myCounter = string.IsNullOrEmpty(myConfig.MachineName) 
                ? new PerformanceCounter("LogicalDisk", "% Free Space", myConfig.Drive)
                : new PerformanceCounter("LogicalDisk", "% Free Space", myConfig.Drive, myConfig.MachineName);
        }

        public override void Execute()
        {
            var sample = myCounter.NextSample();
            Thread.Sleep(1000);
            var sample2 = myCounter.NextSample();
            var value = Math.Round(CounterSampleCalculator.ComputeCounterValue(sample, sample2));

            Publish(new HealthCheckData
                        {
                            Identity = Identity,
                            Info = string.Format("Free disk space on drive '{0}' is {1}%", myConfig.Drive, value),
                            Result = true,
                            ResultCount = value
                        });

        }

        protected override PluginDescriptor BuildIdentity()
        {
            return new PluginDescriptor
            {
                Description = "Reports the free disk space as %",
                TypeId = new Guid("F99F603F-B010-450D-8860-248340003E58"),
                Name = myConfig.FriendlyId
            };
        }
    }
}