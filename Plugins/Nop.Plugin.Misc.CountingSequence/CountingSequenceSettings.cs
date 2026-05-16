using Nop.Core.Configuration;
using Nop.Plugin.Misc.CountingSequence.Domain;

namespace Nop.Plugin.Misc.CountingSequence
{
    /// <summary>
    /// Represents plugin settings
    /// </summary>
    public class CountingSequenceSettings : ISettings
    {
        #region Properties

        public bool Enabled { get; set; }

        public PalletCounting PalletCountingOrder { get; set; }

        public int SaleTypeOrder { get; set; }

        public SequenceLevel Level { get; set; }
        public SequencePositions Position { get; set; }

        #endregion
    }
}
