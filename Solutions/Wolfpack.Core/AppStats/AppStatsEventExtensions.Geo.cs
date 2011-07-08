namespace Wolfpack.Core.AppStats
{
    /// <summary>
    /// This demonstrates how you can add/extend the fluent helper methods of the 
    /// AppStatsEvent* classes. To provide your own extension methods create a new
    /// ext. method and define your own continuation interface(s) which should eventually
    /// return the original AppStatsEvent object back. Create a new class to take as a ctor 
    /// param the AppStatsEvent object passed to the ext. method - this should also explicitly
    /// implement your continuation interface(s)
    /// </summary>
    public static partial class AppStatsEventExtensions
    {            
        public static IAppStatsGeoContinuation<AppStatsEvent> Geo(this AppStatsEvent stat)
        {
            return stat;
        }

        public static IAppStatsGeoContinuation<AppStatsEngine.AppStatsEventTimer> Geo(this AppStatsEngine.AppStatsEventTimer stat)
        {
            return stat;
        }

        public interface IAppStatsGeoContinuation<T>
        {
            T Point(string latitude, string longitude);
        }
    }
}