using System;
using Wolfpack.Periscope.Core.Interfaces.Entities;
using Wolfpack.Periscope.Core.Interfaces.Widgets;

namespace Wolfpack.Periscope.Core.Interfaces
{
    public interface IWidget<out T> : IWidgetPresenter
        where T : WidgetConfiguration
    {
        WidgetDefinition Definition { get; }
        T Configuration { get; }
        IWidgetBootstrapper Bootstrapper { get; set; }
        IWidget<T> Configure(Action<T> configuratron, Func<T, IWidgetBootstrapper> builder = null);
        WidgetInstance CreateInstance();
    }
}