using System;
using System.IO;
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Wolfpack.Contrib.BuildAnalytics.Interfaces.Entities;
using Wolfpack.Core;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Publishers;

namespace Wolfpack.Contrib.BuildAnalytics.Publishers
{
    public abstract class BuildResultPublisherBase<T> : FilteredResultPublisherBase<T>
        where T : BuildParserConfigBase
    {
        protected BuildResultPublisherBase(T config, string friendlyName)
            : base(config, friendlyName)
        {
        }

        protected BuildResultPublisherBase(T config, Func<HealthCheckResult, bool> filter) : base(config, filter)
        {
        }

        /// <summary>
        /// This ensures we only invoke the parser if the build is successful AND matches the target name
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override bool MatchOnFriendlyName(HealthCheckResult message)
        {
            return message.Check.Result.GetValueOrDefault(false) && base.MatchOnFriendlyName(message);
        }

        /// <summary>
        /// This is responsible for building the fully qualified filename to a build artifact
        /// </summary>
        /// <param name="buildResult"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        protected virtual string BuildArtifactFilename(HealthCheckResult buildResult, string template)
        {
            if (string.IsNullOrWhiteSpace(buildResult.Check.Tags))
                return template;

            var filename = template;
            var rx = new Regex("({buildid})", RegexOptions.IgnoreCase);
            filename = rx.Replace(filename, buildResult.Check.Tags);

            return filename;
        }

        /// <summary>
        /// Gets the content of the report file or if stored in a zip then extracts it from there
        /// </summary>
        /// <param name="buildResult"></param>
        /// <param name="defaultFilename"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        protected virtual bool GetContent(HealthCheckResult buildResult, string defaultFilename, out string content)
        {
            content = null;

            if (!string.IsNullOrWhiteSpace(Config.ZipFileTemplate))
            {
                var zipFilename = BuildArtifactFilename(buildResult, Config.ZipFileTemplate);
                if (!File.Exists(zipFilename))
                    return false;

                ZipFile zf = null;

                try
                {
                    var relativeFilename = string.IsNullOrWhiteSpace(Config.ReportFileTemplate) 
                        ? defaultFilename 
                        : BuildArtifactFilename(buildResult, Config.ReportFileTemplate);
                    // zip folders are / delimited
                    relativeFilename = relativeFilename.Replace(@"\", "/");
                    
                    using (var fs = File.OpenRead(zipFilename))
                    {
                        zf = new ZipFile(fs);

                        foreach (ZipEntry zipEntry in zf)
                        {
                            if (!zipEntry.IsFile)
                                continue; // Ignore directories
                            if (string.Compare(zipEntry.Name, relativeFilename, true) != 0)
                                continue;

                            var buffer = new byte[4096]; // 4K is optimum
                            var zipStream = zf.GetInputStream(zipEntry);
                            
                            using (var ms = new MemoryStream())
                            {
                                StreamUtils.Copy(zipStream, ms, buffer);
                                ms.Seek(0, SeekOrigin.Begin);

                                using (var msr = new StreamReader(ms))
                                    content = msr.ReadToEnd();
                            }
                            break;
                        }
                    }
                }
                finally
                {
                    if (zf != null)
                    {
                        zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                        zf.Close(); // Ensure we release resources
                    }
                }
            }
            else
            {
                var reportFilename = BuildArtifactFilename(buildResult, Config.ReportFileTemplate);
                if (!File.Exists(reportFilename))
                    return false;

                using (var sr = new StreamReader(reportFilename))
                {
                    content = sr.ReadToEnd();
                }
            }

            return !string.IsNullOrWhiteSpace(content);
        }

        protected double? ExtractStat(string reportText)
        {
            var rx = new Regex(@"[-+]?\d*\.?\d+");
            var statText = rx.Match(reportText).Value;

            double stat;
            if (double.TryParse(statText, out stat))
                return stat;
            return null;
        }

        protected void PublishStat(HealthCheckResult buildResult, string category, double? stat, string tag)
        {
            Messenger.Publish(new HealthCheckResult
            {
                Agent = buildResult.Agent,
                Check = new HealthCheckData
                {
                    Identity = new PluginDescriptor
                    {
                        Name =
                            string.Format("{0}-{1}",
                                          buildResult.Check.Identity.
                                              Name, category)
                    },
                    ResultCount = stat,
                    Tags = tag
                },

            });
        }
    }
}