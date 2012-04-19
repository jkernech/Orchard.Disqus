using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Disqus.Models;

namespace Orchard.Disqus.Handlers
{
    public class DisqusSettingsPartHandler : ContentHandler
    {
        public DisqusSettingsPartHandler(IRepository<DisqusSettingsRecord> repository)
        {
            Filters.Add(new ActivatingFilter<DisqusSettingsPart>("Site"));
            Filters.Add(StorageFilter.For(repository));
        }
    }
}