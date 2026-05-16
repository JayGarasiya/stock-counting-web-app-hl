using Nop.Plugin.Misc.CountingSequence.Models.History;

namespace Nop.Plugin.Misc.CountingSequence.Factories
{
    /// <summary>
    /// History model factory interface
    /// </summary>
    public interface IHistoryModelFactory
    {
        /// <summary>
        /// Prepare history search model
        /// </summary>
        /// <param name="searchModel">History search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the history search model
        /// </returns>
        Task<HistorySearchModel> PrepareHistorySearchModelAsync(HistorySearchModel searchModel);

        /// <summary>
        /// Prepare history list model
        /// </summary>
        /// <param name="searchModel">History search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the history list model
        /// </returns>
        Task<HistoryListModel> PrepareHistoryListModelAsync(HistorySearchModel searchModel);
    }
}
