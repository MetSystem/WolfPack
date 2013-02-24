using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Notification;

namespace Wolfpack.Core.Checks
{
    /// <summary>
    /// Pings a set of hosts with an ICMP message
    /// </summary>
    public class HostPingCheck : HealthCheckBaseEx
    {
        protected int? _timeout;
        protected List<string> _hosts;

        /// <summary>
        /// default ctor
        /// </summary>
        public HostPingCheck(string friendlyId, 
            bool enabled,
            string notificationMode,
            int? timeout,
            List<string> hosts)
            : base(friendlyId, enabled, notificationMode,
            new Guid("F29826F2-8930-4E76-8710-47F0C08A4461"),
            "Pings a host with an ICMP message and raises an alert if no response is returned")
        {
            _timeout = timeout;
            _hosts = hosts;
        }
       
        public override void Initialise()
        {
            Logger.Debug("Initialising HostPingCheck check for...");
            _hosts.ForEach(host => Logger.Debug("\t{0}", host));
        }

        public override void Execute()
        {
            Logger.Debug("HostPingCheck is pinging...");

            _hosts.ForEach(host =>
                                {
                                    using (var pinger = new Ping())
                                    {
                                        var reply = _timeout.HasValue 
                                                              ? pinger.Send(host, _timeout.Value) 
                                                              : pinger.Send(host);

                                        var data = HealthCheckData.For(Identity, "Successfully pinged host '{0}'",
                                                                         host)
                                            .Succeeded()
                                            .ResultCountIs(reply.RoundtripTime)
                                            .AddTag(host)
                                            .AddTag(reply.Status.ToString());

                                        if (reply.Status != IPStatus.Success)
                                        {
                                            data.Info = string.Format("Failure ({0}) pinging host {1}", reply.Status,
                                                                        host);
                                            data.Failed();
                                        }

                                        Messenger.Publish(NotificationRequestBuilder.For(NotificationMode, data).Build());
                                    }
                                });
        }
    }
}
