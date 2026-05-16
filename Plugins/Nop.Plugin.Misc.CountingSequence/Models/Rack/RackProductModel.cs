using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.CountingSequence.Models.Rack
{
    /// <summary>
    /// Represents a rack product model
    /// </summary>
    public record RackProductModel : BaseNopEntityModel
    {
        #region Properties

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Rack.Fields.RackId")]
        public int RackId { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Rack.Fields.ProductId")]
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Rack.Fields.CategoryName")]
        public string CategoryName { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Rack.Fields.Specification")]
        public string Specification { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Rack.Fields.ProductPositionId")]
        public int ProductPositionId { get; set; }
        public string ProductPositionName { get; set; }

        #endregion
    }
}
