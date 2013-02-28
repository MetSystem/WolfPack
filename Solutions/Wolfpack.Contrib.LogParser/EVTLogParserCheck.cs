using System;
using Wolfpack.Core.Configuration;
using Wolfpack.Core.Interfaces.Entities;
using MSUtil;
using Wolfpack.Core;
using Wolfpack.Core.Notification.Filters.Request;

namespace Wolfpack.Contrib.LogParser
{
    public class EVTLogParserCheckConfig : LogParserConfigBase
    {
        public static class Defaults
        {
            public const bool FullText = true;
            public const bool ResolveSIDs = false;
            public const bool FormatMsg = true;
            public const bool FullEventCode = false;
            public const string MsgErrorMode = "MSG";
            public const string Direction = "FW";
            public const string StringsSep = "|";
            public const string BinaryFormat = "HEX";
        }

        public bool? FullText { get; set; }
        public bool? ResolveSIDs { get; set; }
        public bool? FormatMsg { get; set; }
        public bool? FullEventCode { get; set; }
        public string BinaryFormat { get; set; }
        public string CheckpointFile { get; set; }
        public string StringsSep { get; set; }
        public string MsgErrorMode { get; set; }
        public string Direction { get; set; }
    }

    public class EVTLogParserConfigurationAdvertiser : HealthCheckDiscoveryBase<EVTLogParserCheckConfig>
    {
        protected override EVTLogParserCheckConfig GetConfiguration()
            {
            return new EVTLogParserCheckConfig
                       {
                           Enabled = true,
                           BinaryFormat = EVTLogParserCheckConfig.Defaults.BinaryFormat,
                           CheckpointFile = string.Empty,
                           Direction = EVTLogParserCheckConfig.Defaults.Direction,
                           FormatMsg = EVTLogParserCheckConfig.Defaults.FormatMsg,
                           FriendlyId = "CHANGEME!",
                           FullEventCode = EVTLogParserCheckConfig.Defaults.FullEventCode,
                           FullText = EVTLogParserCheckConfig.Defaults.FullText,
                           GenerateArtifacts = false,
                           InterpretZeroRowsAsAFailure = false,
                           MsgErrorMode = EVTLogParserCheckConfig.Defaults.MsgErrorMode,
                           NotificationMode = StateChangeNotificationFilter.FilterName,
                           Query = "SELECT TOP 50 * FROM APPLICATION",
                           ResolveSIDs = EVTLogParserCheckConfig.Defaults.ResolveSIDs,
                           StringsSep = EVTLogParserCheckConfig.Defaults.StringsSep
                       };
        }

        protected override void Configure(ConfigurationEntry entry)
        {
            entry.Name = "LogParser:EventLog";
            entry.Description = "This logparser check will search the EventLog for entries matching the criteria you set. " +
                LogParserConfigBase.DefaultDescriptionText;
            entry.Tags.AddIfMissing(LogParserConfigBase.LogParserTag, "EventLog");
        }
    }

    public class EVTLogParserCheck : LogParserCheckBase<EVTLogParserCheckConfig>
    {
        public EVTLogParserCheck(EVTLogParserCheckConfig config)
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
            var context = new COMEventLogInputContextClass
                              {
                                  fullText = _config.FullText.GetValueOrDefault(EVTLogParserCheckConfig.Defaults.FullText),
                                  resolveSIDs = _config.ResolveSIDs.GetValueOrDefault(EVTLogParserCheckConfig.Defaults.ResolveSIDs),
                                  formatMsg = _config.FormatMsg.GetValueOrDefault(EVTLogParserCheckConfig.Defaults.FormatMsg),
                                  fullEventCode = _config.FullEventCode.GetValueOrDefault(EVTLogParserCheckConfig.Defaults.FullEventCode),
                                  msgErrorMode = string.IsNullOrWhiteSpace(_config.MsgErrorMode) ? EVTLogParserCheckConfig.Defaults.MsgErrorMode : _config.MsgErrorMode,
                                  direction = string.IsNullOrWhiteSpace(_config.Direction) ? EVTLogParserCheckConfig.Defaults.Direction : _config.Direction,
                                  stringsSep = string.IsNullOrWhiteSpace(_config.StringsSep) ? EVTLogParserCheckConfig.Defaults.StringsSep : _config.StringsSep,
                                  binaryFormat = string.IsNullOrWhiteSpace(_config.BinaryFormat) ? EVTLogParserCheckConfig.Defaults.BinaryFormat : _config.BinaryFormat
                              };

            if (!string.IsNullOrWhiteSpace(_config.CheckpointFile))
                context.iCheckpoint = _config.CheckpointFile;

            return context;
        }

        protected override PluginDescriptor BuildIdentity()
        {
            return new PluginDescriptor
                             {
                                 Description = string.Format("EventLog LogParser Check"),
                                 Name = _config.FriendlyId,
                                 TypeId = new Guid("451EE20A-2938-47cc-B972-050CABB1DBBE")
                             };
        }
    }
}