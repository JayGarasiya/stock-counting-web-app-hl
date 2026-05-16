using Nop.Core;

namespace Nop.Plugin.Misc.CountingSequence.Domain
{
    /// <summary>
    /// Represents a History
    /// </summary>
    public class History : BaseEntity
    {
        /// <summary>
        ///  Gets or sets Product Id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        ///  Gets or sets Month
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        ///  Gets or sets Year
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        ///  Gets or sets Type Id
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        ///  Gets or sets Number Of Units
        /// </summary>
        public int NumberOfUnits { get; set; }

        /// <summary>
        ///  Gets or sets US Shipment Number
        /// </summary>
        public int USShipmentNumber { get; set; }

        /// <summary>
        ///  Gets or sets HL Shipment Number
        /// </summary>
        public int HLShipmentNumber { get; set; }

    }
}
