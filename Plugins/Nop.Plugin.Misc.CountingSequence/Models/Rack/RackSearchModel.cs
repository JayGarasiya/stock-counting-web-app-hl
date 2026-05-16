using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.CountingSequence.Models.Rack
{
    /// <summary>
    /// Represents a rack search model
    /// </summary>
    public partial record RackSearchModel : BaseSearchModel
    {
        #region Ctor

        public RackSearchModel()
        {
            AvailableVisibleOptions = new List<SelectListItem>();
            AvailableLevelTypes = new List<SelectListItem>();
            AvailableFunctionType = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchRackName")]
        public string SearchRackName { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchLevelTypeId")]
        public int SearchLevelTypeId { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchVisible")]
        public int SearchVisible { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchProductId")]
        public int SearchProductId { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchFunctionTypeId")]
        public int SearchFunctionTypeId { get; set; }

        public IList<SelectListItem> AvailableFunctionType { get; set; }

        public IList<SelectListItem> AvailableVisibleOptions { get; set; }

        public IList<SelectListItem> AvailableLevelTypes { get; set; }

        #endregion
    }
}
