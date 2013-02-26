using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Contrib.LogParser
{
    public class LogParserConfigBase : PluginConfigBase, 
        ISupportNotificationMode, 
        ISupportResultInversion, 
        ISupportArtifactGeneration
    {
        public const string DefaultQueryPropertyText = "CHANGEME! Enter the full logparser query here";

        public const string DefaultDescriptionText = "Notifications will be generated based on the number of results returned (this behaviour can be altered with the 'InterpretZeroRowsAsAFailure' switch. " +
                "You can use the 'GenerateArtifacts' switch to also capture the results; these are stored and can be accessed via the WebService Interface.";

        public const string LogParserTag = "LogParser";

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