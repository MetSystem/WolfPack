using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;
using System.Linq;

namespace Wolfpack.Core.Checks
{
    public class UrlPingCheckConfig : PluginConfigBase
    {
        /// <summary>
        /// A list of Urls to ping
        /// </summary>
        public List<string> Urls { get; set;}
        /// <summary>
        /// An optional threshold to set - if you want to test the url
        /// returns in a timely manner then set this to the number of milliseconds
        /// you expect to get a return in. If the url returns slower than this
        /// a failed HealthCheck will be published.
        /// </summary>
        public int? FailIfResponseMillisecondsOver { get; set; }

        /// <summary>
        /// If you are only interested in track whether the url is available or not
        /// then set this to true (default). If you want to track the response time
        /// then set this to false and every ping result will be published; successful
        /// pings include the response time in the result "ResultCount" property.
        /// </summary>
        public bool PublishOnlyIfFailure { get; set; }

        /// <summary>
        /// Set this to insure that you get the correct version of the URL Data that you want.
        /// in some cases you want the Moble version or maybe the version for the ipad. Some sites
        /// and network firewalls/proxies will not allow a requet through if the User agent is not 
        /// a know type.
        /// <summary>
        public string UserAgentString { get; set; }


        /// <summary>
        /// default ctor
        /// </summary>
        public UrlPingCheckConfig()
        {
            PublishOnlyIfFailure = true;
            UserAgentString = "Wolfpack.Agent";
        }
    }

    /// <summary>
    /// This will make a GET call to a set of Urls specified in the <see cref="UrlPingCheckConfig"/>
    /// configuration. If the call fails it will publish a failed result. If you have set the optional
    /// "FailIfResponseMillisecondsOver" property in the configuration and the response takes longer
    /// than this value a failed result is also published. NOTE: Each url is treated separately and you
    /// will receive a result for each ONE that fails.
    /// </summary>
    public class UrlPingCheck : IHealthCheckPlugin
    {
        protected readonly UrlPingCheckConfig myConfig;
        protected PluginDescriptor  myIdentity;

        /// <summary>
        /// default ctor
        /// </summary>
        public UrlPingCheck(UrlPingCheckConfig config)
        {
            myConfig = config;
            myIdentity = new PluginDescriptor
            {
                Description = "Pings a url and reports Http code and response time",
                TypeId = new Guid("54246DEB-36F2-4e9b-9BFA-B75BF40A8B7A"),
                Name = myConfig.FriendlyId
            };
        }

        public Status Status { get; set; }

        public PluginDescriptor Identity
        {
            get { return myIdentity; }
        }
       
        public void Initialise()
        {
            Logger.Debug("Initialising UrlPingCheck check for...");
            myConfig.Urls.ForEach(url => Logger.Debug("\t{0}", url));
        }

        public void Execute()
        {
            Logger.Debug("UrlPingCheck is pinging urls...");
            myConfig.Urls.ToList().ForEach(url =>
                                               {
                                                   using (var wc = new WebClient())
                                                   {
                                                       try
                                                       {
                                                           wc.Headers.Add("User-Agent", myConfig.UserAgentString);
                                                           var publish = !myConfig.PublishOnlyIfFailure;
                                                           var outcome = true;
                                                           var msg = string.Format("Successfully pinged url '{0}'", url);
                                                           Logger.Debug("Starting request Timer for {0}",url);
                                                           var timer = Stopwatch.StartNew();
                                                           wc.DownloadString(url);
                                                           timer.Stop();
                                                           Logger.Debug("Stopped request Timer({1}) for {0}", url,timer.ElapsedMilliseconds);
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
        }
    }
}
