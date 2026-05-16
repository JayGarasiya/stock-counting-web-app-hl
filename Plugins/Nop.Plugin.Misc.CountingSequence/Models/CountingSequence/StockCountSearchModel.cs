using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Nop.Plugin.Misc.CountingSequence.Models.CountingSequence
{
    /// <summary>
    /// Represent a stock count search model
    /// </summary>
    public record StockCountSearchModel : BaseSearchModel
    {
        public StockCountSearchModel()
        {
            AvailableWarehouse = new List<SelectListItem>();
            AvailableCountType = new List<SelectListItem>();
            AvailableProgressStatusType = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.StockCountSearch.SearchName")]
        public string SearchName { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.StockCountSearch.SearchWarehouseId")]
        public int SearchWarehouseId { get; set; }

        public IList<SelectListItem> AvailableWarehouse { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.StockCountSearch.SearchCountTypeId")]
        public int SearchCountTypeId { get; set; }

        public IList<SelectListItem> AvailableCountType { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.StockCountSearch.SearchProgressStatusTypeId")]
        public int SearchProgressStatusTypeId { get; set; }

        public IList<SelectListItem> AvailableProgressStatusType { get; set; }

        [UIHint("DateNullable")]
        [NopResourceDisplayName("Plugins.Misc.CountingSequence.StockCountSearch.SearchFromDate")]
        public DateTime? SearchFromDate { get; set; }

        [UIHint("DateNullable")]
        [NopResourceDisplayName("Plugins.Misc.CountingSequence.StockCountSearch.SearchToDate")]
        public DateTime? SearchToDate { get; set; }
    }
}
