using System.Diagnostics;
using Wolfpack.Periscope.Core.Interfaces.Infrastructure;

namespace Wolfpack.Periscope.Core.Infrastructure
{
    public class TraceLogger : ILogger
    {
        public void LogDebug(string format, params object[] args)
        {
            Trace.WriteLine(string.Format(format, args));
        }
    }
}