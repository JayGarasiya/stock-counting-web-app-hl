using Nop.Core;
    
namespace Nop.Plugin.Misc.CountingSequence.Domain
{
    /// <summary>
    /// Represents a shipment dispatches
    /// </summary>
    public class ShipmentDispatches : BaseEntity
    {
        /// <summary>
        ///  Gets or sets name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///  Gets or sets dispatch type
        /// </summary>
        public int DispatchType { get; set; }

        /// <summary>
        ///  Gets or sets shipped month
        /// </summary>
        public int ShippedMonth { get; set; }

        /// <summary>
        ///  Gets or sets shipped year
        /// </summary>
        public int ShippedYear { get; set; }

        /// <summary>
        ///  Gets or sets display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        ///  Gets or sets visible
        /// </summary>
        public bool Visible { get; set; }
    }
}
