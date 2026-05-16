using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Nop.Plugin.Misc.CountingSequence.Models.BackOrders
{
    /// <summary>
    /// Represents a back orders search model
    /// </summary>
    public partial record BackOrdersSearchModel : BaseSearchModel
    {
        #region Ctor

        public BackOrdersSearchModel()
        {
            AvailableStatus = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchRefrenceNumber")]
        public string SearchRefrenceNumber { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchChennel")]
        public int SearchChennelId { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.Channel.SearchProductId")]
        public int SearchProductId { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.MinQuantity")]
        public int? MinQuantity { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.MaxQuantity")]
        public int? MaxQuantity { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchSatus")]
        public int SearchSatus { get; set; }

        [NopResourceDisplayName("Admin.Orders.List.StartDate")]
        [UIHint("DateNullable")]
        public DateTime? StartDate { get; set; }

        [NopResourceDisplayName("Admin.Orders.List.EndDate")]
        [UIHint("DateNullable")]
        public DateTime? EndDate { get; set; }

        public IList<SelectListItem> AvailableStatus { get; set; }

        #endregion
    }
}
