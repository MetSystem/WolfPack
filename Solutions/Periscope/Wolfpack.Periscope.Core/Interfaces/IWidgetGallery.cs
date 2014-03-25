using System.Linq;
using Wolfpack.Periscope.Core.Interfaces.Entities;

namespace Wolfpack.Periscope.Core.Interfaces
{
    public interface IWidgetGallery
    {
        IQueryable<WidgetDefinition> Get();
        IQueryable<WidgetDefinition> Get(PaginationRequest paging);
        IQueryable<WidgetDefinition> Search(SearchRequest request);
    }
}