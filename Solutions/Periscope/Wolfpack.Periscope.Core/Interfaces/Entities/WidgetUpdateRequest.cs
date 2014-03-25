using System;

namespace Wolfpack.Periscope.Core.Interfaces.Entities
{
    public class WidgetUpdateRequest
    {
        public string Target { get; set; }
        public DateTime Timestamp { get; set; }
        public string Origin { get; set; }
    }
}