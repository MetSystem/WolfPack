using System;
using System.Collections.Generic;

namespace Wolfpack.Periscope.Core.Interfaces
{
    public interface IDashboardPanel
    {
        Guid Id { get; set; }
        string Name { get; set; }
        int Sequence { get; set; }
        int DwellInSeconds { get; set; }
        IEnumerable<IWidget> Widgets { get; set; }
    }
}