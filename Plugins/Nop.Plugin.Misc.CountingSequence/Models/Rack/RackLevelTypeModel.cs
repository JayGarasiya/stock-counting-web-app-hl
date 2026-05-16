using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.CountingSequence.Models.Rack
{
    public record RackLevelTypeModel : BaseNopEntityModel
    {
        #region Ctor

        public RackLevelTypeModel()
        {
            AvailableFunctionType = new List<SelectListItem>();
        }

        #endregion

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.RackLevelType.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.RackLevelType.FunctionTypeId")]
        public int FunctionTypeId { get; set; }
        public string FunctionTypeName { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.RackLevelType.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.RackLevelType.IsVisible")]
        public bool IsVisible { get; set; }

        public IList<SelectListItem> AvailableFunctionType { get; set; }
    }
}
