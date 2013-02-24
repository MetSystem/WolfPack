using System;
using Wolfpack.Core.Interfaces.Entities;
using MSUtil;

namespace Wolfpack.Contrib.LogParser
{
    public class IISW3CLogParserCheckConfig : LogParserConfigBase
    {
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
        /// The number of days back to go. This will create the MinModDate from today
        /// </summary>
        public int? Days { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? DoubleQuotes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? DirTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? ConsolidateLogs { get; set; }

        public string CheckpointFile { get; set; }
    }

    public class IISW3CLogParserCheck : LogParserCheckBase<IISW3CLogParserCheckConfig>
    {
        public IISW3CLogParserCheck(IISW3CLogParserCheckConfig config)
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
            var context = new COMIISW3CInputContextClass
                              {
                                  iCodepage = _config.Codepage.GetValueOrDefault(-2),
                                  recurse = _config.Recurse.GetValueOrDefault(0),
                                  consolidateLogs = _config.ConsolidateLogs.GetValueOrDefault(false),
                                  dirTime = _config.DirTime.GetValueOrDefault(false),
                                  dQuotes = _config.DoubleQuotes.GetValueOrDefault(false)
                              };

            if (_config.Days.HasValue)
            {
                var minDate = DateTime.Today.AddDays(Math.Abs(_config.Days.Value)*-1);
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
                           Description = string.Format("IISW3C LogParser Check"),
                           Name = _config.FriendlyId,
                           TypeId = new Guid("2C98A60B-88B3-4333-965C-08A41789B7A3")
                       };
        }
    }
}   