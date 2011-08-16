using System;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Checks
{
    /// <summary>
    /// Pings a host with an ICMP message
    /// </summary>
    public class HostPingCheck : IHealthCheckPluginEx
    {
        protected bool myEnabled;
        protected PluginDescriptor  myIdentity;

        /// <summary>
        /// default ctor
        /// </summary>
        public HostPingCheck(string friendlyName, bool enabled)
        {
            Enabled = enabled;
            myIdentity = new PluginDescriptor
            {
                Description = "Pings a host with an ICMP message and raises an alert if no response is returned",
                TypeId = new Guid("F29826F2-8930-4E76-8710-47F0C08A4461"),
                Name = friendlyName
            };
        }

        public Status Status { get; set; }

        public bool Enabled
        {
            get { return myEnabled; }
            set { myEnabled = value; }
        }

        public PluginDescriptor Identity
        {
            get { return myIdentity; }
        }
       
        public void Initialise()
        {
            Logger.Debug("Initialising HostPingCheck check for...");
            //myConfig.Urls.ForEach(url => Logger.Debug("\t{0}", url));
        }

        public void Execute()
        {
            Logger.Debug("HostPingCheck is pinging...");
            /*
            myConfig.Urls.ToList().ForEach(url =>
                                               {
                                                   using (var wc = new WebClient())
                                                   {
                                                       try
                                                       {
                                                           var publish = !myConfig.PublishOnlyIfFailure;
                                                           var outcome = true;
                                                           var msg = string.Format("Successfully pinged url '{0}'", url);

                                                           var timer = Stopwatch.StartNew();
                                                           wc.DownloadString(url);
                                                           timer.Stop();

                                                           // perform threshold check...
                                                           if (myConfig.FailIfResponseMillisecondsOver.HasValue &&
                                                               (myConfig.FailIfResponseMillisecondsOver.Value > 0) &&
                                                               (timer.ElapsedMilliseconds > myConfig.FailIfResponseMillisecondsOver.Value))
                                                           {
                                                               // ok so we set a threshold and it was breached so...
                                                               publish = true;
                                                               outcome = false;
                                                               msg = string.Format(
                                                                       "Url '{0}' responded too slowly in {1}ms",
                                                                       url, timer.ElapsedMilliseconds);
                                                           }

                                                           if (!publish)
                                                               return;

                                                           var result = new HealthCheckData
                                                                            {
                                                                                Identity = Identity,
                                                                                Info = msg,
                                                                                Result = outcome,
                                                                                ResultCount = timer.ElapsedMilliseconds
                                                                            };
                                                           Messenger.Publish(result);
                                                       }
                                                       catch (WebException wex)
                                                       {
                                                           var extraInfo = string.Empty;

                                                           if (wex.Status == WebExceptionStatus.ProtocolError)
                                                           {
                                                               extraInfo = string.Format(", Http state: {0}, '{1}'",
                                                                                         (int)((HttpWebResponse)wex.Response)
                                                                                             .StatusCode,
                                                                                         ((HttpWebResponse)wex.Response).
                                                                                             StatusDescription);
                                                           }

                                                           var result = new HealthCheckData
                                                           {
                                                               Identity = Identity,
                                                               Info = string.Format("Url '{0}' failed with code '{1}'{2}",
                                                                   url, wex.Status, extraInfo),
                                                               Result = false
                                                           };
                                                           Messenger.Publish(result);
                                                       }
                                                   }
                                               });
             */
        }
    }
}
