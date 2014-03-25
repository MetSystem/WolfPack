using System.Collections.Generic;

namespace Wolfpack.Periscope.Core.Interfaces.Entities
{
    public class Page<T>
    {
        public PaginationRequest Request { get; set; }
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
    }
}