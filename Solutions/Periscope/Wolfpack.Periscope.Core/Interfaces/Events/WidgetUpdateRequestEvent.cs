using Wolfpack.Periscope.Core.Infrastructure;
using Wolfpack.Periscope.Core.Interfaces.Entities;

namespace Wolfpack.Periscope.Core.Interfaces.Events
{
    public class WidgetUpdateRequestEvent : GenericTinyMessage<WidgetUpdateRequest>
    {
        public WidgetUpdateRequestEvent(object sender, WidgetUpdateRequest args) 
            : base(sender, args)
        {
        }
    }
}