using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SnippetVault.UI.TagHelpers
{
    [HtmlTargetElement("a", Attributes = "active-route-css-class")]
    public class AnchorCurrentRouteTagHelper : TagHelper
    {
        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("active-route-css-class")]
        public string? ActiveCssClass { get; set; }

        [HtmlAttributeName("additional-routes-to-match")]
        public string? AdditionalRoutesToMatch { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var requestPath = ViewContext.HttpContext?.Request.Path.Value;

            var hrefValue = output.Attributes["href"]?.Value.ToString();
            var additionalRoutesToMatch = AdditionalRoutesToMatch?.Split(",");

            var currentRoute = false;

            if (additionalRoutesToMatch != null)
            {
                currentRoute = additionalRoutesToMatch.Any(el => el == requestPath);
            }

            if (requestPath == hrefValue)
            {
                currentRoute = true;
            }

            if (currentRoute)
            {
                var existedCssClasses = output.Attributes["class"]?.Value;
                output.Attributes.SetAttribute("class", $"{ActiveCssClass} {existedCssClasses}");
            }

            base.Process(context, output);
        }
    }
}