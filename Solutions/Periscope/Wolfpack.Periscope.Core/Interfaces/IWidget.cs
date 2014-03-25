using Wolfpack.Periscope.Core.Interfaces.Entities;
using Wolfpack.Periscope.Core.Interfaces.Widgets;

namespace Wolfpack.Periscope.Core.Interfaces
{
    public interface IWidget : IWidgetPresenter
    {
        WidgetDefinition Definition { get; }
        WidgetConfiguration Configuration { get; set; }
        IWidgetBootstrapper Bootstrapper { get; set; }
    }
}