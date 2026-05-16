using Nop.Core;

namespace Nop.Plugin.Misc.CountingSequence.Domain
{
    /// <summary>
    /// Represents an stock count item
    /// </summary>
    public class StockCountItem : BaseEntity
    {
        /// <summary>
        /// Gets or sets the stock count identifier
        /// </summary>
        public int StockCountId { get; set; }

        /// <summary>
        /// Gets or sets the rack identifier
        /// </summary>
        public int RackId { get; set; }

        /// <summary>
        /// Gets or sets the pallet identifier
        /// </summary>
        public int PalletId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the count sheet xml
        /// </summary>
        public string CountSheetXml { get; set; }

        /// <summary>
        /// Gets or sets the quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the progress status identifier
        /// </summary>
        public int ProgressStatusId { get; set; }

        /// <summary>
        /// Gets or sets the date and time of stock count item creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the progress status
        /// </summary>
        public ProgressStatus ProgressStatus
        {
            get => (ProgressStatus)ProgressStatusId;
            set => ProgressStatusId = (int)value;
        }
    }
}
