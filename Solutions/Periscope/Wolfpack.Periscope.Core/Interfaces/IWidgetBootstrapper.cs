using System;

namespace Wolfpack.Periscope.Core.Interfaces
{
    public interface IWidgetBootstrapper : IDisposable
    {
        void Execute();
    }
}