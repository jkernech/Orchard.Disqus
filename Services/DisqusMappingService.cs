using System.Linq;
using Orchard.Autoroute.Models;
using Orchard.Comments.Models;
using Orchard.Comments.Services;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Disqus.Models;

namespace Orchard.Disqus.Services
{
    public class DisqusMappingService : IDisqusMappingService
    {
        private readonly IOrchardServices _orchardServices;
        private readonly ICommentService _commentService;
        private readonly IRepository<DisqusThreadMappingRecord> _threadMappingRepository;
        private readonly IRepository<DisqusPostMappingRecord> _postMappingRepository;

        public DisqusMappingService(
            IOrchardServices orchardServices,
            ICommentService commentService,
            IRepository<DisqusThreadMappingRecord> threadMappingRepository,
            IRepository<DisqusPostMappingRecord> postMappingRepository)
        {
            _orchardServices = orchardServices;
            _commentService = commentService;
            _threadMappingRepository = threadMappingRepository;
            _postMappingRepository = postMappingRepository;
        }

        public bool MapThreadIdToContentId(string threadId, int contentId, string validSlug)
        {
            var success = false;
            var result = _threadMappingRepository.Fetch(t => t.ThreadId == threadId).ToList();

            if (result.Any())
            {
                var record = result.FirstOrDefault();
                if (record != null && record.ContentId == contentId)
                {
                    success = true;
                }
            }
            else
            {
                var contentItem = _orchardServices.ContentManager.Get(contentId);
                if (contentItem != null)
                {
                    string slug = null;
                    if (contentItem.Has(typeof(AutoroutePart)))
                    {
                        var route = contentItem.Get<AutoroutePart>();
                        slug = route.Path;
                    }

                    if (slug == validSlug)
                    {
                        var record = new DisqusThreadMappingRecord { ThreadId = threadId, ContentId = contentId };

                        _threadMappingRepository.Create(record);
                        _threadMappingRepository.Flush();

                        success = true;
                    }
                }
            }

            return success;
        }

        public int GetContentIdForThreadId(string threadId)
        {
            var result = _threadMappingRepository.Fetch(cm => cm.ThreadId == threadId).FirstOrDefault();

            return result == null ? -1 : result.ContentId;
        }

        public bool CreateCommentFromPost(int contentId, DisqusPost post)
        {
            var posts = _orchardServices.ContentManager
                .Query<DisqusPostMappingPart, DisqusPostMappingRecord>()
                .Where(p => p.PostId == post.Id);

            if (posts.Count() > 0)
            {
                return false;
            }

            var context = new CreateCommentContext
            {
                Author = post.Author.Name,
                CommentText = post.Message,
                CommentedOn = contentId,
                Email = post.Author.Email,
                SiteName = post.Author.Url,
            };

            var commentPart = _commentService.CreateComment(context, false);

            commentPart.Record.CommentDateUtc = post.CreatedAt;
            commentPart.Record.Status = CommentStatus.Approved;

            var record = new DisqusPostMappingRecord { PostId = post.Id, ContentItemRecord = commentPart.ContentItem.Record };

            _postMappingRepository.Create(record);

            return true;
        }
    }
}