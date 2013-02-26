using System;
using Wolfpack.Core.Configuration;
using Wolfpack.Core.Interfaces.Entities;
using MSUtil;
using Wolfpack.Core;
using Wolfpack.Core.Notification.Filters.Request;

namespace Wolfpack.Contrib.LogParser
{
    public class TSVLogParserCheckConfig : LogParserConfigBase
    {
        public static class Defaults
        {
            public const int NumberOfSeparators = 1;
            public const bool FixedSeparator = false;
            public const bool HeaderRow = false;
            public const int Lines = 100;
            public const int SkipLines = 0;
            public const int Fields = -1;
            public const int Codepage = 0;
        }

        /// <summary>
        /// Valid values are a single character | spaces | space | tab (backslash t)
        /// </summary>
        public string Separator { get; set; }

        public int? NumberOfSeparators { get; set; }
        public bool? FixedSeparator { get; set; }
        public bool? HeaderRow { get; set; }
        public string HeaderFile { get; set; }
        public int? Fields { get; set; }
        public int? Lines { get; set; }
        public int? SkipLines { get; set; }
        public string LineFilter { get; set; }
        public int? CodePage { get; set; }
        public string TimestampFormat { get; set; }
        public string CheckpointFile { get; set; }
    }

    public class TSVLogParserConfigurationAdvertiser : HealthCheckDiscoveryBase<TSVLogParserCheckConfig>
    {
        protected override TSVLogParserCheckConfig GetConfiguration()
        {
            return new TSVLogParserCheckConfig
                       {
                           CheckpointFile = string.Empty,
                           CodePage = TSVLogParserCheckConfig.Defaults.Codepage,
                           Enabled = true,
                           Fields = TSVLogParserCheckConfig.Defaults.Fields,
                           FixedSeparator = TSVLogParserCheckConfig.Defaults.FixedSeparator,
                           FriendlyId = "CHANGEME!",
                           GenerateArtifacts = false,
                           HeaderFile = "Path to file containing meaningful headers or blank",
                           HeaderRow = TSVLogParserCheckConfig.Defaults.HeaderRow,
                           InterpretZeroRowsAsAFailure = false,
                           LineFilter = "Blank or +match or -notmatch",
                           Lines = TSVLogParserCheckConfig.Defaults.Lines,
                           NotificationMode = StateChangeNotificationFilter.FilterName,
                           NumberOfSeparators = TSVLogParserCheckConfig.Defaults.NumberOfSeparators,
                           Query = LogParserConfigBase.DefaultQueryPropertyText,
                           Separator = "Valid values are a single character | spaces | space | tab (backslash t)",
                           SkipLines = TSVLogParserCheckConfig.Defaults.SkipLines,
                           TimestampFormat = string.Empty
                       };
        }

        protected override void Configure(ConfigurationEntry entry)
        {
            entry.Name = "LogParser:TSV";
            entry.Description = "This logparser check can interrogate TSV (Tabular, tab or space) separated text files. " +
                                LogParserConfigBase.DefaultDescriptionText;
            entry.Tags.AddIfMissing("LogParser", "TSV");
        }
    }

    public class TSVLogParserCheck : LogParserCheckBase<TSVLogParserCheckConfig>
    {
        public TSVLogParserCheck(TSVLogParserCheckConfig config)
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
            var context = new COMTSVInputContextClass
                              {
                                  nSeparators = _config.NumberOfSeparators.GetValueOrDefault(TSVLogParserCheckConfig.Defaults.NumberOfSeparators),
                                  fixedSep = _config.FixedSeparator.GetValueOrDefault(TSVLogParserCheckConfig.Defaults.FixedSeparator),
                                  headerRow = _config.HeaderRow.GetValueOrDefault(TSVLogParserCheckConfig.Defaults.HeaderRow),
                                  dtLines = _config.Lines.GetValueOrDefault(TSVLogParserCheckConfig.Defaults.Lines),
                                  nSkipLines = _config.SkipLines.GetValueOrDefault(TSVLogParserCheckConfig.Defaults.SkipLines),
                                  codepage = _config.CodePage.GetValueOrDefault(TSVLogParserCheckConfig.Defaults.Codepage)
                              };

            if (_config.Fields.HasValue)
                context.nFields = _config.Fields.Value;
            if (!string.IsNullOrEmpty(_config.Separator))
                context.iSeparator = _config.Separator;
            if (!string.IsNullOrEmpty(_config.TimestampFormat))
                context.iTsFormat = _config.TimestampFormat;
            if (!string.IsNullOrEmpty(_config.HeaderFile))
                context.headerFile = _config.HeaderFile;
            if (!string.IsNullOrEmpty(_config.CheckpointFile))
                context.iCheckpoint = _config.CheckpointFile;
            if (!string.IsNullOrEmpty(_config.LineFilter))
                context.lineFilter = _config.LineFilter;

            return context;
        }

        protected override PluginDescriptor BuildIdentity()
        {
            return new PluginDescriptor
                       {
                           Description = string.Format("TSV LogParser Check"),
                           Name = _config.FriendlyId,
                           TypeId = new Guid("A82F0463-387D-4e80-AD62-A5FDBDD78DD1")
                       };
        }
    }
}