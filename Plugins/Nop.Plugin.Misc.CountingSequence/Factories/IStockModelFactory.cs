using Nop.Plugin.Misc.CountingSequence.Models.Stock;

namespace Nop.Plugin.Misc.CountingSequence.Factories
{
    /// <summary>
    /// Represents a stock model factory interface
    /// </summary>
    public interface IStockModelFactory
    {
        /// <summary>
        /// Prepare stock search model
        /// </summary>
        /// <param name="searchModel">Stock search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the stock search model
        /// </returns>
        Task<StockSearchModel> PrepareStockSearchModelAsync(StockSearchModel searchModel);

        /// <summary>
        /// Prepare stock list model
        /// </summary>
        /// <param name="searchModel">Stock search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the stock list model
        /// </returns>
        Task<StockListModel> PrepareStockListModelAsync(StockSearchModel searchModel);

    }
}
