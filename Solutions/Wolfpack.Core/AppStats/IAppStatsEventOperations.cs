using System;

namespace Wolfpack.Core.AppStats
{
    public interface IAppStatsEventOperations<T>
    {
        T One();
        T Count(double count);
        T Time(TimeSpan duration);
    }
}