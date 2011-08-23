using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Checks
{
    /// <summary>
    /// Pings a set of hosts with an ICMP message
    /// </summary>
    public class HostPingCheck : HealthCheckBaseEx
    {
        protected int? myTimeout;
        protected List<string> myHosts;
        protected bool myPublishOnlyIfFailure;

        /// <summary>
        /// default ctor
        /// </summary>
        public HostPingCheck(string friendlyName, 
            bool enabled,
            bool publishOnlyIfFailure,
            int? timeout,
            List<string> hosts)
            : base(friendlyName, enabled,
            "Pings a host with an ICMP message and raises an alert if no response is returned",
            new Guid("F29826F2-8930-4E76-8710-47F0C08A4461"))
        {
            myTimeout = timeout;
            myHosts = hosts;
            myPublishOnlyIfFailure = publishOnlyIfFailure;
        }
       
        public override void Initialise()
        {
            Logger.Debug("Initialising HostPingCheck check for...");
            myHosts.ForEach(host => Logger.Debug("\t{0}", host));
        }

        public override void Execute()
        {
            Logger.Debug("HostPingCheck is pinging...");

            myHosts.ForEach(host =>
                                {
                                    using (var pinger = new Ping())
                                    {
                                        var reply = myTimeout.HasValue 
                                                              ? pinger.Send(host, myTimeout.Value) 
                                                              : pinger.Send(host);

                                        var result = HealthCheckData.For(Identity, "Successfully pinged host '{0}'",
                                                                         host)
                                            .Succeeded()
                                            .ResultCountIs(reply.RoundtripTime)
                                            .AddTag(host)
                                            .AddTag(reply.Status.ToString());

                                        if (reply.Status == IPStatus.Success)
                                        {
                                            if (myPublishOnlyIfFailure)
                                                return;
                                        }
                                        else
                                        {
                                            result.Info = string.Format("Failure ({0}) pinging host {1}", reply.Status,
                                                                        host);
                                            result.Failed();
                                        }

                                        Messenger.Publish(result);
                                    }
                                });

        }
    }
}
