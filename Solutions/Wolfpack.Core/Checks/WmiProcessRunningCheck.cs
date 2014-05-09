using System;
using System.Linq;
using System.Management;
using Wolfpack.Core.Configuration;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Notification;
using Wolfpack.Core.Notification.Filters.Request;

namespace Wolfpack.Core.Checks
{
    public class WmiProcessRunningCheckConfig : PluginConfigBase, ISupportNotificationMode
    {
        public string RemoteUser { get; set; }
        public string RemotePwd { get; set; } 
        public string RemoteMachineId { get; set; } 
        public string ProcessName { get; set; }
        public string NotificationMode { get; set; }
    }

    public class WmiProcessConfigurationAdvertiser : HealthCheckDiscoveryBase<WmiProcessRunningCheckConfig, WmiProcessRunningCheck>
    {
        protected override WmiProcessRunningCheckConfig GetConfiguration()
        {
            return new WmiProcessRunningCheckConfig
                       {
                           Enabled = true,
                           FriendlyId = "CHANGEME!",
                           NotificationMode = StateChangeNotificationFilter.FilterName,
                           ProcessName = "CHANGEME!",
                           RemoteMachineId = "localhost",
                           RemoteUser = string.Empty,
                           RemotePwd = string.Empty
                       };
        }

        protected override void Configure(ConfigurationEntry entry)
        {
            entry.Name = "WmiProcessCheck";
            entry.Description =
                "This check uses WMI to detect if a named process is running. The process can be local or on a remote machine.";
            entry.Tags.AddIfMissing("HealthCheck", "WMI");
        }
    }

    public class WmiProcessRunningCheck : IHealthCheckPlugin
    {
        protected readonly PluginDescriptor _identity;
        protected readonly string _wmiNamespace;
        protected readonly WmiProcessRunningCheckConfig _config;

        public WmiProcessRunningCheck(WmiProcessRunningCheckConfig config)
        {
            _config = config;
            _wmiNamespace = string.Format(@"\\{0}\root\cimv2", config.RemoteMachineId);
            _identity = new PluginDescriptor
            {
                Description = string.Format("Checks for the existance of process '{0}' on {1}", _config.ProcessName, _config.RemoteMachineId),
                TypeId = new Guid("46D4374C-C65D-442e-9B93-AF50BB8C045C"),
                Name = _config.FriendlyId
            };
        }

        public Status Status { get; set; }

        public PluginDescriptor Identity
        {
            get { return _identity; }
        }

        public void Execute()
        {
            ManagementScope wmiScope;

            Logger.Debug("Querying wmi namespace {0}...", _wmiNamespace);
            if (!string.IsNullOrEmpty(_config.RemoteUser) && !string.IsNullOrEmpty(_config.RemotePwd))
            {
                wmiScope = new ManagementScope(_wmiNamespace, new ConnectionOptions
                {
                    Username = _config.RemoteUser,
                    Password = _config.RemotePwd
                });
            }
            else
            {
                wmiScope = new ManagementScope(_wmiNamespace);
            }

            // set up the query and execute it
            var wmiQuery = new ObjectQuery("Select * from Win32_Process");
            var wmiSearcher = new ManagementObjectSearcher(wmiScope, wmiQuery);
            var wmiResults = wmiSearcher.Get();
            var processes = wmiResults.Cast<ManagementObject>();

            var matches = (from process in processes
                          where (string.Compare(process["Name"].ToString(), _config.ProcessName, 
                          StringComparison.InvariantCultureIgnoreCase) == 0)
                          select process).ToList();

            var data = HealthCheckData.For(Identity, "There are {0} instances of process '{1}' on {2}",
                                          matches.Count(),
                                          _config.ProcessName,
                                          _config.RemoteMachineId)
                .ResultIs(matches.Any())
                .ResultCountIs(matches.Count);

            Messenger.Publish(NotificationRequestBuilder.For(_config.NotificationMode, data).Build());
        }

        public void Initialise()
        {
            // do nothing here
        }
    }
}