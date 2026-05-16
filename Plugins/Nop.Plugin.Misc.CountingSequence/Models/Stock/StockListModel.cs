using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.CountingSequence.Models.Stock
{
    /// <summary>
    /// Represents a stock list model
    /// </summary>
    public partial record StockListModel : BasePagedListModel<StockModel>
    {
    }
}
