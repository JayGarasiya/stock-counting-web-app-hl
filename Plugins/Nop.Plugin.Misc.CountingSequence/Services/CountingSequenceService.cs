using Nop.Core;
using Nop.Data;
using Nop.Plugin.Misc.CountingSequence.Domain;

namespace Nop.Plugin.Misc.CountingSequence.Services
{
    /// <summary>
    /// Counting sequence service
    /// </summary>
    public partial class CountingSequenceService : ICountingSequenceService
    {
        #region Fields

        protected readonly IRepository<StockCount> _stockCountRepository;
        protected readonly IRepository<StockCountItem> _stockCountItemRepository;

        #endregion

        #region Ctor

        public CountingSequenceService(IRepository<StockCount> stockCountRepository,
            IRepository<StockCountItem> stockCountItemRepository)
        {
            _stockCountRepository = stockCountRepository;
            _stockCountItemRepository = stockCountItemRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Inserts a stock count
        /// </summary>
        /// <param name="stockCount">StockCount</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertStockCountAsync(StockCount stockCount)
        {
            await _stockCountRepository.InsertAsync(stockCount);
        }

        /// <summary>
        /// Updates a stock count
        /// </summary>
        /// <param name="stockCount">StockCount</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateStockCountAsync(StockCount stockCount)
        {
            await _stockCountRepository.UpdateAsync(stockCount);
        }

        /// <summary>
        /// Gets a stock Count
        /// </summary>
        /// <param name="stockCountId">StockCount identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the stock Count
        /// </returns>
        public virtual async Task<StockCount> GetStockCountByIdAsync(int stockCountId)
        {
            return await _stockCountRepository.GetByIdAsync(stockCountId, cache => default);
        }

        /// <summary>
        /// Inserts a stock count item
        /// </summary>
        /// <param name="stockCountItem">StockCountItem</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertStockCountItemAsync(StockCountItem stockCountItem)
        {
            await _stockCountItemRepository.InsertAsync(stockCountItem);
        }
        /// <summary>
        /// Updates a stock count item
        /// </summary>
        /// <param name="stockCountItem">StockCountItem</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateStockCountItemAsync(StockCountItem stockCountItem)
        {
            await _stockCountItemRepository.UpdateAsync(stockCountItem);
        }

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
        public async Task<IPagedList<StockCount>> GetAllStockCountAsync(string name = null,
            int warehouseId = 0,
            int countTypeId = 0,
            ProgressStatus? progressStatusId = null,
            DateTime? createdFromUtc = null,
            DateTime? createdToUtc = null,
            int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            var query = _stockCountRepository.Table;

            if (!string.IsNullOrEmpty(name))
                query = query.Where(x => x.Name.Contains(name));

            if (countTypeId > 0)
            {
                if (countTypeId == 1)
                    query = query.Where(i =>
                        _stockCountItemRepository.Table
                            .Any(v => v.StockCountId == i.Id && v.RackId > 0));

                if (countTypeId == 2)
                    query = query.Where(i =>
                        _stockCountItemRepository.Table
                            .Any(v => v.StockCountId == i.Id && v.PalletId > 0));
            }

            if (createdFromUtc.HasValue)
                query = query.Where(o => createdFromUtc.Value.Date <= o.CreatedOnUtc.Date);

            if (createdToUtc.HasValue)
                query = query.Where(o => createdToUtc.Value.Date >= o.CreatedOnUtc.Date);

            if (progressStatusId.HasValue)
                query = query.Where(o => (int)progressStatusId == o.ProgressStatusId);

            return await query.OrderByDescending(x => x.Id).ToPagedListAsync(pageIndex, pageSize);
        }

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
        public async Task<IList<StockCountItem>> GetAllStockCountItem(
            int stockCountId = 0,
            int rackId = 0,
            int palletId = 0,
            int productId = 0,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var query = _stockCountItemRepository.Table;

            if (stockCountId > 0)
                query = query.Where(x => x.StockCountId.Equals(stockCountId));

            if (rackId > 0)
                query = query.Where(x => x.RackId.Equals(rackId));

            if (palletId > 0)
                query = query.Where(x => x.PalletId.Equals(palletId));

            if (productId > 0)
                query = query.Where(x => x.ProductId.Equals(productId));

            if (startDate.HasValue)
                query = query.Where(o => startDate.Value <= o.CreatedOnUtc);

            if (endDate.HasValue)
                query = query.Where(o => endDate.Value >= o.CreatedOnUtc);

            return await query.ToListAsync();
        }

        /// <summary>
        /// Delete the counting
        /// </summary>
        /// <param name="stockCount">Stock Count</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task DeleteStockCountAsync(StockCount stockCount)
        {
            await _stockCountRepository.DeleteAsync(stockCount);
        }

        /// <summary>
        /// Delete the counting items
        /// </summary>
        /// <param name="stockCountItem">Stock Count Item</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task DeleteStockCountItemAsync(StockCountItem stockCountItem)
        {
            await _stockCountItemRepository.DeleteAsync(stockCountItem);
        }

        #endregion
    }
}
