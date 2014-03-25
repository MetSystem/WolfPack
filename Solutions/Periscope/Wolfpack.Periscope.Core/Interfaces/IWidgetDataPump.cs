using System;
using Wolfpack.Periscope.Core.Interfaces.Entities;

namespace Wolfpack.Periscope.Core.Interfaces
{
    public interface IWidgetDataPump : IDisposable
    {
        void Initialise(WidgetConfiguration target);
        void Start();        
    }
}