using Orchard.ContentManagement.Records;

namespace Orchard.Disqus.Models
{
    public class DisqusPostMappingRecord : ContentPartRecord
    {
        public virtual string PostId { get; set; }
    }
}