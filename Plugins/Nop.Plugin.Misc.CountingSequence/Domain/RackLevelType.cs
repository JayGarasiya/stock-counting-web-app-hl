using Nop.Core;

namespace Nop.Plugin.Misc.CountingSequence.Domain
{
    public class RackLevelType : BaseEntity
    {
        /// <summary>
        /// Gets or sets name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///  Gets or sets is function identifier 
        /// </summary>
        public int FunctionTypeId { get; set; }

        /// <summary>
        /// Gets or sets display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets is visible
        /// </summary>
        public bool IsVisible { get; set; }

        public RackFunctionType RackLevelTypeEnum
        {
            get => (RackFunctionType)FunctionTypeId;
            set => FunctionTypeId = (int)value;
        }
    }
}
