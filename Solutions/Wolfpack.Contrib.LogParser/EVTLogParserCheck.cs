using System;
using Wolfpack.Core.Interfaces.Entities;
using MSUtil;

namespace Wolfpack.Contrib.LogParser
{
    public class EVTLogParserCheckConfig : LogParserConfigBase
    {
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
                                  fullText = _config.FullText.GetValueOrDefault(true),
                                  resolveSIDs = _config.ResolveSIDs.GetValueOrDefault(false),
                                  formatMsg = _config.FormatMsg.GetValueOrDefault(true),
                                  fullEventCode = _config.FullEventCode.GetValueOrDefault(false),
                                  msgErrorMode = _config.MsgErrorMode ?? "MSG",
                                  direction = _config.Direction ?? "FW",
                                  stringsSep = _config.StringsSep ?? "|",
                                  binaryFormat = _config.BinaryFormat ?? "HEX"
                              };

            if (!string.IsNullOrEmpty(_config.CheckpointFile))
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