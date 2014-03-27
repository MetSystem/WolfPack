using System.Collections.Generic;
using System.IO;
using Wolfpack.Periscope.Core.Interfaces.Entities;

namespace Wolfpack.Periscope.Core.Interfaces.Widgets
{
    public interface IWidgetPresenter
    {
        List<Property> Includes { get; set; }
        void RenderMarkup(TextWriter writer);
        void RenderScript(TextWriter writer);
    }
}