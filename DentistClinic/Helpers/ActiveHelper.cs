using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DentistClinic.Helpers
{
    [HtmlTargetElement("a", Attributes = "asp-active")]
    public class ActiveHelper: TagHelper
    {
        public string? AspActive { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContextData { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            if (string.IsNullOrEmpty(AspActive))
                return;

            var currentController = ViewContextData?.HttpContext.GetRouteValue("controller") ?? string.Empty;
            if (currentController!.Equals(AspActive))
            {
                if (output.Attributes.ContainsName("class"))
                    output.Attributes.SetAttribute("class", $"{output.Attributes["class"].Value} active");
                else
                    output.Attributes.SetAttribute("class", "active");
            }
        }
    }


}


