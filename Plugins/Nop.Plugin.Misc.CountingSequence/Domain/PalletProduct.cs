using Nop.Core;

namespace Nop.Plugin.Misc.CountingSequence.Domain
{
    /// <summary>
    /// Represents a Pallet Product
    /// </summary>
    public class PalletProduct : BaseEntity
    {
        /// <summary>
        ///  Gets or sets Pallet Id
        /// </summary>
        public int PalletId { get; set; }

        /// <summary>
        ///  Gets or sets Product Id
        /// </summary>
        public int ProductId { get; set; }

    }
}
