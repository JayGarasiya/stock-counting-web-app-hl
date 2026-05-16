using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.CountingSequence.Models.CountingSequence
{
    /// <summary>
    /// Represent a stock count list model
    /// </summary>
    public record StockCountListModel : BasePagedListModel<StockCountModel>
    {
    }
}
