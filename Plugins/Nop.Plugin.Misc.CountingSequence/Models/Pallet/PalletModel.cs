using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.CountingSequence.Models.Pallet
{
    /// <summary>
    /// Represents a pallet model
    /// </summary>
    public record PalletModel : BaseNopEntityModel
    {
        public PalletModel()
        {
            if (PageSize < 1)
            {
                PageSize = 5;
            }

            PalletProductSearchModel = new PalletProductSearchModel();
        }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.Pallet.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.Pallet.ShipmentDispatchId")]
        public int ShipmentDispatchId { get; set; }
        public string ShipmentDispatchName { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.Pallet.SequenceOrder")]
        public int SequenceOrder { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.Pallet.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.Pallet.IsVisible")]
        public bool IsVisible { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.Pallet.Description")]
        public string Description { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.PageSize")]
        public int PageSize { get; set; }
        public PalletProductSearchModel PalletProductSearchModel { get; set; }

        public IList<SelectListItem> AvailableShipmentDispatch { get; set; }
    }
}
