using System;
using Orchard.Commands;
using Orchard.Disqus.Services;
using Orchard.Mvc.Html;

namespace Orchard.Disqus.Commands
{
    public class DisqusCommands : DefaultOrchardCommandHandler
    {
        private readonly IDisqusSynchronizationService _disqusSynchronizationService;

        public DisqusCommands(IDisqusSynchronizationService disqusSynchronizationService)
        {
            _disqusSynchronizationService = disqusSynchronizationService;
        }

        [CommandName("disqus comments import")]
        [CommandHelp("disqus comments import\r\n\t" + "Import comments from Disqus")]
        public void Import()
        {
            try
            {
                var totalAdded = _disqusSynchronizationService.ImportComments();
                Context.Output.WriteLine(T.Plural("1 Disqus comment imported.", "{0} Disqus comments imported.", totalAdded));
            }
            catch (Exception)
            {
                Context.Output.WriteLine(T("An error occured while importing comments from Disqus."));
            }
        }
    }
}