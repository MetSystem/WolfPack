using System;
using Wolfpack.Core.Configuration;
using Wolfpack.Core.Interfaces.Entities;
using MSUtil;
using Wolfpack.Core;
using Wolfpack.Core.Notification.Filters.Request;

namespace Wolfpack.Contrib.LogParser
{
    public class XMLLogParserCheckConfig : LogParserConfigBase
    {
        public static class Defaults
        {
            public const string FieldNames = "Compact";
            public const string fMode = "Auto";
            public const int NumberOfLeafNodes = -1;
        }

        public string RootXPath { get; set; }
        public string fMode { get; set; }
        public string TimestampFormat { get; set; }
        public int? NumberOfLeafNodes { get; set; }

        /// <summary>
        /// Compact | XPath 
        /// </summary>
        public string FieldNames { get; set; }
    }

    public class XMLLogParserConfigurationAdvertiser : HealthCheckDiscoveryBase<XMLLogParserCheckConfig>
    {
        protected override XMLLogParserCheckConfig GetConfiguration()
        {
            return new XMLLogParserCheckConfig
                       {

                           Enabled = true,
                           FieldNames = XMLLogParserCheckConfig.Defaults.FieldNames,
                           FriendlyId = "CHANGEME!",
                           GenerateArtifacts = false,
                           InterpretZeroRowsAsAFailure = false,
                           NotificationMode = StateChangeNotificationFilter.FilterName,
                           NumberOfLeafNodes = XMLLogParserCheckConfig.Defaults.NumberOfLeafNodes,
                           Query = LogParserConfigBase.DefaultQueryPropertyText,
                           RootXPath = string.Empty,
                           TimestampFormat = string.Empty,
                           fMode = XMLLogParserCheckConfig.Defaults.fMode
                       };
        }

        protected override void Configure(ConfigurationEntry entry)
        {
            entry.Name = "LogParser:Xml";
            entry.Description = "This logparser check can interrogate information stored in XML structured text files. " + 
                LogParserConfigBase.DefaultDescriptionText;
            entry.Tags.AddIfMissing("LogParser", "Xml");
        }
    }

    public class XMLLogParserCheck : LogParserCheckBase<XMLLogParserCheckConfig>
    {
        public XMLLogParserCheck(XMLLogParserCheckConfig config)
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
            var context = new COMXMLInputContextClass
                              {
                                  fNames = _config.FieldNames ?? XMLLogParserCheckConfig.Defaults.FieldNames,
                              };

            if (_config.NumberOfLeafNodes.HasValue)
                context.dtNodes = _config.NumberOfLeafNodes.Value;
            if (!string.IsNullOrEmpty(_config.TimestampFormat))
                context.iTsFormat = _config.TimestampFormat;
            if (!string.IsNullOrEmpty(_config.fMode))
                context.fMode = _config.fMode;
            if (!string.IsNullOrEmpty(_config.RootXPath))
                context.rootXPath = _config.RootXPath;

            return context;
        }

        protected override PluginDescriptor BuildIdentity()
        {
            return new PluginDescriptor
                       {
                           Description = string.Format("XML LogParser Check"),
                           Name = _config.FriendlyId,
                           TypeId = new Guid("2A14E4DE-972A-44a1-9754-04457E5AFCDD")
                       };
        }
    }
}