using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Nop.Plugin.Misc.CountingSequence.Models.BackOrders
{
    /// <summary>
    /// Represents a back orders model
    /// </summary>
    public record BackOrdersModel : BaseNopEntityModel
    {
        #region Ctor

        public BackOrdersModel()
        {
            AvailableStatus = new List<SelectListItem>();
            AvailableProduct = new List<SelectListItem>();
            AvailableChannel = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.BackOrder.Fields.ReferenceNo")]
        public string ReferenceNo { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.BackOrder.Fields.ChannelId")]
        public int ChannelId { get; set; }
        public string ChannelName { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.BackOrder.Fields.ProductId")]
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.BackOrder.Fields.Quantity")]
        public int Quantity { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.BackOrder.Fields.Status")]
        public int Status { get; set; }
        public string Statuss { get; set; }

        [UIHint("Date")]
        [NopResourceDisplayName("Plugins.Misc.CountingSequence.BackOrder.Fields.OrderDate")]
        public DateTime OrderDate { get; set; }

        public bool CanEditDelete { get; set; }

        public IList<SelectListItem> AvailableStatus { get; set; }

        public IList<SelectListItem> AvailableProduct { get; set; }

        public IList<SelectListItem> AvailableChannel { get; set; }

        #endregion
    }
}
