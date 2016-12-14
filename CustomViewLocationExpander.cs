using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Razor;

namespace SoundBoardPOC
{
    public class CustomViewLocationExpander : IViewLocationExpander  
{
    public void PopulateValues(ViewLocationExpanderContext context)
    {
        context.Values["customviewlocation"] = nameof(CustomViewLocationExpander);
    }

    public IEnumerable<string> ExpandViewLocations(
              ViewLocationExpanderContext context, 
              IEnumerable<string> viewLocations)
    {
        var viewLocationFormats = new[]
        {
            "~/Pages/{0}.cshtml",
            "~/Pages/{1}/{0}.cshtml"
        };
        return viewLocationFormats;
    }
}
}