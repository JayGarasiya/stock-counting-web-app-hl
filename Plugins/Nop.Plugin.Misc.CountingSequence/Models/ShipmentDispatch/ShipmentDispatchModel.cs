using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.CountingSequence.Models.ShipmentDispatch
{
    /// <summary>
    /// Represents a shipment dispatch model
    /// </summary>
    public record ShipmentDispatchModel : BaseNopEntityModel
    {
        #region Ctor

        public ShipmentDispatchModel()
        {
            AvailableDispatchTypes = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.ShipmentDispatch.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.ShipmentDispatch.Fields.DispatchType")]
        public int DispatchType { get; set; }

        public string DispatchTypeName { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.ShipmentDispatch.Fields.ShippedMonth")]
        public int ShippedMonth { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.ShipmentDispatch.Fields.ShippedYear")]
        public int ShippedYear { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.ShipmentDispatch.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.ShipmentDispatch.Fields.Visible")]
        public bool Visible { get; set; }

        public IList<SelectListItem> AvailableDispatchTypes { get; set; }

        #endregion
    }
}
