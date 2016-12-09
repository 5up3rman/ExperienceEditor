using System.Text;
using Sitecore;
using Sitecore.Data;
using Sitecore.Globalization;
using Sitecore.Mvc.Presentation;
using Sitecore.Pipelines.GetChromeData;
using Sitecore.Web.UI.PageModes;
using Sitecore.Web.UI.WebControls;

namespace SitecoreSuperman.ExperienceEditor.RenderRendering
{
    public class DatasourceRenderingMarker : Sitecore.Mvc.ExperienceEditor.Presentation.IMarker
    {
        private RenderingContext RenderingContext { get; set; }
        private string RenderingName { get; set; }
        private string ControlId { get; set; }
        private ChromeData clientData;
        protected ChromeData ClientData => this.clientData ?? (this.clientData = this.GetClientData());

        public DatasourceRenderingMarker(RenderingContext renderingContext, string renderingName)
        {
            RenderingContext = renderingContext;
            RenderingName = renderingName;
            ControlId = ID.Parse(renderingContext.Rendering.UniqueId).ToShortID().ToString();
        }

        public string GetStart()
        {
            if (this.ClientData == null)
                return string.Empty;

            var sb = new StringBuilder();
            sb.AppendLine("<div class=\"ee set-datasource\">");
            sb.AppendLine("<i class=\"alert-ico\"></i>");
            sb.AppendLine($"<span class=\"message-bar\">{Translate.Text("Please set the Datasource on")} {RenderingName}</span>");
            sb.AppendLine("</div>");

            return string.Concat(Placeholder.GetControlStartMarker(this.ControlId, this.ClientData, true), sb.ToString());
        }

        public string GetEnd()
        {
            return this.ClientData != null
                ? Placeholder.GetControlEndMarker(this.ClientData, this.ControlId)
                : string.Empty;
        }

        protected ChromeData GetClientData()
        {
            var args = new GetChromeDataArgs("rendering", this.RenderingContext.Rendering.Item);
            var renderingReference = this.RenderingContext.Rendering.GetRenderingReference(Context.Language,
                this.RenderingContext.PageContext.Database);
            args.CustomData["renderingReference"] = (object) renderingReference;
            GetChromeDataPipeline.Run(args);

            return args.ChromeData;
        }
    }
}