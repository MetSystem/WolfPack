using System;
using Wolfpack.Core.Configuration;
using Wolfpack.Core.Interfaces.Entities;
using MSUtil;
using Wolfpack.Core;
using Wolfpack.Core.Notification.Filters.Request;

namespace Wolfpack.Contrib.LogParser
{
    public class TextLineLogParserCheckConfig : LogParserConfigBase
    {
        public static class Defaults
        {
            public const int Codepage = 0;
            public const int Recurse = 0;
            public const bool SplitLongLines = false;
        }

        public int? CodePage { get; set; }
        public int? Recurse { get; set; }
        public bool? SplitLongLines { get; set; }
        public string CheckpointFile { get; set; }
    }

    public class TextLineLogParserConfigurationAdvertiser : HealthCheckDiscoveryBase<TextLineLogParserCheckConfig>
    {
        protected override TextLineLogParserCheckConfig GetConfiguration()
        {
            return new TextLineLogParserCheckConfig
                       {
                           CheckpointFile = string.Empty,
                           CodePage = TextLineLogParserCheckConfig.Defaults.Codepage,
                           Enabled = true,
                           FriendlyId = "CHANGEME!",
                           GenerateArtifacts = false,
                           InterpretZeroRowsAsAFailure = false,
                           NotificationMode = StateChangeNotificationFilter.FilterName,
                           Query = LogParserConfigBase.DefaultQueryPropertyText,
                           Recurse = TextLineLogParserCheckConfig.Defaults.Recurse,
                           SplitLongLines = TextLineLogParserCheckConfig.Defaults.SplitLongLines
                       };
        }

        protected override void Configure(ConfigurationEntry entry)
        {
            entry.Name = "LogParser:TextLine";
            entry.Description = "This logparser check allows you to search for information in textfiles" +
                                LogParserConfigBase.DefaultDescriptionText;
            entry.Tags.AddIfMissing("LogParser", "TextLine");
        }
    }

    public class TextLineLogParserCheck : LogParserCheckBase<TextLineLogParserCheckConfig>
    {
        public TextLineLogParserCheck(TextLineLogParserCheckConfig config)
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
            var context = new COMTextLineInputContextClass
                              {
                                  codepage = _config.CodePage.GetValueOrDefault(TextLineLogParserCheckConfig.Defaults.Codepage),
                                  recurse = _config.Recurse.GetValueOrDefault(TextLineLogParserCheckConfig.Defaults.Recurse),
                                  splitLongLines = _config.SplitLongLines.GetValueOrDefault(TextLineLogParserCheckConfig.Defaults.SplitLongLines)
                              };

            if (!string.IsNullOrEmpty(_config.CheckpointFile))
                context.iCheckpoint = _config.CheckpointFile;

            return context;
        }

        protected override PluginDescriptor BuildIdentity()
        {
            return new PluginDescriptor
                       {
                           Description = string.Format("TextLine LogParser Check"),
                           Name = _config.FriendlyId,
                           TypeId = new Guid("22C41BC0-DDCA-4d6b-830A-F47959F1A516")
                       };
        }
    }
}