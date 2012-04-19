namespace Orchard.Disqus.Models
{
    public class DisqusThreadMappingRecord
    {
        public virtual int Id { get; set; }

        public virtual string ThreadId { get; set; }

        public virtual int ContentId { get; set; }
    }
}