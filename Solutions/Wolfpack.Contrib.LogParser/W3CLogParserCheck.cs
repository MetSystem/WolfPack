using System;
using Wolfpack.Core.Configuration;
using Wolfpack.Core.Interfaces.Entities;
using MSUtil;
using Wolfpack.Core;
using Wolfpack.Core.Notification.Filters.Request;

namespace Wolfpack.Contrib.LogParser
{
    public class W3CLogParserCheckConfig : LogParserConfigBase
    {
        public static class Defaults
        {
            public const int Lines = 10;
            public const int Codepage = 0;
            public const bool DoubleQuoted = false;
        }

        public int? CodePage { get; set; }
        public int? Lines { get; set; }
        public bool? DoubleQuoted { get; set; }
        public string Separator { get; set; }
    }

    public class W3CLogParserConfigurationAdvertiser : HealthCheckDiscoveryBase<W3CLogParserCheckConfig>
    {
        protected override W3CLogParserCheckConfig GetConfiguration()
        {
            return new W3CLogParserCheckConfig
                       {
                           CodePage = W3CLogParserCheckConfig.Defaults.Codepage,
                           DoubleQuoted = W3CLogParserCheckConfig.Defaults.DoubleQuoted,
                           Enabled = true,
                           FriendlyId = "CHANGEME!",
                           GenerateArtifacts = false,
                           InterpretZeroRowsAsAFailure = false,
                           Lines = W3CLogParserCheckConfig.Defaults.Lines,
                           NotificationMode = StateChangeNotificationFilter.FilterName,
                           Query = LogParserConfigBase.DefaultQueryPropertyText,
                           Separator = "a single character | space | tab | auto"
                       };
        }

        protected override void Configure(ConfigurationEntry entry)
        {
            entry.Name = "LogParser:W3C";
            entry.Description = "This logparser check can interrogate W3C extended log file format; this include Personal Firewall, " +
                "Microsoft Internet Security and Acceleration Server (ISA Server), Windows Media Services, Exchange Tracking and SMTP log file. " + 
                LogParserConfigBase.DefaultDescriptionText;
            entry.Tags.AddIfMissing("LogParser", "W3C");
        }
    }

    public class W3CLogParserCheck : LogParserCheckBase<W3CLogParserCheckConfig>
    {
        public W3CLogParserCheck(W3CLogParserCheckConfig config)
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
            var context = new COMW3CInputContextClass
                              {
                                  dtLines = _config.Lines.GetValueOrDefault(W3CLogParserCheckConfig.Defaults.Lines),
                                  codepage = _config.CodePage.GetValueOrDefault(W3CLogParserCheckConfig.Defaults.Codepage),
                                  doubleQuotedStrings = _config.DoubleQuoted.GetValueOrDefault(W3CLogParserCheckConfig.Defaults.DoubleQuoted)
                              };

            if (!string.IsNullOrEmpty(_config.Separator))
                context.separator = _config.Separator;

            return context;
        }

        protected override PluginDescriptor BuildIdentity()
        {
            return new PluginDescriptor
                       {
                           Description = string.Format("W3C LogParser Check"),
                           Name = _config.FriendlyId,
                           TypeId = new Guid("7A633E92-4211-4efe-B557-9A8258DD016C")
                       };
        }
    }
}