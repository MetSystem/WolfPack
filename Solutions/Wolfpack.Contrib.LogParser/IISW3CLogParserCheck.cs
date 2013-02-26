using System;
using Wolfpack.Core;
using Wolfpack.Core.Configuration;
using Wolfpack.Core.Interfaces.Entities;
using MSUtil;
using Wolfpack.Core.Notification.Filters.Request;

namespace Wolfpack.Contrib.LogParser
{
    public class IISW3CLogParserCheckConfig : LogParserConfigBase
    {
        public static class Defaults
        {
            public const int Codepage = -2;
            public const int Recurse = 0;
            public const bool ConsolidateLogs = false;
            public const bool DoubleQuotes = false;
            public const bool DirTime = false;
        }

        /// <summary>
        /// 0 is the system codepage; -2 specifies that the codepage is automatically 
        /// determined by inspecting the filename and/or the site's "LogInUTF8" property. 
        /// </summary>
        public int? Codepage { get; set; }

        /// <summary>
        /// 0 disables subdirectory recursion
        /// -1 enables unlimited recursion
        /// other wise is the level to recurse
        /// </summary>
        public int? Recurse { get; set; }

        /// <summary>
        /// The number of days back to go. Zero means do not set.
        /// This will create the MinModDate from today minus the number of days set
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? DoubleQuotes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? DirTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? ConsolidateLogs { get; set; }

        public string CheckpointFile { get; set; }
    }

    public class IISW3CLogParserConfigurationAdvertiser : HealthCheckDiscoveryBase<IISW3CLogParserCheckConfig>
    {
        protected override IISW3CLogParserCheckConfig GetConfiguration()
        {
            return new IISW3CLogParserCheckConfig
                       {
                           CheckpointFile = string.Empty,
                           Codepage = IISW3CLogParserCheckConfig.Defaults.Codepage,
                           ConsolidateLogs = IISW3CLogParserCheckConfig.Defaults.ConsolidateLogs,
                           Days = 0,
                           DirTime = IISW3CLogParserCheckConfig.Defaults.DirTime,
                           DoubleQuotes = IISW3CLogParserCheckConfig.Defaults.DoubleQuotes,
                           Enabled = true,
                           FriendlyId = "CHANGEME!",
                           GenerateArtifacts = false,
                           InterpretZeroRowsAsAFailure = false,
                           NotificationMode = StateChangeNotificationFilter.FilterName,
                           Query = LogParserConfigBase.DefaultQueryPropertyText,
                           Recurse = IISW3CLogParserCheckConfig.Defaults.Recurse
                       };
        }

        protected override void Configure(ConfigurationEntry entry)
        {
            entry.Name = "LogParser:IISW3CLog";
            entry.Description = "This logparser check will search the IIS W3C log for entries matching the criteria you set. " +
                LogParserConfigBase.DefaultDescriptionText;
            entry.Tags.AddIfMissing(LogParserConfigBase.LogParserTag, "IIS");
        }
    }

    public class IISW3CLogParserCheck : LogParserCheckBase<IISW3CLogParserCheckConfig>
    {
        public IISW3CLogParserCheck(IISW3CLogParserCheckConfig config)
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
            var context = new COMIISW3CInputContextClass
                              {
                                  iCodepage = _config.Codepage.GetValueOrDefault(IISW3CLogParserCheckConfig.Defaults.Codepage),
                                  recurse = _config.Recurse.GetValueOrDefault(IISW3CLogParserCheckConfig.Defaults.Recurse),
                                  consolidateLogs = _config.ConsolidateLogs.GetValueOrDefault(IISW3CLogParserCheckConfig.Defaults.ConsolidateLogs),
                                  dirTime = _config.DirTime.GetValueOrDefault(IISW3CLogParserCheckConfig.Defaults.DirTime),
                                  dQuotes = _config.DoubleQuotes.GetValueOrDefault(IISW3CLogParserCheckConfig.Defaults.DoubleQuotes)
                              };

            if (_config.Days > 0)
            {
                var minDate = DateTime.Today.AddDays(Math.Abs(_config.Days)*-1);
                context.minDateMod = minDate.ToString("yyyy-MM-dd hh:mm:ss");
            }
            if (!string.IsNullOrEmpty(_config.CheckpointFile))
                context.iCheckpoint = _config.CheckpointFile;

            return context;
        }

        protected override PluginDescriptor BuildIdentity()
        {
            return new PluginDescriptor
                       {
                           Description = string.Format("IISW3C LogParser Check"),
                           Name = _config.FriendlyId,
                           TypeId = new Guid("2C98A60B-88B3-4333-965C-08A41789B7A3")
                       };
        }
    }
}   