namespace Wolfpack.Periscope.Core.Interfaces.Entities
{
    public class WidgetInstance
    {
        public string Markup { get; set; }
        public string Script { get; set; }
        public IWidgetDataPump Trigger { get; set; }
        public WidgetDefinition Definition { get; set; }
        public WidgetConfiguration Configuration { get; set; }
    }
}