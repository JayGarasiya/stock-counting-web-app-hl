using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.CountingSequence.Models.History
{
    /// <summary>
    /// Represents a history model
    /// </summary>
    public record HistoryModel : BaseNopEntityModel
    {
        #region Properties

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int QuantityAdjustment { get; set; }

        public int StockQuantity { get; set; }

        public int NumberOfUnits { get; set; }

        public DateTime StockDate { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public string Type { get; set; }

        public int? USShipmentNumber { get; set; }

        public string USShipmentName { get; set; }

        public int? HLShipmentNumber { get; set; }

        public string HLShipmentName { get; set; }

        #endregion
    }
}
