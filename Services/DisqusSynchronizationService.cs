using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Helpers;
using Orchard.ContentManagement;
using Orchard.Disqus.Models;
using Orchard.Logging;

namespace Orchard.Disqus.Services
{
    public class DisqusSynchronizationService : IDisqusSynchronizationService
    {
        private const string DisqusListPostsUrl = "http://disqus.com/api/3.0/forums/listPosts.json?forum={0}&order=asc&limit={1}&related=thread&api_secret={2}";

        private readonly IOrchardServices _orchardServices;
        private readonly IDisqusMappingService _commentMappingService;

        public DisqusSynchronizationService(
            IOrchardServices orchardServices,
            IDisqusMappingService commentMappingService)
        {
            _orchardServices = orchardServices;
            _commentMappingService = commentMappingService;

            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        public int ImportComments()
        {
            var settings = _orchardServices.WorkContext.CurrentSite.As<DisqusSettingsPart>();

            var lastSync = settings.SynchronizedAt;
            var postCount = 5;
            var totalAdded = 0;

            // loop through until disqus does not return 100 posts
            while (postCount >= 5)
            {
                var posts = GetPosts(settings.ShortName, settings.ApiKey, 5, lastSync);
                postCount = posts.Count();
                foreach (var post in posts)
                {
                    int contentId = -1;
                    var success = false;

                    var thread = post.Thread;
                    
                    foreach (string id in thread.Identifiers)
                    {
                        var parts = id.Split(' ');
                        if (parts.Length == 2 && int.TryParse(parts[0], out contentId))
                        {
                            success = _commentMappingService.MapThreadIdToContentId(thread.Id, contentId, parts[1]);
                            if (success)
                            {
                                break;
                            }
                        }
                    }

                    if (success)
                    {
                        if (_commentMappingService.CreateCommentFromPost(contentId, post))
                        {
                            totalAdded += 1;
                        }
                    }

                    lastSync = post.CreatedAt;
                }
            }

            settings.SynchronizedAt = lastSync;

            Logger.Information(string.Format("Added {0} comments from Disqus.", totalAdded));

            return totalAdded;
        }

        public List<DisqusPost> GetPosts(string shortname, string secretKey, int limit, DateTime? lastsync)
        {
            try
            {
                var url = string.Format(DisqusListPostsUrl, shortname, limit, secretKey);

                if (lastsync.HasValue)
                {
                    url = string.Concat(url, "&since=", lastsync.Value.ToString("s"));
                }

                var result = GetResponse(url);

                var postResponse = Json.Decode<DisqusPostResponse>(result);
                return postResponse.Response;
            }
            catch (WebException e)
            {
                var response = new StreamReader(e.Response.GetResponseStream());

                Logger.Error("Could not retrieve posts from Disqus.");
                Logger.Error(response.ReadToEnd());

                throw;
            }
        }

        private static string GetResponse(string url)
        {
            var request = WebRequest.Create(url);
            var response = request.GetResponse();
            var stream = new StreamReader(response.GetResponseStream());
            return stream.ReadToEnd();
        }
    }
}