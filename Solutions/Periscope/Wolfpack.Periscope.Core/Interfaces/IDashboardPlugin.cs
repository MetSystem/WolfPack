using System;

namespace Wolfpack.Periscope.Core.Interfaces
{
    public interface IDashboardPlugin : IDisposable
    {
        void Initialise();
        void Start();
    }
}