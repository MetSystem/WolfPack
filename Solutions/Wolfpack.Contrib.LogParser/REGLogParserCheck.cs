using System;
using Wolfpack.Core.Configuration;
using Wolfpack.Core.Interfaces.Entities;
using MSUtil;
using Wolfpack.Core;
using Wolfpack.Core.Notification.Filters.Request;

namespace Wolfpack.Contrib.LogParser
{
    public class REGLogParserCheckConfig : LogParserConfigBase
    {
        public static class Defaults
        {
            public const string MultiSZSep = "|";
            public const string BinaryFormat = "ASC";
            public const int Recurse = -1;
        }

        public int? Recurse { get; set; }

        /// <summary>
        /// default = "|"
        /// </summary>
        public string MultiSZSep { get; set; }

        /// <summary>
        /// Valid values are "ASC", "HEX", "PRINT"
        /// </summary>
        public string BinaryFormat { get; set; }
    }

    public class REGLogParserConfigurationAdvertiser : HealthCheckDiscoveryBase<REGLogParserCheckConfig>
    {
        protected override REGLogParserCheckConfig GetConfiguration()
        {
            return new REGLogParserCheckConfig
                       {
                            BinaryFormat = REGLogParserCheckConfig.Defaults.BinaryFormat,
                            Enabled = true,
                            FriendlyId = "CHANGEME!",
                            GenerateArtifacts = false,
                            InterpretZeroRowsAsAFailure = false,
                            MultiSZSep = REGLogParserCheckConfig.Defaults.MultiSZSep,
                            NotificationMode = StateChangeNotificationFilter.FilterName,
                            Query = LogParserConfigBase.DefaultQueryPropertyText,
                            Recurse = REGLogParserCheckConfig.Defaults.Recurse
                       };
        }

        protected override void Configure(ConfigurationEntry entry)
        {
            entry.Name = "LogParser:Registry";
            entry.Description = "This logparser check allows you to locate information in the registry. " +
                                LogParserConfigBase.DefaultDescriptionText;
            entry.Tags.AddIfMissing("LogParser", "Registry");
        }
    }

    public class REGLogParserCheck : LogParserCheckBase<REGLogParserCheckConfig>
    {
        public REGLogParserCheck(REGLogParserCheckConfig config)
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
            var context = new COMRegistryInputContextClass
                              {
                                  recurse = _config.Recurse ?? -1,
                                  multiSZSep = _config.MultiSZSep ?? "|",
                                  binaryFormat = _config.BinaryFormat ?? "ASC"
                              };

            return context;
        }

        protected override PluginDescriptor BuildIdentity()
        {
            return new PluginDescriptor
            {
                Description = string.Format("Registry LogParser Check"),
                Name = _config.FriendlyId,
                TypeId = new Guid("6725AA64-376F-415c-8F36-E2DE1889EBC0")
            };
        }
    }
}