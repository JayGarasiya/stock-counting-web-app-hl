using Nop.Core.Domain.Catalog;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.CountingSequence.Models.Pallet
{
    /// <summary>
    /// Represents a pallet product model
    /// </summary>
    public partial record PalletProductModel : BaseNopEntityModel
    {
        public PalletProductModel()
        {
            if (PageSize < 1)
            {
                PageSize = 5;
            }

            PalletProductSearchModel = new PalletProductSearchModel();
        }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.Pallet.PalletId")]
        public int PalletId { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.Pallet.ProductId")]
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.Pallet.CategoryName")]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.Pallet.Specification")]
        public string Specification { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.Pallet.PageSize")]
        public int PageSize { get; set; }
        public PalletProductSearchModel PalletProductSearchModel { get; set; }

    }
}
