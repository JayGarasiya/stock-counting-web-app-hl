using Nop.Core;

namespace Nop.Plugin.Misc.CountingSequence.Domain
{
    /// <summary>
    /// Represents a shipment transit
    /// </summary>
    public class ShipmentTransit : BaseEntity
    {
        /// <summary>
        ///  Gets or sets from dispatch identifier
        /// </summary>
        public int FromDispatchId { get; set; }

        /// <summary>
        ///  Gets or sets status
        /// </summary>
        public int Status { get; set; }
    }
}
