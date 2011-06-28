using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Bus
{
    public class BusConfig : PluginConfigBase
    {
        public string ErrorQueue { get; set; }
        public string InputQueue { get; set; }
    }

    public class BusPublisherConfig : BusConfig
    {
        public string HttpGatewayUri { get; set; }
        public string OutputQueue { get; set; }
    }
}