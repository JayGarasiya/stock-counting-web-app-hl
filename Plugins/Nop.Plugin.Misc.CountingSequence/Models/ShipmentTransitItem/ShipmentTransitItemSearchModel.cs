using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.CountingSequence.Models.ShipmentTransitItem
{
    /// <summary>
    /// Represents a shipment transit item search model
    /// </summary>
    public record ShipmentTransitItemSearchModel : BaseSearchModel
    {
        public int ShipmentTransitId { get; set; }
        public int PalletId { get; set; }
    }
}
