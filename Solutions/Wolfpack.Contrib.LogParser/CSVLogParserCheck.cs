using System;
using Wolfpack.Core.Configuration;
using Wolfpack.Core.Interfaces.Entities;
using MSUtil;
using Wolfpack.Core;
using Wolfpack.Core.Notification.Filters.Request;

namespace Wolfpack.Contrib.LogParser
{
    public class CSVLogParserCheckConfig : LogParserConfigBase
    {
        public static class Defaults
        {
            public const int CodePage = 0;
            public const int SkipLines = 0;
            public const int Lines = 10;
            public const int Fields = -1;
            public const bool HeaderRow = true;
            public const bool FixedFields = true;
            public const string DoubleQuotes = "Auto";
        }

        public bool? HeaderRow { get; set; }
        public string HeaderFile { get; set; }
        public bool? FixedFields { get; set; }
        public int? Fields { get; set; }
        public int? Lines { get; set; }

        /// <summary>
        /// Valid values are "Auto" or "Ignore"
        /// </summary>
        public string DoubleQuotes { get; set; }

        public int? SkipLines { get; set; }
        public string Comment { get; set; }
        public int? CodePage { get; set; }
        public string TimestampFormat { get; set; }
        public string CheckpointFile { get; set; }
    }

    public class CSVLogParserConfigurationAdvertiser : HealthCheckDiscoveryBase<CSVLogParserCheckConfig>
    {
        protected override CSVLogParserCheckConfig GetConfiguration()
        {
            return new CSVLogParserCheckConfig
                       {
                           CheckpointFile = string.Empty,
                           CodePage = CSVLogParserCheckConfig.Defaults.CodePage,
                           Comment = string.Empty,
                           DoubleQuotes = CSVLogParserCheckConfig.Defaults.DoubleQuotes,
                           Enabled = true,
                           Fields = CSVLogParserCheckConfig.Defaults.Fields,
                           FixedFields = CSVLogParserCheckConfig.Defaults.FixedFields,
                           FriendlyId = "CHANGEME!",
                           GenerateArtifacts = false,
                           HeaderFile = string.Empty,
                           HeaderRow = CSVLogParserCheckConfig.Defaults.HeaderRow,
                           InterpretZeroRowsAsAFailure = false,
                           Lines = CSVLogParserCheckConfig.Defaults.Lines,
                           NotificationMode = StateChangeNotificationFilter.FilterName,
                           Query = LogParserConfigBase.DefaultQueryPropertyText,
                           SkipLines = CSVLogParserCheckConfig.Defaults.SkipLines,
                           TimestampFormat = string.Empty
                       };
        }

        protected override void Configure(ConfigurationEntry entry)
        {
            entry.Name = "LogParser:CSV";
            entry.Description = "This logparser check will search a CSV datasource for entries matching the criteria you set. " +
                LogParserConfigBase.DefaultDescriptionText;
            entry.Tags.AddIfMissing(LogParserConfigBase.LogParserTag, "CSV");
        }
    }

    public class CSVLogParserCheck : LogParserCheckBase<CSVLogParserCheckConfig>
    {
        public CSVLogParserCheck(CSVLogParserCheckConfig config)
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
            var context = new COMCSVInputContextClass
                              {
                                  headerRow = _config.HeaderRow.GetValueOrDefault(CSVLogParserCheckConfig.Defaults.HeaderRow),
                                  fixedFields = _config.FixedFields.GetValueOrDefault(CSVLogParserCheckConfig.Defaults.FixedFields),
                                  nFields = _config.Fields.GetValueOrDefault(CSVLogParserCheckConfig.Defaults.Fields),
                                  dtLines = _config.Lines.GetValueOrDefault(CSVLogParserCheckConfig.Defaults.Lines),
                                  iDQuotes = _config.DoubleQuotes ?? CSVLogParserCheckConfig.Defaults.DoubleQuotes,
                                  nSkipLines = _config.SkipLines.GetValueOrDefault(CSVLogParserCheckConfig.Defaults.SkipLines),
                                  codepage = _config.CodePage.GetValueOrDefault(CSVLogParserCheckConfig.Defaults.CodePage)
                              };

            if (!string.IsNullOrEmpty(_config.TimestampFormat))
                context.iTsFormat = _config.TimestampFormat;
            if (!string.IsNullOrEmpty(_config.HeaderFile))
                context.headerFile = _config.HeaderFile;
            if (!string.IsNullOrEmpty(_config.Comment))
                context.comment = _config.Comment;
            if (!string.IsNullOrEmpty(_config.CheckpointFile))
                context.iCheckpoint = _config.CheckpointFile;

            return context;
        }

        protected override PluginDescriptor BuildIdentity()
        {
            return new PluginDescriptor
            {
                Description = string.Format("CSV LogParser Check"),
                Name = _config.FriendlyId,
                TypeId = new Guid("591DFDD0-FC26-44de-AF84-42C2BB20421D")
            };
        }
    }
}