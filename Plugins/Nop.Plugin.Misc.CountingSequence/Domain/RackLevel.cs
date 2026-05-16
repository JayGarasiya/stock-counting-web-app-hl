using Nop.Core;

namespace Nop.Plugin.Misc.CountingSequence.Domain
{
    /// <summary>
    /// Represents a rack level
    /// </summary>
    public class RackLevel : BaseEntity
    {
        /// <summary>
        ///  Gets or sets rack indentifier
        /// </summary>
        public int RackId { get; set; }

        /// <summary>
        ///  Gets or sets level indentifier
        /// </summary>
        public int LevelId { get; set; }

        
    }
}
