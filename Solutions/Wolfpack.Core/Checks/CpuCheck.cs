using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Wolfpack.Core.Interfaces.Entities;
using Castle.Core;
using System.Linq;

namespace Wolfpack.Core.Checks
{
    public class CpuCheckConfig : StreamThresholdCheckConfigBase
    {
        public string MachineName { get; set; }
    }

    public class CpuCheck : StreamThresholdCheckBase<CpuCheckConfig>
    {
        private class ProcessCounterSample
        {
            public Process Process { get; set; }
            public PerformanceCounter Counter { get; set; }
            public CounterSample Sample1 { get; set; }
            public double Value { get; set; }
        }

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

        protected override void AdjustMessageForBreach(HealthCheckData message)
        {
            base.AdjustMessageForBreach(message);

            var counters = new List<ProcessCounterSample>();
            var ps = Process.GetProcesses();

            ps.ForEach(process =>
                           {
                               try
                               {
                                   var counter = (string.IsNullOrEmpty(myConfig.MachineName)
                                                      ? new PerformanceCounter("Process", "% Processor Time", process.ProcessName)
                                                      : new PerformanceCounter("Process", "% Processor Time", process.ProcessName, myConfig.MachineName));
                                   counter.ReadOnly = true;

                                   counters.Add(new ProcessCounterSample
                                                    {
                                                        Counter = counter,
                                                        Process = process,
                                                        Sample1 = counter.NextSample()
                                                    });
                               }
                               catch
                               {
                                   Logger.Debug("*** Process '{0}' has no Performance Counter ***", process.ProcessName);
                               }
                           });

            Logger.Info("Getting CPU% for {0} processes...", counters.Count);
            Thread.Sleep(1000);

            counters.ForEach(pcs =>
                                 {
                                     var sample2 = pcs.Counter.NextSample();
                                     pcs.Value = Math.Round(CounterSampleCalculator.ComputeCounterValue(pcs.Sample1, sample2));
                                 });

            counters.Where(pcs => pcs.Value > 0)
                .OrderByDescending(pcs => pcs.Value)
                .Take(5)
                .ForEach(pcs =>
                             {
                                 var name = string.Format("{0}[{1}]", pcs.Process.ProcessName, pcs.Process.Id);
                                 message.AddProperty(name, Convert.ToString(pcs.Value));
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