using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.CountingSequence.Models.ShipmentTransit
{
    /// <summary>
    /// Represents a shipment transit model
    /// </summary>
    public record ShipmentTransitPalletSearchModel : BaseSearchModel
    {
        public int ShipmentTransitId { get; set; }
    }
}
