using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.CountingSequence.Models.ShipmentTransit
{
    /// <summary>
    /// Represents a shipment transit search model
    /// </summary>
    public record ShipmentTransitSearchModel : BaseSearchModel
    {
        public ShipmentTransitSearchModel()
        {
            AvailableFromDisptch = new List<SelectListItem>();
            AvailableToDisptch = new List<SelectListItem>();
            AvailableStatus = new List<SelectListItem>();
        }
        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchFromDisptchId")]
        public int SearchFromDisptchId { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchProductName")]
        public int SearchProductId { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchPalletName")]
        public int SearchPalletId { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.ShipmentTransitSearchModel.SearchPalletName")]
        public int SearchStatus { get; set; }

       
        public IList<SelectListItem> AvailableFromDisptch { get; set; }
        public IList<SelectListItem> AvailableToDisptch { get; set; }
        public List<SelectListItem> AvailableStatus { get; set; }
    }
}
