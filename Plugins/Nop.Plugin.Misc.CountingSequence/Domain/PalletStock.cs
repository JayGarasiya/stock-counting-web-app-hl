using Nop.Core;

namespace Nop.Plugin.Misc.CountingSequence.Domain
{
    /// <summary>
    /// Represents a Pallet Stock
    /// </summary>
    public class PalletStock : BaseEntity
    {
        /// <summary>
        ///  Gets or sets Pallet Id
        /// </summary>
        public int PalletId { get; set; }

        /// <summary>
        ///  Gets or sets Product Id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        ///  Gets or sets Warehouse Id
        /// </summary>
        public int WarehouseId { get; set; }

        /// <summary>
        ///  Gets or sets No Of Pack
        /// </summary>
        public int NoOfPack { get; set; }

        /// <summary>
        ///  Gets or sets No Of Unit
        /// </summary>
        public int NoOfUnit { get; set; }

        /// <summary>
        ///  Gets or sets Quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        ///  Gets or sets Stock Date
        /// </summary>
        //public DateTime StockDate { get; set; }

        /// <summary>
        ///  Gets or sets to dispatch identifier 
        /// </summary>
        public int ToDispatchId { get; set; }

        /// <summary>
        ///  Gets or sets to Shipment Transit identifier 
        /// </summary>
        public int ShipmentTransitId { get; set; }


        /// <summary>
        ///  Gets or sets to product position identifier 
        /// </summary>
        public int AvailableQuantity { get; set; }

        /// <summary>
        ///  Gets or sets to Stock Count identifier 
        /// </summary>
        //public int StockCountId { get; set; }
    }
}
