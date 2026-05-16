using Nop.Core;

namespace Nop.Plugin.Misc.CountingSequence.Domain
{
    /// <summary>
    /// Represents an stock count
    /// </summary>
    public class StockCount : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier
        /// </summary>
        public int CutsomerId { get; set; }

        /// <summary>
        /// Gets or sets the warehouse identifier
        /// </summary>
        public int WarehouseId { get; set; }

        /// <summary>
        /// Gets or sets the progress status identifier
        /// </summary>
        public int ProgressStatusId { get; set; }

        /// <summary>
        /// Gets or sets the date and time of stock count creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets discontinue pallet identifier
        /// </summary>
        public string DiscontinuePalletIds { get; set; }

        /// <summary>
        /// Gets or sets delivered shipment identifier
        /// </summary>
        public string DeliveredShipmentIds { get; set; }


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
