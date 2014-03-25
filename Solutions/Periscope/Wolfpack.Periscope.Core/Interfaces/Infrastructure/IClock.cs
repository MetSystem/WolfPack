using System;

namespace Wolfpack.Periscope.Core.Interfaces.Infrastructure
{
    public interface IClock
    {
        void SetIntervalInMilliseconds(int interval);
        void Start(Action callback);
    }
}