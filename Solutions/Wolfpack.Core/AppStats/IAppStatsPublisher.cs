
namespace Wolfpack.Core.AppStats
{
    public interface IAppStatsPublisher
    {
        void Publish(AppStatsEvent stat);
    }
}