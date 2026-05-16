using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.CountingSequence.Models.Pallet
{
    /// <summary>
    /// Represents a pallet search model
    /// </summary>
    public partial record PalletSearchModel : BaseSearchModel
    {
        public PalletSearchModel()
        {
            PalletProductSearchModel = new PalletProductSearchModel();
            AvailableVisibleOptions = new List<SelectListItem>();
            AvailableShipmentDispatch = new List<SelectListItem>();
            AvailableProducts = new List<SelectListItem>();

        }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchPalletName")]
        public string SearchPalletName { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchShipmentDispatchId")]
        public int SearchShipmentDispatchId { get; set; }
        public IList<SelectListItem> AvailableShipmentDispatch { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchVisible")]
        public int SearchVisible { get; set; }
        public IList<SelectListItem> AvailableVisibleOptions { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchProductName")]
        public int SearchProductId { get; set; }

        public IList<SelectListItem> AvailableProducts { get; set; }

        public PalletProductSearchModel PalletProductSearchModel { get; set; }
    }
}
