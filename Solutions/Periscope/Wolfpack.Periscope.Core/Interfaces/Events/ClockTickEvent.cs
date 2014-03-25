using System;
using Wolfpack.Periscope.Core.Infrastructure;

namespace Wolfpack.Periscope.Core.Interfaces.Events
{
    public class ClockTickEvent : GenericTinyMessage<DateTime>
    {
        public ClockTickEvent(object sender) 
            : base(sender, DateTime.UtcNow)
        {
        }
    }
}