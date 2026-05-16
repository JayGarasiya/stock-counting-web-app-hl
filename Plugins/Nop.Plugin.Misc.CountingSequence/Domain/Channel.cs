using Nop.Core;

namespace Nop.Plugin.Misc.CountingSequence.Domain
{
    /// <summary>
    /// Represents a channel
    /// </summary>
    public class Channel : BaseEntity
    {
        /// <summary>
        ///  Gets or sets name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///  Gets or sets channel indentifier
        /// </summary>
        public int ChannelId { get; set; }

        /// <summary>
        ///  Gets or sets description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///  Gets or sets display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        ///  Gets or sets visible
        /// </summary>
        public bool Visible { get; set; }

        public ChannelType BackOrdersEnum
        {
            get => (ChannelType)ChannelId;
            set => ChannelId = (int)value;
        }
    }
}
