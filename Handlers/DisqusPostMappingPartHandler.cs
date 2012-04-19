using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Disqus.Models;

namespace Orchard.Disqus.Handlers
{
    public class DisqusPostMappingPartHandler : ContentHandler
    {
        public DisqusPostMappingPartHandler(IRepository<DisqusPostMappingRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}