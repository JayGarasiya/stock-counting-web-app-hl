using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.CountingSequence.Models.BackOrders
{
    /// <summary>
    /// Represents a back orders list model
    /// </summary>
    public partial record BackOrdersListModel : BasePagedListModel<BackOrdersModel>
    {
    }
}
