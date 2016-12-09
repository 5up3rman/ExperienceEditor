using Sitecore.Diagnostics;
using Sitecore.Mvc.Pipelines.Response.GetRenderer;
using Sitecore.Mvc.Presentation;

namespace SitecoreSuperman.ExperienceEditor.GetRenderer
{
    public class GetControllerRenderer : Sitecore.Mvc.Pipelines.Response.GetRenderer.GetControllerRenderer
    {
        public override void Process(GetRendererArgs args)
        {
            var rendering = args.Rendering;
            var database = Sitecore.Context.Database;

            if (RenderingExtensions.RequiresDatasource(rendering.RenderingItem) &&
                !RenderingExtensions.DatasourceExists(database, rendering.DataSource) &&
                !rendering.RenderingType.Equals("Layout"))
            {
                // Return Blank View if the Rendering Requires a Datasource and the Datasource doesn't exist.
                args.Result = this.GetRenderer(rendering, args);

                // Log for funzies
                Log.Warn($"EXPERIENCE EDITOR - Datasource Error: There is an issue with the datasource on the rendering: {rendering.RenderingItem.DisplayName}, Path: {rendering.Item.Paths.ContentPath}.", this);
                return;
            }

            base.Process(args);
        }

        protected override Renderer GetRenderer(Rendering rendering, GetRendererArgs args)
        {
            return new ViewRenderer
            {
                ViewPath = ExperienceEditorConstants.Views.BlankViewPath,
                Rendering = rendering
            };
        }
    }
}