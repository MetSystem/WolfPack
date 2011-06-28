using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Contrib.BuildAnalytics.Interfaces.Entities
{
    public abstract class BuildParserConfigBase : PluginConfigBase
    {
        /// <summary>
        /// Set to true to match a successful build, false for an unsuccessful build
        /// and leave blank/null if the build result state from the <see cref="TargetHealthCheckName"/>
        /// build result is not important 
        /// </summary>
        public bool? MatchBuildResult { get; set; }
        public string TargetHealthCheckName { get; set; }
        public string ZipFileTemplate { get; set; }
        public string ReportFileTemplate { get; set; }        
    }
}