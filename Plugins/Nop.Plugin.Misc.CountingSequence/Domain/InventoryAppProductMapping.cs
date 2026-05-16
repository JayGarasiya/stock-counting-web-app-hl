using Nop.Core;

namespace Nop.Plugin.Misc.CountingSequence.Domain
{
    /// <summary>
    /// Represents a Inventory App Product Mapping
    /// </summary>
    public class InventoryAppProductMapping : BaseEntity
    {
        /// <summary>
        ///  Gets or sets Inventory Product Model Identifier
        /// </summary>
        public int InventoryProductModelId { get; set; }

        /// <summary>
        ///  Gets or sets Product
        /// </summary>
        public int CountProductId { get; set; }
    }
}
