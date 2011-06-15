using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Contrib.BuildAnalytics.Interfaces.Entities
{
    public abstract class BuildParserConfigBase : PluginConfigBase
    {
        public string TargetHealthCheckName { get; set; }
        public string ZipFileTemplate { get; set; }
        public string ReportFileTemplate { get; set; }        
    }
}