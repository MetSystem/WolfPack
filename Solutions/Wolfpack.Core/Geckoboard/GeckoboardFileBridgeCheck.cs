using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Wolfpack.Core.Checks;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Geckoboard
{
    public class GeckoboardFileBridgeCheckConfig : PluginConfigBase
    {
        public string OutputFolder { get; set; }
        /// <summary>
        /// A list of Geckoboard Data Service Urls to call
        /// </summary>
        public List<string> Reports { get; set;}
    }

    /// <summary>
    /// </summary>
    public class GeckoboardFileBridgeCheck : HealthCheckBase<GeckoboardFileBridgeCheckConfig>
    {
        public class GeckoboardFileReport
        {
            public string Filename { get; set; }
            public string Url { get; set; }
        }

        protected List<GeckoboardFileReport> myReportsToRun;

        /// <summary>
        /// default ctor
        /// </summary>
        public GeckoboardFileBridgeCheck(GeckoboardFileBridgeCheckConfig config)
            : base(config)
        {
            myReportsToRun = ParseReports(config.Reports);
        }

        private static List<GeckoboardFileReport> ParseReports(List<string> reports)
        {
            var reportsToRun = new List<GeckoboardFileReport>();

            reports.ForEach(report =>
                                {
                                    var parts = report.Split(';');
                                    reportsToRun.Add(new GeckoboardFileReport
                                                         {
                                                             Filename = parts[0],
                                                             Url = parts[1]
                                                         });
                                });
            return reportsToRun;
        }

        public override void Execute()
        {
            Logger.Debug("Calling Geckoboard Data Service urls...");
            myReportsToRun.ForEach(report =>
                                               {
                                                   using (var wc = new WebClient())
                                                   {
                                                       try
                                                       {
                                                           var content = wc.DownloadString(report.Url);

                                                           var outputFile = SmartLocation.GetLocation(Path.Combine(
                                                               myConfig.OutputFolder,
                                                               report.Filename));
                                                           using (var sw = new StreamWriter(outputFile))
                                                           {
                                                               sw.Write(content);
                                                           }
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
                                                                   report.Url, wex.Status, extraInfo),
                                                               Result = false
                                                           };
                                                           Messenger.Publish(result);
                                                       }
                                                   }
                                               });
        }

        protected override PluginDescriptor BuildIdentity()
        {
            return new PluginDescriptor
                       {
                           Description = "Calls a Geckoboard Data Service url and saves the data to file",
                           TypeId = new Guid("54246DEB-36F2-4e9b-9BFA-B75BF40A8B7A"),
                           Name = myConfig.FriendlyId
                       };
        }
    }
}
