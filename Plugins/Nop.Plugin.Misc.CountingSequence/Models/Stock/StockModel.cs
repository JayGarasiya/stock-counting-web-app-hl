using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.CountingSequence.Models.Stock
{
    /// <summary>
    /// Represents a stock model
    /// </summary>
    public record StockModel : BaseNopEntityModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public string RackName { get; set; }
        public string PalletName { get; set; }
        public int LevelTypeId { get; set; }
        public string LevelTypeName { get; set; }

        public int BegQuantity { get; set; }
        public int Quantity { get; set; }
        public int NoOfPack { get; set; }
        public int NoOfUnit { get; set; }

        public string WarehouseName { get; set; }
    }
}