using System.Collections.Generic;

namespace Wolfpack.Periscope.Core.Interfaces.Entities
{
    public class SearchRequest
    {
        public IEnumerable<string> Terms { get; set; }
        public PaginationRequest Paging { get; set; }
    }
}