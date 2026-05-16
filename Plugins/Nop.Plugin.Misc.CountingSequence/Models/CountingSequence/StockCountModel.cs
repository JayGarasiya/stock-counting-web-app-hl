using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.CountingSequence.Models.CountingSequence
{
    /// <summary>
    /// Represents a stock count model
    /// </summary>
    public record StockCountModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Stock.Fields.Name")]
        public string Name { get; set; }

        public int CustomerId { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Stock.Fields.CustomerName")]
        public string CustomerName { get; set; }

        public int WarehouseId { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Stock.Fields.WarehouseName")]
        public string WarehouseName { get; set; }

        public int ProgressStatusId { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Stock.Fields.ProgressStatusName")]

        public string ProgressStatusName { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Stock.Fields.CountType")]
        public string CountType { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Stock.Fields.CreatedOnUtc")]
        public string CreatedOnUtc { get; set; }
    }
}
