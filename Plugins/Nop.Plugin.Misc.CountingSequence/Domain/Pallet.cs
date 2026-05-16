using Nop.Core;

namespace Nop.Plugin.Misc.CountingSequence.Domain
{
    /// <summary>
    /// Represents a pallet
    /// </summary>
    public class Pallet : BaseEntity
    {
        /// <summary>
        /// Gets or sets name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Shipment Dispatch Id
        /// </summary>
        public int ShipmentDispatchId { get; set; }

        /// <summary>
        /// Gets or sets display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets is visible
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets is Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets is Sequence Order
        /// </summary>
        public int SequenceOrder { get; set; }
    }
}
