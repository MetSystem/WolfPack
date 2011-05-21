namespace Wolfpack.Core.Database.MongoDb
{
    public class MongoDbConfiguration
    {
        public string ServerName { get; set; }
        public int? Port { get; set; }
        public string DatabaseName { get; set; }
        public bool Enabled { get; set; }
        public string FriendlyId { get; set; }
        public string CollectionName { get; set; }
        public bool? Overwrite { get; set; }

        public bool AutoIndexId { get; set; }
        public bool Capped { get; set; }
        public string Create { get; set; }
        public long? Max { get; set; }
        public int? Size { get; set; }
    }
}