using System;
using Wolfpack.Core.Configuration;
using Wolfpack.Core.Interfaces.Entities;
using MSUtil;
using Wolfpack.Core.Notification.Filters.Request;
using Wolfpack.Core;

namespace Wolfpack.Contrib.LogParser
{
    public class FSLogParserCheckConfig : LogParserConfigBase
    {
        public static class Defaults
        {
            public const int Recurse = -1;
            public const bool PreserveLastAccTime = false;
            public const bool UseLocalTime = true;
        }

        public int? Recurse { get; set; }
        public bool? PreserveLastAccTime { get; set; }
        public bool? UseLocalTime { get; set; }
    }

    public class FSLogParserConfigurationAdvertiser : HealthCheckDiscoveryBase<FSLogParserCheckConfig>
    {
        protected override FSLogParserCheckConfig GetConfiguration()
        {
            return new FSLogParserCheckConfig
                       {
                           Enabled = true,
                           FriendlyId = "CHANGEME!",
                           GenerateArtifacts = false,
                           InterpretZeroRowsAsAFailure = false,
                           NotificationMode = StateChangeNotificationFilter.FilterName,
                           PreserveLastAccTime = FSLogParserCheckConfig.Defaults.PreserveLastAccTime,
                           Query = LogParserConfigBase.DefaultQueryPropertyText,
                           Recurse = FSLogParserCheckConfig.Defaults.Recurse,
                           UseLocalTime = FSLogParserCheckConfig.Defaults.UseLocalTime
                       };
        }

        protected override void Configure(ConfigurationEntry entry)
        {
            entry.Name = "LogParser:FileSystem";
            entry.Description = "This logparser check can query the filesystem. " + 
                LogParserConfigBase.DefaultDescriptionText;
            entry.Tags.AddIfMissing("LogParser", "FileSystem");
        }
    }

    public class FSLogParserCheck : LogParserCheckBase<FSLogParserCheckConfig>
    {
        public FSLogParserCheck(FSLogParserCheckConfig config)
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
            var context = new COMFileSystemInputContextClass
                              {
                                  recurse = _config.Recurse.GetValueOrDefault(FSLogParserCheckConfig.Defaults.Recurse),
                                  preserveLastAccTime = _config.PreserveLastAccTime.GetValueOrDefault(FSLogParserCheckConfig.Defaults.PreserveLastAccTime),
                                  useLocalTime = _config.UseLocalTime.GetValueOrDefault(FSLogParserCheckConfig.Defaults.UseLocalTime)
                              };

            return context;
        }

        protected override PluginDescriptor BuildIdentity()
        {
            return new PluginDescriptor
            {
                Description = string.Format("FileSystem LogParser Check"),
                Name = _config.FriendlyId,
                TypeId = new Guid("64717308-4E6D-4b54-896C-EA0C40229287")
            };
        }
    }
}