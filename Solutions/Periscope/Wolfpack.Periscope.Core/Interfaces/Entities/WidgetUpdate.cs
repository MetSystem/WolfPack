namespace Wolfpack.Periscope.Core.Interfaces.Entities
{
    public class WidgetUpdate : WidgetUpdateRequest
    {
        // json serialised widget data
        public string Payload { get; set; }
    }
}