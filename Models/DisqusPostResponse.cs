using System.Collections.Generic;

namespace Orchard.Disqus.Models
{
    public class DisqusPostResponse
    {
        public int Code { get; set; }

        public List<DisqusPost> Response { get; set; }
    }
}