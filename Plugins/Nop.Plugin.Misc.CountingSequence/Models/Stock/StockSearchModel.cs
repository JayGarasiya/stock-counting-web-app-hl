using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.CountingSequence.Models.Stock
{
    /// <summary>
    /// Represents a stock search model
    /// </summary>
    public partial record StockSearchModel : BaseSearchModel
    {
        public StockSearchModel()
        {
            AvailableProducts = new List<SelectListItem>();
            AvailableRacks = new List<SelectListItem>();
            AvailablePallets = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchProductName")]
        public int SearchProductId { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchRackName")]
        public int SearchRackId { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchPalletName")]
        public int SearchPalletId { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.NoOfUnitMin")]
        public int? NoOfUnitMin { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.NoOfUnitMax")]
        public int? NoOfUnitMax { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.NoOfPackMin")]
        public int? NoOfPackMin { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.NoOfPackMax")]
        public int? NoOfPackMax { get; set; }


        public IList<SelectListItem> AvailableProducts { get; set; }
        public IList<SelectListItem> AvailableRacks { get; set; }
        public IList<SelectListItem> AvailablePallets { get; set; }
    }
}
