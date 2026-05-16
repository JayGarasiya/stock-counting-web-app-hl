using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.CountingSequence.Models.CountingSequence
{
    public record StockCountItemModel : BaseNopEntityModel
    {
        public int StockCountId { get; set; }
        public int RackId { get; set; }
        public int PalletId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CountSheetXml { get; set; }
        public int Quantity { get; set; }
        public int LastMonthQuantity { get; set; }
        public int ProgressStatusId { get; set; }
        public DateTime CreatedOnUtc { get; set; }
    }
}
