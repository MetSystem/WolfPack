using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Wolfpack.Core.Configuration;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Notification;
using Wolfpack.Core.Notification.Filters.Request;

namespace Wolfpack.Core.Checks
{
    public class DiskSpaceCheckConfig : PluginConfigBase, ISupportNotificationMode, ISupportNotificationThreshold
    {
        public string MachineName { get; set; }
        public string Drive { get; set; }
        public string NotificationMode { get; set; }
        public double? NotificationThreshold { get; set; }
    }

    public class DiskSpaceConfigurationAdvertiser : HealthCheckDiscoveryBase<DiskSpaceCheckConfig>
    {
        protected override DiskSpaceCheckConfig GetConfiguration()
        {
            return new DiskSpaceCheckConfig
            {
                Enabled = true,
                FriendlyId = "CHANGEME!",
                NotificationMode = StateChangeNotificationFilter.FilterName,
                NotificationThreshold = 80,
                MachineName = "localhost",
                Drive = "C"
            };
        }

        protected override void Configure(ConfigurationEntry entry)
        {
            entry.Name = "DiskSpaceUtilisation";
            entry.Description = "This check monitors the free disk space. Typically this check is configured to start producing notifications once a threshold % is breached.";
            entry.Tags.AddIfMissing("Disk", "Threshold");
        }
    }


    public class DiskSpaceCheck : ThresholdCheckBase<DiskSpaceCheckConfig>
    {
        protected PerformanceCounter _counter;

        /// <summary>
        /// default ctor
        /// </summary>
        public DiskSpaceCheck(DiskSpaceCheckConfig config)
            : base(config)
        {
        }

        public override void  Initialise()
        {
            _counter = string.IsNullOrEmpty(_config.MachineName) 
                ? new PerformanceCounter("LogicalDisk", "% Free Space", _config.Drive)
                : new PerformanceCounter("LogicalDisk", "% Free Space", _config.Drive, _config.MachineName);
        }

        public override void Execute()
        {
            var sample = _counter.NextSample();
            Thread.Sleep(1000);
            var sample2 = _counter.NextSample();
            var value = 100 - Math.Round(CounterSampleCalculator.ComputeCounterValue(sample, sample2));

            Publish(NotificationRequestBuilder.For(_config.NotificationMode, HealthCheckData.For(Identity, 
                "Disk space used on drive '{0}' is {1}%", _config.Drive, value)
                .Succeeded()
                .ResultCountIs(value)
                .DisplayUnitIs("%")
                .AddTag(_config.Drive))
                .Build());
        }

        protected override PluginDescriptor BuildIdentity()
        {
            return new PluginDescriptor
            {
                Description = "Reports the used disk space as %",
                TypeId = new Guid("F99F603F-B010-450D-8860-248340003E58"),
                Name = _config.FriendlyId
            };
        }
    }
}