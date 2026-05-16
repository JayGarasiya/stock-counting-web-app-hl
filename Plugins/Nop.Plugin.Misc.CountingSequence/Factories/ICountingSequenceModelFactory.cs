using Nop.Plugin.Misc.CountingSequence.Models.CountingSequence;

namespace Nop.Plugin.Misc.CountingSequence.Factories
{
    /// <summary>
    /// Prepare counting sequence model factory interface
    /// </summary>
    public interface ICountingSequenceModelFactory
    {
        /// <summary>
        /// Prepare counting sequence search model
        /// </summary>
        /// <param name="searchModel">Pallet search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the pallet search model
        /// </returns>
        Task<StockCountSearchModel> PrepareCountingSequenceSearchModelAsync(StockCountSearchModel searchModel);

        /// <summary>
        /// Prepare counting sequence list model
        /// </summary>
        /// <param name="searchModel">Pallet search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the pallet list model
        /// </returns>
        Task<StockCountListModel> PrepareCountingSequenceListModelAsync(StockCountSearchModel searchModel);
    }
}
