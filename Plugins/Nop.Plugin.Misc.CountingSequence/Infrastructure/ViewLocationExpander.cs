using Microsoft.AspNetCore.Mvc.Razor;

namespace Nop.Plugin.Misc.CountingSequence.Infrastructure
{
    /// <summary>
    /// Specifies the contracts for a view location expander that is used by Microsoft.AspNetCore.Mvc.Razor.RazorViewEngine instances to determine search paths for a view.
    /// </summary>
    public class ViewLocationExpander : IViewLocationExpander
    {
        /// <summary>
        /// Invoked by a Microsoft.AspNetCore.Mvc.Razor.RazorViewEngine to determine the
        /// values that would be consumed by this instance of Microsoft.AspNetCore.Mvc.Razor.IViewLocationExpander.
        /// The calculated values are used to determine if the view location has changed since the last time it was located.
        /// </summary>
        /// <param name="context">Context</param>
        public void PopulateValues(ViewLocationExpanderContext context)
        {
        }

        /// <summary>
        /// Invoked by a Microsoft.AspNetCore.Mvc.Razor.RazorViewEngine to determine potential locations for a view.
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="viewLocations">View locations</param>
        /// <returns>iew locations</returns>
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            var area = context.AreaName;

            if (area == "Admin")
            {
                var adminLocations = new[]
                {
            "/Plugins/Misc.CountingSequence/Areas/Admin/Views/{1}/{0}.cshtml",
            "/Plugins/Misc.CountingSequence/Areas/Admin/Views/Shared/{0}.cshtml"
            };

                viewLocations = adminLocations.Concat(viewLocations);
            }

            if (context.ViewName == "Login")
            {
                viewLocations = new[]
                {
                    "/Plugins/Misc.CountingSequence/Views/Customer/Login.cshtml",
                }.Concat(viewLocations);
            }

            if (context.ViewName == "_Root")
            {
                viewLocations = new[]
                {
                    "/Plugins/Misc.CountingSequence/Views/Shared/_Root.cshtml",
                }.Concat(viewLocations);
            }

            return viewLocations;
        }
    }
}
