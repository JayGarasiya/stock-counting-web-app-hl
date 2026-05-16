using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.CountingSequence.Models.Rack
{
    /// <summary>
    /// Represents a rack model
    /// </summary>
    public record RackModel : BaseNopEntityModel
    {
        #region Ctor

        public RackModel()
        {
            if (PageSize < 1)
            {
                PageSize = 5;
            }

            RackProductSearchModel = new RackProductSearchModel();

            AvailableLevelType = new List<SelectListItem>();

            AvailableFunctionType = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Rack.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Rack.Fields.Description")]
        public string Description { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Rack.Fields.SequenceOrder")]
        public int SequenceOrder { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Rack.Fields.LevelTypeId")]
        public IList<int> LevelTypeIds { get; set; } = new List<int>();
        public string LevelTypeName { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Rack.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Rack.Fields.IsVisible")]
        public bool IsVisible { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Rack.Fields.FunctionTypeId")]
        public int FunctionTypeId { get; set; }

        public string FunctionTypeName { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.PageSize")]
        public int PageSize { get; set; }
        public RackProductSearchModel RackProductSearchModel { get; set; }


        public IList<SelectListItem> AvailableFunctionType { get; set; }
        public IList<SelectListItem> AvailableLevelType { get; set; }

        #endregion
    }
}
