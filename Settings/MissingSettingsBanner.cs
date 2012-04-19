using System.Collections.Generic;
using System.Web.Mvc;
using Orchard.ContentManagement;
using Orchard.Disqus.Models;
using Orchard.Localization;
using Orchard.UI.Admin.Notification;
using Orchard.UI.Notify;

namespace Orchard.Disqus.Settings
{
    public class MissingSettingsBanner : INotificationProvider
    {
        private readonly IOrchardServices _orchardServices;
        private readonly WorkContext _workContext;

        public MissingSettingsBanner(IOrchardServices orchardServices, IWorkContextAccessor workContextAccessor)
        {
            _orchardServices = orchardServices;
            _workContext = workContextAccessor.GetContext();

            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public IEnumerable<NotifyEntry> GetNotifications()
        {
            var settings = _orchardServices.WorkContext.CurrentSite.As<DisqusSettingsPart>();

            if (settings == null || !settings.IsValid())
            {
                var urlHelper = new UrlHelper(_workContext.HttpContext.Request.RequestContext);
                var url = urlHelper.Action("Comments", "Admin", new { Area = "Settings" });
                yield return new NotifyEntry { Message = T("The <a href=\"{0}\">Disqus settings</a> needs to be configured.", url), Type = NotifyType.Warning };
            }
        }
    }
}
