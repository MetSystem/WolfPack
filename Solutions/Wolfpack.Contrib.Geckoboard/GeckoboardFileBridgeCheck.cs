//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Net;
//using Wolfpack.Core;
//using Wolfpack.Core.Checks;
//using Wolfpack.Core.Interfaces.Entities;

//namespace Wolfpack.Contrib.Geckoboard
//{
//    public class GeckoboardFileBridgeCheckConfig : PluginConfigBase
//    {
//        /// <summary>
//        /// The address of your Wolfpack Geckoboard Data Service eg: http://localhost/geckoboard
//        /// </summary>
//        public string BaseUri { get; set; }
//        /// <summary>
//        /// The location to save the files
//        /// </summary>
//        public string OutputFolder { get; set; }
//        /// <summary>
//        /// A list of Geckoboard Data Service Urls to call in the format, filename;uri
//        /// where the uri is the fragment from the BaseUri above eg: piechart/sommecheckid/any/sum
//        /// </summary>
//        public List<string> Reports { get; set;}
//    }

//    /// <summary>
//    /// </summary>
//    public class GeckoboardFileBridgeCheck : HealthCheckBase<GeckoboardFileBridgeCheckConfig>
//    {
//        public class GeckoboardFileReport
//        {
//            public string Filename { get; set; }
//            public string ResourceUri { get; set; }
//        }

//        protected List<GeckoboardFileReport> myReportsToRun;

//        /// <summary>
//        /// default ctor
//        /// </summary>
//        public GeckoboardFileBridgeCheck(GeckoboardFileBridgeCheckConfig config)
//            : base(config)
//        {
//            myReportsToRun = ParseReports(config.Reports);
//        }

//        private static List<GeckoboardFileReport> ParseReports(List<string> reports)
//        {
//            var reportsToRun = new List<GeckoboardFileReport>();

//            reports.ForEach(report =>
//                                {
//                                    var parts = report.Split(';');
//                                    reportsToRun.Add(new GeckoboardFileReport
//                                                         {
//                                                             Filename = parts[0],
//                                                             ResourceUri = parts[1]
//                                                         });
//                                });
//            return reportsToRun;
//        }

//        public override void Execute()
//        {
//            var restClient = new RestClient(_config.BaseUri);

//            Logger.Debug("Calling Geckoboard Data Service urls...");
//            myReportsToRun.ForEach(report =>
//                                       {
//                                           try
//                                           {
//                                               var request = new RestRequest(report.ResourceUri, Method.POST);
//                                               var response = restClient.Execute(request);

//                                               if (response.ResponseStatus != ResponseStatus.Completed)
//                                                   // handle this how?
//                                                   return;
//                                               if (response.StatusCode != HttpStatusCode.OK)
//                                                   // handle this how?
//                                                   return;
                                               
//                                               var outputFile = SmartLocation.GetLocation(Path.Combine(
//                                                   _config.OutputFolder,
//                                                   report.Filename));
                                               
//                                               Logger.Debug("\tWriting response from {0} to {1}", response.ResponseUri, outputFile);

//                                               using (var sw = new StreamWriter(outputFile))
//                                               {
//                                                   sw.Write((string) response.Content);
//                                               }
//                                           }
//                                           catch (WebException wex)
//                                           {
//                                               var extraInfo = string.Empty;

//                                               if (wex.Status == WebExceptionStatus.ProtocolError)
//                                               {
//                                                   extraInfo = string.Format(", Http state: {0}, '{1}'",
//                                                                             (int) ((HttpWebResponse) wex.Response)
//                                                                                       .StatusCode,
//                                                                             ((HttpWebResponse) wex.Response).
//                                                                                 StatusDescription);
//                                               }

//                                               var result = new HealthCheckData
//                                                                {
//                                                                    Identity = Identity,
//                                                                    Info =
//                                                                        string.Format(
//                                                                            "Url '{0}' failed with code '{1}'{2}",
//                                                                            report.ResourceUri, wex.Status, extraInfo),
//                                                                    Result = false
//                                                                };
//                                               Messenger.Publish(result);
//                                           }
//                                       });
//        }

//        protected override PluginDescriptor BuildIdentity()
//        {
//            return new PluginDescriptor
//                       {
//                           Description = "Calls a Geckoboard Data Service url and saves the data to file",
//                           TypeId = new Guid("678D0FBE-FF45-495A-BF9E-A4E34805C552"),
//                           Name = _config.FriendlyId
//                       };
//        }
//    }
//}
