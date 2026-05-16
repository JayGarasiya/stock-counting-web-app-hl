using Nop.Core;
using Nop.Plugin.Misc.CountingSequence.Domain;

namespace Nop.Plugin.Misc.CountingSequence.Services
{
    /// <summary>
    /// Counting sequence service interface
    /// </summary>
    public partial interface ICountingSequenceService
    {
        /// <summary>
        /// Inserts a stock count
        /// </summary>
        /// <param name="stockCount">StockCount</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertStockCountAsync(StockCount stockCount);

        /// <summary>
        /// Updates a stock count
        /// </summary>
        /// <param name="stockCount">StockCount</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateStockCountAsync(StockCount stockCount);

        /// <summary>
        /// Inserts a stock count item
        /// </summary>
        /// <param name="stockCountItem">StockCountItem</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertStockCountItemAsync(StockCountItem stockCountItem);

        /// <summary>
        /// Updates a stock count item
        /// </summary>
        /// <param name="stockCountItem">StockCountItem</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateStockCountItemAsync(StockCountItem stockCountItem);

        /// <summary>
        /// Gets a stock Count
        /// </summary>
        /// <param name="stockCountId">StockCount identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the stock Count
        /// </returns>
        Task<StockCount> GetStockCountByIdAsync(int stockCountId);

        /// <summary>
        /// Get all stock count item
        /// </summary>
        /// <param name="stockCountId">Stock count indentifier</param>
        /// <param name="rackId">Rack indentifier</param>
        /// <param name="palletId">Pallet indentifier</param>
        /// <param name="productId">Product indentifier</param>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the all stock count item
        /// </returns>
        Task<IList<StockCountItem>> GetAllStockCountItem(
            int stockCountId = 0,
            int rackId = 0,
            int palletId = 0,
            int productId = 0,
            DateTime? startDate = null,
            DateTime? endDate = null);

        /// <summary>
        /// Get all stock count
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="warehouseId">Warehouse indentifier</param>
        /// <param name="countTypeId">Count Type indentifier</param>
        /// <param name="progressStatusId">Progress Status indentifier</param>
        /// <param name="createdFromUtc">Created date from utc</param>
        /// <param name="createdToUtc">Created date to utc</param> 
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the all stock count
        /// </returns>
        Task<IPagedList<StockCount>> GetAllStockCountAsync(string name = null,
            int warehouseId = 0,
            int countTypeId = 0,
            ProgressStatus? progressStatusId = null,
            DateTime? createdFromUtc = null,
            DateTime? createdToUtc = null,
            int pageIndex = 0,
            int pageSize = int.MaxValue);
        /// <summary>
        /// Delete the counting
        /// </summary>
        /// <param name="stockCount">Stock Count</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteStockCountAsync(StockCount stockCount);

        /// <summary>
        /// Delete the counting items
        /// </summary>
        /// <param name="stockCountItem">Stock Count Item</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteStockCountItemAsync(StockCountItem stockCountItem);
    }
}
