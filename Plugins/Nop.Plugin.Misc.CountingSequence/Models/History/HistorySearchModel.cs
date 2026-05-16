using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.CountingSequence.Models.History
{
    /// <summary>
    /// Represents a history search model
    /// </summary>
    public partial record HistorySearchModel : BaseSearchModel
    {
        #region Ctor

        public HistorySearchModel()
        {
            AvailableShipment = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.HistorySearch.SearchProductId")]
        public int SearchProductId { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.HistorySearch.SearchMonth")]
        public int? SearchMonth { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.HistorySearch.SearchYear")]
        public int? SearchYear { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.HistorySearch.SearchOfUnit")]
        public int? SearchOfUnit { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.HistorySearch.SearchShipment")]
        public int SearchShipment { get; set; }

        public IList<SelectListItem> AvailableShipment { get; set; }


        #endregion
    }
}
