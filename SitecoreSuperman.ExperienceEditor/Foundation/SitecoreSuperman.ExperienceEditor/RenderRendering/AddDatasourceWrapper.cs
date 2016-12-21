using Sitecore;
using Sitecore.Mvc.ExperienceEditor.Presentation;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;
using Sitecore.Mvc.Presentation;

namespace SitecoreSuperman.ExperienceEditor.RenderRendering
{
    public class AddDatasourceWrapper : Sitecore.Mvc.ExperienceEditor.Pipelines.Response.RenderRendering.AddWrapper
    {
        public override void Process(RenderRenderingArgs args)
        {
            if (args.Rendered || Context.Site == null || !Context.PageMode.IsExperienceEditorEditing ||
                args.Rendering.RenderingType.Equals("Layout"))
                return;

            var marker = this.GetMarker(args.Rendering);

            if (marker == null)
            {
                base.Process(args);
                return;
            }

            var index = args.Disposables.FindIndex(x => x.GetType() == typeof (Wrapper));

            if (index < 0)
                index = 0;

            args.Disposables.Insert(index, new Wrapper(args.Writer, marker));
        }

        protected IMarker GetMarker(Rendering rendering )
        {
            // If the Rendering's Datasource Template field is empty, we assume that it does not need a Datasource to function.
            if (!RenderingExtensions.RequiresDatasource(rendering.RenderingItem))
                return null;

            // If the rendering has a valid Datasource, do not add the Wrapper otherwise add it.
            return Context.ContentDatabase.GetItem(rendering.DataSource) == null
                ? new DatasourceRenderingMarker(RenderingContext.Current, rendering.RenderingItem.DisplayName)
                : null;
        }
    }
}