using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Core.Database.SqlServer
{
    public class SqlServerConfiguration : PluginConfigBase
    {
        public string ConnectionString { get; set; }
    }
}