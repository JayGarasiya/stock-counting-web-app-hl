using Nop.Core;

namespace Nop.Plugin.Misc.CountingSequence.Domain
{
    /// <summary>
    /// Represents a shipment transit items
    /// </summary>
    public class ShipmentTransitItems : BaseEntity
    {
        /// <summary>
        ///  Gets or sets shipment transit identifier
        /// </summary>
        public int ShipmentTransitId { get; set; }

        /// <summary>
        ///  Gets or sets product identifier
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        ///  Gets or sets pallet identifier
        /// </summary>
        public int PalletId { get; set; }

        /// <summary>
        ///  Gets or sets quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        ///  Gets or sets NoOfUnits 
        /// </summary>
        public int NoOfUnits { get; set; }

        /// <summary>
        ///  Gets or sets NoOfPacks 
        /// </summary>
        public int NoOfPacks { get; set; }

        /// <summary>
        ///  Gets or sets to dispatch identifier 
        /// </summary>
        public int ToDispatchId { get; set; }
    }
}
