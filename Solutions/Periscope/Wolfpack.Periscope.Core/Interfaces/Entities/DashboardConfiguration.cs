using System.Collections.Generic;

namespace Wolfpack.Periscope.Core.Interfaces.Entities
{
    public class DashboardConfiguration
    {
        public IEnumerable<DashboardPanelConfiguration> Panels { get; set; }    
    }
}