using Wolfpack.Periscope.Core.Infrastructure;

namespace Wolfpack.Periscope.Core.Interfaces.Events
{
    public class PanelChangeRequestEvent : GenericTinyMessage<IDashboardPanel>
    {
        public PanelChangeRequestEvent(object sender, IDashboardPanel panel) 
            : base(sender, panel)
        {
        }
    }
}