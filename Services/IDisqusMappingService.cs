using Orchard.Disqus.Models;

namespace Orchard.Disqus.Services
{
    public interface IDisqusMappingService : IDependency
    {
        int GetContentIdForThreadId(string threadId);

        bool MapThreadIdToContentId(string threadId, int contentId, string validSlug);

        bool CreateCommentFromPost(int contentId, DisqusPost post);
    }
}