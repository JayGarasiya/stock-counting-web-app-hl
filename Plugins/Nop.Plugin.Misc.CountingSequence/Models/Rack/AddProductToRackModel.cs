using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.CountingSequence.Models.Rack
{
    /// <summary>
    /// Represents an add product to rack model
    /// </summary>
    public partial record AddProductToRackModel : BaseSearchModel
    {
        #region Ctor

        public AddProductToRackModel()
        {
            SelectedProductIds = new List<int>();
            AvailableProductPositions = new List<SelectListItem>();
            ProductPositions = new Dictionary<int, int>();
        }

        #endregion

        #region Properties

        public int RackId { get; set; }

        public int LevelId { get; set; }

        public int ProductPositionId { get; set; }

        public IList<int> SelectedProductIds { get; set; }

        public IList<SelectListItem> AvailableProductPositions { get; set; }

        public Dictionary<int, int> ProductPositions { get; set; }
        #endregion

    }
}
