using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.CountingSequence.Models.ShipmentTransitItem
{
    /// <summary>
    /// Represents a shipment transit item model
    /// </summary>
    public record ShipmentTransitItemModel : BaseNopEntityModel
    {
        public ShipmentTransitItemModel()
        {
            items = new List<TransitItem>();
        }

        public int ShipmentTransitId { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Pallet")]
        public int PalletId { get; set; }
        public string ProductName { get; set; }
        public string PalletName { get; set; }
        public int Quantity { get; set; }
        public int NoOfUnits { get; set; }
        public int NoOfPacks { get; set; }
        public int ToDispatchId { get; set; }
        public string ToDispatchName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Specifications { get; set; }
        public IList<TransitItem> items { get; set; }
        public IList<SelectListItem> AvailablePallets { get; set; } = new List<SelectListItem>();
        public IList<SelectListItem> AvailableProducts { get; set; } = new List<SelectListItem>();
        public IList<SelectListItem> AvailableHAndL { get; set; } = new List<SelectListItem>();
    }
    public class TransitItem
    {
        public int ProductId { get; set; }
        public int NoOfUnits { get; set; }
        public int NoOfPacks { get; set; }
        public int HAndL { get; set; }
    }
}
