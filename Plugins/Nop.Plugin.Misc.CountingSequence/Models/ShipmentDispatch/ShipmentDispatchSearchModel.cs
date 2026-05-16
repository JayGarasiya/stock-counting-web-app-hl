using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.CountingSequence.Models.ShipmentDispatch
{
    /// <summary>
    /// Represents a shipment dispatch search model
    /// </summary>
    public record ShipmentDispatchSearchModel : BaseSearchModel
    {
        #region Ctor

        public ShipmentDispatchSearchModel()
        {
            AvailableDispatchType = new List<SelectListItem>();
            AvailableVisibleType = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchName")]
        public string SearchName { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchDispatchTypeId")]
        public int SearchDispatchTypeId { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchMinMonth")]
        public int? SearchMinMonth { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchMaxMonth")]
        public int? SearchMaxMonth { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchMinYear")]
        public int? SearchMinYear { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchMaxYear")]
        public int? SearchMaxYear { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchVisible")]
        public int SearchVisible { get; set; }

        public IList<SelectListItem> AvailableVisibleType { get; set; }
        public IList<SelectListItem> AvailableDispatchType { get; set; }

        #endregion
    }
}
