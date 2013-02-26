using System;
using Wolfpack.Core.Interfaces.Entities;
using MSUtil;

namespace Wolfpack.Contrib.LogParser
{
    public class IISLogParserCheckConfig : LogParserConfigBase
    {
        public static class Defaults
        {
            public const int Codepage = -2;
            public const int Recurse = 0;
            public const string Locale = "DEF";
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
        /// default is DEF
        /// </summary>
        public string Locale { get; set; }

        public string CheckpointFile { get; set; }
    }

    public class IISLogParserCheck : LogParserCheckBase<IISLogParserCheckConfig>
    {
        public IISLogParserCheck(IISLogParserCheckConfig config)
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
            var context = new COMIISIISInputContextClass
                              {
                                  iCodepage = _config.Codepage.GetValueOrDefault(IISLogParserCheckConfig.Defaults.Codepage),
                                  recurse = _config.Recurse.GetValueOrDefault(IISLogParserCheckConfig.Defaults.Recurse),
                                  locale = _config.Locale ?? IISLogParserCheckConfig.Defaults.Locale
                              };

            if (_config.Days > 0)
            {
                var minDate = DateTime.Today.AddDays(Math.Abs(_config.Days) * -1);
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
                           Description = string.Format("IIS LogParser Check"),
                           Name = _config.FriendlyId,
                           TypeId = new Guid("FA8E0B46-43E5-433b-B417-21216531F92F")
                       };
        }
    }
}