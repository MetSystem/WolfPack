namespace Wolfpack.Periscope.Core.Interfaces.Infrastructure
{
    public interface ILogger
    {
        void LogDebug(string format, params object[] args);
    }
}