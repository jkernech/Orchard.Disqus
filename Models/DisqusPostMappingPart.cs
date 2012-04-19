using Orchard.ContentManagement;

namespace Orchard.Disqus.Models
{
    public class DisqusPostMappingPart : ContentPart<DisqusPostMappingRecord>
    {
        public string PostId
        {
            get { return Record.PostId; }
            set { Record.PostId = value; }
        }
    }
}