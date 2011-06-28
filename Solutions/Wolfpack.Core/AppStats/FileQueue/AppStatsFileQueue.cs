namespace Wolfpack.Core.AppStats.FileQueue
{
    public static class AppStatsFileQueue
    {
        public static AppStatsConfigBuilder PublishWith(this AppStatsConfigBuilder builder, string folder)
        {
            builder.PublisherInjector = (config => config.Publisher = new AppStatsFileQueuePublisher(folder));
            return builder;
        }
    }
}