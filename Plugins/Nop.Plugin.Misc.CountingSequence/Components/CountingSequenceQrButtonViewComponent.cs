using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Misc.CountingSequence.Components
{
    /// <summary>
    /// Represents view component to display qr button on product details page
    /// </summary>
    public class CountingSequenceQrButtonViewComponent : NopViewComponent
    {
        /// <summary>
        /// Invoke view component
        /// </summary>
        /// <param name="widgetZone">Widget zone name</param>
        /// <param name="additionalData">Additional data</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the view component result
        /// </returns>
        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            return View("~/Plugins/Misc.CountingSequence/Views/CountingSequenceQrButton.cshtml", additionalData);

        }
    }
}
