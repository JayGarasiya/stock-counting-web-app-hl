using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.CountingSequence.Models.Pallet
{
    public partial record AddProductToPalletModel : BaseSearchModel
    {
        /// <summary>
        /// Represents an add product to pallet model
        /// </summary>
        
        #region Ctor

        public AddProductToPalletModel()
        {
            SelectedProductIds = new List<int>();
        }
        #endregion

        #region Properties

        public IList<int> SelectedProductIds { get; set; }

        public int PalletId { get; set; }

        #endregion


    }

}
