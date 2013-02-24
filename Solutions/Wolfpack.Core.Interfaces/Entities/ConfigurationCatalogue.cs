using System.Collections.Generic;

namespace Wolfpack.Core.Interfaces.Entities
{
    public class ConfigurationCatalogue
    {
        public IEnumerable<ConfigurationChangeRequest> Pending { get; set; }
        public IEnumerable<ConfigurationEntry> Items { get; set; }
    }
}