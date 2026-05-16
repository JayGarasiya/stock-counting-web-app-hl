using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.CountingSequence.Models.ShipmentTransit
{
    /// <summary>
    /// Represents a shipment transit pallet model
    /// </summary>
    public record ShipmentTransitPalletModel : BaseNopEntityModel
    {
        public int PalletId { get; set; }
        public string PalletName { get; set; }
    }
}
