using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.CountingSequence.Models.ShipmentTransit
{
    /// <summary>
    /// Represents a shipment transit model
    /// </summary>
    public record ShipmentTransitModel : BaseNopEntityModel
    {
        #region Ctor

        public ShipmentTransitModel()
        {
            if (PageSize < 1)
            {
                PageSize = 5;
            }

            ShipmentTransitPalletSearchModel = new ShipmentTransitPalletSearchModel();

            AvailableFromDispatches = new List<SelectListItem>();
            AvailableToDispatches = new List<SelectListItem>();
            AvailableStatuses = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.FromDispatchId")]
        public int FromDispatchId { get; set; }
        public string FromDispatchName { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.ST.Status")]
        public int Status { get; set; }
        public string StatusName { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.ST.PalletCount")]
        public int PalletCount { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.PageSize")]
        public int PageSize { get; set; }
        public ShipmentTransitPalletSearchModel ShipmentTransitPalletSearchModel { get; set; }

        public IList<SelectListItem> AvailableFromDispatches { get; set; }
        public IList<SelectListItem> AvailableToDispatches { get; set; }
        public IList<SelectListItem> AvailableStatuses { get; set; }

        #endregion
    }
}
