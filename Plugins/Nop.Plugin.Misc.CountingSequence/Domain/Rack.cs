using Nop.Core;

namespace Nop.Plugin.Misc.CountingSequence.Domain
{
    /// <summary>
    /// Represents a rack
    /// </summary>
    public class Rack : BaseEntity
    {
        /// <summary>
        ///  Gets or sets name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///  Gets or sets description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///  Gets or sets sequence order
        /// </summary>
        public int SequenceOrder { get; set; }

        /// <summary>
        ///  Gets or sets display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        ///  Gets or sets is visible
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        ///  Gets or sets is function type 
        /// </summary>
        public int FunctionTypeId { get; set; }

        public RackFunctionType RackLevelType
        {
            get => (RackFunctionType)FunctionTypeId;
            set => FunctionTypeId = (int)value;
        }

    }
}
