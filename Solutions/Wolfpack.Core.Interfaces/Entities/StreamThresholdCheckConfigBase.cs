
namespace Wolfpack.Core.Interfaces.Entities
{
    public abstract class StreamThresholdCheckConfigBase : PluginConfigBase
    {
        public bool StreamData { get; set; }
        public double? Threshold { get; set; }
    }
}