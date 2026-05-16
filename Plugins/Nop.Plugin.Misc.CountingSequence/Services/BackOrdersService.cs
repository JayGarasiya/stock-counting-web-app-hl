using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Plugin.Misc.CountingSequence.Domain;

namespace Nop.Plugin.Misc.CountingSequence.Services
{
    /// <summary>
    /// Back order service
    /// </summary>
    public class BackOrdersService : IBackOrdersService
    {
        #region Fields

        protected readonly IRepository<BackOrders> _backOrderRepository;
        protected readonly IRepository<Channel> _chennelRepository;
        protected readonly IRepository<Product> _productRepository;

        #endregion

        #region Ctor

        public BackOrdersService(IRepository<BackOrders> backOrderRepository, 
            IRepository<Channel> chennelRepository, 
            IRepository<Product> productRepository)
        {
            _backOrderRepository = backOrderRepository;
            _chennelRepository = chennelRepository;
            _productRepository = productRepository;
        }

        #endregion

        #region Methods

        #region Back Orders

        /// <summary>
        /// Gets a back order by identifier
        /// </summary>
        /// <param name="id">Back order identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the back orders
        /// </returns>
        public async Task<BackOrders> GetBackOrderByIdAsync(int id)
        {
            return await _backOrderRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Inserts a back orders
        /// </summary>
        /// <param name="backOrders">Back orders</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task InsertBackOrdersAsync(BackOrders backOrders)
        {
            await _backOrderRepository.InsertAsync(backOrders);
        }

        /// <summary>
        /// Updates the back orders
        /// </summary>
        /// <param name="backOrders">Back orders</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task UpdateBackOrdersAsync(BackOrders backOrders)
        {
            await _backOrderRepository.UpdateAsync(backOrders);
        }

        /// <summary>
        /// Delete the back orders
        /// </summary>
        /// <param name="backOrders">Back orders</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task DeleteBackOrdersAsync(BackOrders backOrders)
        {
            await _backOrderRepository.DeleteAsync(backOrders);
        }

        /// <summary>
        /// Get all back orders
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="referenceNumber">reference number</param>
        /// <param name="searchStatus">Status</param>
        /// <param name="productId">Product</param>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <param name="minQuantity">Minimum  quantity</param>
        /// <param name="maxQuantity">Maximum quantity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the paged list of entity entries
        /// </returns>
        public async Task<IPagedList<BackOrders>> GetAllBackOrdersPagedAsync(
            string referenceNumber = null,
            int channelId = 0,
            int searchStatus = 0,
            int productId = 0,
            DateTime? startDate = null,
            DateTime? endDate = null,
            int? minQuantity = null,
            int? maxQuantity = null,
            int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            var query = _backOrderRepository.Table;
            if (!string.IsNullOrWhiteSpace(referenceNumber))
                query = query.Where(x => x.ReferenceNo.Contains(referenceNumber));

            if (channelId > 0)
                query = query.Where(x => x.ChannelId == channelId);

            if (searchStatus > 0)
            {
                query = query.Where(x => x.Status == searchStatus);
            }
            if (productId > 0)
            {
                query = query.Where(x => x.ProductId == productId);
            }

            if (startDate.HasValue)
                query = query.Where(o => startDate.Value <= o.OrderDate);

            if (endDate.HasValue)
                query = query.Where(o => endDate.Value > o.OrderDate);

            if (minQuantity.HasValue)
                query = query.Where(b => b.Quantity >= minQuantity);

            if (maxQuantity.HasValue)
                query = query.Where(b => b.Quantity <= maxQuantity);

            return await query.ToPagedListAsync(pageIndex, pageSize);
        }

        #endregion

        #endregion
    }
}
