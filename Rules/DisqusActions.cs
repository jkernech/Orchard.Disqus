using Orchard.Disqus.Services;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Rules.Models;
using Orchard.Rules.Services;

namespace Orchard.Disqus.Rules
{
    [OrchardFeature("Orchard.Disqus.Rules")]
    public class DisqusActions : IActionProvider
    {
        private readonly IDisqusSynchronizationService _disqusSynchronizationService;

        public DisqusActions(IDisqusSynchronizationService disqusSynchronizationService)
        {
            _disqusSynchronizationService = disqusSynchronizationService;

            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void Describe(DescribeActionContext describe)
        {

            describe.For("Comments")
                .Element("DisqusImportComments", T("Import Disqus Comments"), T("Import comments from Disqus."),
                         ImportCommentsFromDisqus, context => T("Import comments from Disqus"));
        }

        public bool ImportCommentsFromDisqus(ActionContext context)
        {
            try
            {
                _disqusSynchronizationService.ImportComments();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}