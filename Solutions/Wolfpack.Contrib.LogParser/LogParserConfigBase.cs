using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Contrib.LogParser
{
    public class LogParserConfigBase : PluginConfigBase, 
        ISupportNotificationMode, 
        ISupportResultInversion, 
        ISupportArtifactGeneration
    {
        public string NotificationMode { get; set; }

        public string Query { get; set; }

        public bool InterpretZeroRowsAsAFailure { get; set; }

        public bool? GenerateArtifacts { get; set; }
    }

    public interface ISupportArtifactGeneration
    {
        bool? GenerateArtifacts { get; set; }
    }
}