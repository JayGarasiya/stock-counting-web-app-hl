using Nop.Core;

namespace Nop.Plugin.Misc.CountingSequence.Domain
{
    /// <summary>
    /// Represents a rack stock
    /// </summary>
    public class RackStock : BaseEntity
    {
        /// <summary>
        ///  Gets or sets rack identifier
        /// </summary>
        public int RackId { get; set; }

        /// <summary>
        ///  Gets or sets level identifier
        /// </summary>
        public int LevelId { get; set; }

        /// <summary>
        ///  Gets or sets product identifier
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        ///  Gets or sets warehouse identifier
        /// </summary>
        public int WarehouseId { get; set; }

        /// <summary>
        ///  Gets or sets no of pack
        /// </summary>
        public int NoOfPack { get; set; }

        /// <summary>
        ///  Gets or sets no of unit
        /// </summary>
        public int NoOfUnit { get; set; }

        /// <summary>
        ///  Gets or sets quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        ///  Gets or sets stock date
        /// </summary>
        //public DateTime StockDate { get; set; }

        /// <summary>
        ///  Gets or sets to product position identifier 
        /// </summary>
        public int ProductPositionId { get; set; }

        /// <summary>
        ///  Gets or sets to Stock Count identifier 
        /// </summary>
        //public int StockCountId { get; set; }
    }
}
