using Orchard.Autoroute.Models;
using Orchard.ContentManagement;
using Orchard.DisplayManagement.Descriptors;
using Orchard.DisplayManagement.Implementation;
using Orchard.Disqus.Models;

namespace Orchard.Disqus
{
    public class DisqusShapes : IShapeTableProvider
    {
        private readonly IOrchardServices _orchardServices;

        public DisqusShapes(IOrchardServices services)
        {
            _orchardServices = services;
        }

        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("Parts_Comments")
                .OnDisplaying(shapeDisplayingContext =>
                    AddShapeWrapper(shapeDisplayingContext, "Parts_Comments_Wrapper"));

            builder.Describe("Parts_Comments_Count")
                .OnDisplaying(shapeDisplayingContext =>
                    AddShapeWrapper(shapeDisplayingContext, "Parts_Comments_Count_Wrapper"));

            builder.Describe("Parts_Blogs_BlogPost_List")
                .OnDisplaying(shapeDisplayingContext =>
                    AddShapeWrapper(shapeDisplayingContext, "Parts_BlogPost_List_Wrapper", includeUniqueIndentifier: false));
        }

        private void AddShapeWrapper(ShapeDisplayingContext shapeDisplayingContext, string wrapperName, bool includeUniqueIndentifier = true)
        {
            var settings = _orchardServices.WorkContext.CurrentSite.As<DisqusSettingsPart>();
            shapeDisplayingContext.Shape.DisqusSettings = settings;
            shapeDisplayingContext.ShapeMetadata.Wrappers.Add(wrapperName);

            if (includeUniqueIndentifier)
            {
                shapeDisplayingContext.Shape.DisqusUniqueId = GetUniqueIdentifier(shapeDisplayingContext.Shape.ContentPart.ContentItem);
            }
        }

        private static string GetUniqueIdentifier(ContentItem item)
        {
            string slug = null;
            if (item.Has<AutoroutePart>())
            {
                var route = item.Get<AutoroutePart>();
                slug = route.Path;
            }

            return string.Format("{0} {1}", item.Id, slug);
        }
    }
}