using Nop.Core;
using Nop.Plugin.Misc.CountingSequence.Domain;

namespace Nop.Plugin.Misc.CountingSequence.Services
{
    /// <summary>
    /// Back order service interface
    /// </summary>
    public interface IBackOrdersService
    {
        /// <summary>
        /// Gets a back order by identifier
        /// </summary>
        /// <param name="id">Back order identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the back orders
        /// </returns>
        Task<BackOrders> GetBackOrderByIdAsync(int id);

        /// <summary>
        /// Inserts a back orders
        /// </summary>
        /// <param name="backOrders">Back orders</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertBackOrdersAsync(BackOrders backOrders);

        /// <summary>
        /// Updates the back orders
        /// </summary>
        /// <param name="backOrders">Back orders</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateBackOrdersAsync(BackOrders backOrders);

        /// <summary>
        /// Deletes the back orders
        /// </summary>
        /// <param name="backOrders">Back orders</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteBackOrdersAsync(BackOrders backOrders);

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
        Task<IPagedList<BackOrders>> GetAllBackOrdersPagedAsync(string referenceNumber = null,
            int channelId = 0,
            int searchStatus = 0,
            int productId = 0,
            DateTime? startDate = null,
            DateTime? endDate = null,
            int? minQuantity = null,
            int? maxQuantity = null,
            int pageIndex = 0,
            int pageSize = int.MaxValue);
    }
}
