using System;
using System.Diagnostics;
using System.Threading;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Checks
{
    public class CpuCheckConfig : StreamThresholdCheckConfigBase
    {
        public string MachineName { get; set; }
    }

    public class CpuCheck : StreamThresholdCheckBase<CpuCheckConfig>
    {
        protected PerformanceCounter myCounter;

        /// <summary>
        /// default ctor
        /// </summary>
        public CpuCheck(CpuCheckConfig config)
            : base(config)
        {
        }

        public override void  Initialise()
        {
            myCounter = string.IsNullOrEmpty(myConfig.MachineName) 
                ? new PerformanceCounter("Processor", "% Processor Time", "_Total") 
                : new PerformanceCounter("Processor", "% Processor Time", "_Total", myConfig.MachineName);
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
                            Info = string.Format("Cpu utilisation is {0}%", value),
                            Result = true,
                            ResultCount = value
                        });

        }

        protected override PluginDescriptor BuildIdentity()
        {
            return new PluginDescriptor
            {
                Description = "Reports the CPU load as a %",
                TypeId = new Guid("E6C9C3DB-E234-407B-9949-2BE94C185D72"),
                Name = myConfig.FriendlyId
            };
        }
    }
}