using Wolfpack.Periscope.Core.Infrastructure;
using Wolfpack.Periscope.Core.Interfaces.Entities;

namespace Wolfpack.Periscope.Core.Interfaces.Events
{
    public class WidgetUpdateEvent : GenericTinyMessage<WidgetUpdate>
    {
        public WidgetUpdateEvent(object sender, WidgetUpdate update) 
            : base(sender, update)
        {
        }
    }
}