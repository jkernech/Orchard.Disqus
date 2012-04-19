using System;
using Orchard.ContentManagement.Records;

namespace Orchard.Disqus.Models
{
    public class DisqusSettingsRecord : ContentPartRecord
    {
        public virtual string ShortName { get; set; }

        public virtual string ApiKey { get; set; }

        public virtual DateTime? SynchronizedAt { get; set; }
    }
}