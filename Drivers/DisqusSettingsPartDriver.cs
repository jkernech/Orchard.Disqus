using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Disqus.Models;
using Orchard.Localization;

namespace Orchard.Disqus.Drivers
{
    public class DisqusSettingsPartDriver : ContentPartDriver<DisqusSettingsPart>
    {
        public DisqusSettingsPartDriver()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        protected override string Prefix 
        { 
            get { return "CommentSettings"; } 
        }

        protected override DriverResult Editor(DisqusSettingsPart part, dynamic shapeHelper)
        {
            return Editor(part, null, shapeHelper);
        }

        protected override DriverResult Editor(DisqusSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var shape = ContentShape("Parts_Disqus_Settings", () =>
            {
                if (updater != null)
                {
                    updater.TryUpdateModel(part.Record, Prefix, null, null);
                }

                return shapeHelper.EditorTemplate(TemplateName: "Parts.Disqus.Settings", Model: part.Record, Prefix: Prefix);
            });
            
            return shape.OnGroup("comments");
        }
    }
}