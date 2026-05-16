using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.CountingSequence.Models.History
{
    /// <summary>
    /// Represents a history list model
    /// </summary>
    public partial record HistoryListModel : BasePagedListModel<HistoryModel>
    {
    }
}
