using Wolfpack.Periscope.Core.Infrastructure;

namespace Wolfpack.Periscope.Core.Interfaces.Events
{
    public class PanelChangedEvent : GenericTinyMessage<IDashboardPanel>
    {
        public PanelChangedEvent(object sender, IDashboardPanel panel)
            : base(sender, panel)
        {
        }
    }
}