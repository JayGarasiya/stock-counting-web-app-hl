using Nop.Core;

namespace Nop.Plugin.Misc.CountingSequence.Domain
{
    /// <summary>
    /// Represents a back orders
    /// </summary>
    public class BackOrders : BaseEntity
    {
        /// <summary>
        ///  Gets or sets reference number
        /// </summary>
        public string ReferenceNo { get; set; }

        /// <summary>
        ///  Gets or sets channel identifier
        /// </summary>
        public int ChannelId { get; set; }

        /// <summary>
        ///  Gets or sets product identifier
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        ///  Gets or sets quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        ///  Gets or sets status
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        ///  Gets or sets order date
        /// </summary>
        public DateTime OrderDate { get; set; }


        public BackOrdersEnum BackOrdersEnum
        {
            get => (BackOrdersEnum)Status;
            set => Status = (int)value;
        }
    }
}
