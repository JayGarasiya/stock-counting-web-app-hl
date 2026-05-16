using Nop.Core;

namespace Nop.Plugin.Misc.CountingSequence.Domain
{
    /// <summary>
    /// Represents a warehouse stock history
    /// </summary>
    public class WarehouseStockHistory : BaseEntity
    {
        /// <summary>
        ///  Gets or sets product identifier
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        ///  Gets or sets warehouse identifier
        /// </summary>
        public int WarehouseId { get; set; }

        /// <summary>
        ///  Gets or sets rack stock identifier
        /// </summary>
        public int RackId { get; set; }

        /// <summary>
        ///  Gets or sets pallet stock identifier
        /// </summary>
        public int PalletId { get; set; }

        /// <summary>
        ///  Gets or sets stock count identifier
        /// </summary>
        public int StockCountId { get; set; }

        /// <summary>
        ///  Gets or sets quantity adjustment
        /// </summary>
        public int QuantityAdjustment { get; set; }

        /// <summary>
        ///  Gets or sets stock quantity
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        ///  Gets or sets message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///  Gets or sets stock date
        /// </summary>
        public DateTime StockDate { get; set; }
    }
}
