using Nop.Core;

namespace Nop.Plugin.Misc.CountingSequence.Domain
{
    /// <summary>
    /// Represents a rack product
    /// </summary>
    public class RackProduct : BaseEntity
    {
        /// <summary>
        ///  Gets or sets rack identifier
        /// </summary>
        public int RackId { get; set; }

        /// <summary>
        ///  Gets or sets product identifier
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        ///  Gets or sets level identifier
        /// </summary>
        public int RackLevelId { get; set; }

        /// <summary>
        ///  Gets or sets rack product position identifier
        /// </summary>
        public int ProductPositionId { get; set; }

        public RackProductPosition RackProductPositionEnum
        {
            get => (RackProductPosition)ProductPositionId;
            set => ProductPositionId = (int)value;
        }

    }
}
