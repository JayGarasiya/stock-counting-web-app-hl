using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.CountingSequence.Models.Rack
{
    public partial record RackLevelTypeSearchModel : BaseSearchModel
    {
        #region Ctor

        public RackLevelTypeSearchModel()
        {
            AvailableFunctionType = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchRackLevelName")]
        public string SearchRackLevelName { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchFunctionTypeId")]
        public int SearchFunctionTypeId { get; set; }

        public IList<SelectListItem> AvailableFunctionType { get; set; }

        #endregion
    }
}
