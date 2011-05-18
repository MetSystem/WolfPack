using Growl.Connector;

namespace Wolfpack.Core.Growl
{
    public interface IGrowlConnection
    {
        GrowlConnector Connection { get; }
        GrowlConfiguration Config { get; }
    }
}