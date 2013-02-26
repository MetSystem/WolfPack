using System;
using Wolfpack.Core.Configuration;
using Wolfpack.Core.Interfaces.Entities;
using MSUtil;
using Wolfpack.Core;
using Wolfpack.Core.Notification.Filters.Request;

namespace Wolfpack.Contrib.LogParser
{
    public class URLSCANLogParserCheckConfig : LogParserConfigBase
    {
        public string CheckpointFile { get; set; }
    }

    public class URLScanLogParserConfigurationAdvertiser : HealthCheckDiscoveryBase<URLSCANLogParserCheckConfig>
    {
        protected override URLSCANLogParserCheckConfig GetConfiguration()
        {
            return new URLSCANLogParserCheckConfig
                       {
                           CheckpointFile = string.Empty,
                           Enabled = true,
                           FriendlyId = "CHANGEME!",
                           GenerateArtifacts = false,
                           InterpretZeroRowsAsAFailure = false,
                           NotificationMode = StateChangeNotificationFilter.FilterName,
                           Query = LogParserConfigBase.DefaultQueryPropertyText
                       };
        }

        protected override void Configure(ConfigurationEntry entry)
        {
            entry.Name = "LogParser:UrlScan";
            entry.Description = "This logparser check will interrogate data files produced by the IIS URLScan filter. " + LogParserConfigBase.DefaultDescriptionText;
            entry.Tags.AddIfMissing("LogParser", "UrlScan", "IIS");
        }
    }

    public class URLSCANLogParserCheck : LogParserCheckBase<URLSCANLogParserCheckConfig>
    {
        public URLSCANLogParserCheck(URLSCANLogParserCheckConfig config)
            : base(config)
        {
        }

        /// <summary>
        /// Returns the correct Input Context class for this type of query. It will also sanity check
        /// all relevant config params and throw exceptions where there are violations (required
        /// setting missing, badly formed etc)
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown if a setting is missing</exception>
        /// <exception cref="FormatException">Thrown if a setting is badly formed</exception>
        protected override object GetInputContext()
        {
            var context = new COMURLScanLogInputContextClass();

            if (!string.IsNullOrEmpty(_config.CheckpointFile))
                context.iCheckpoint = _config.CheckpointFile;

            return context;
        }

        protected override PluginDescriptor BuildIdentity()
        {
            return new PluginDescriptor
                       {
                           Description = string.Format("URLSCAN LogParser Check"),
                           Name = _config.FriendlyId,
                           TypeId = new Guid("0E16424C-1BE2-4ae4-A239-9826AD399B8C")
                       };
        }
    }
}