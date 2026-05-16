using Nop.Core;
using Nop.Plugin.Misc.CountingSequence.Domain;
using Nop.Plugin.Misc.CountingSequence.Models.History;

namespace Nop.Plugin.Misc.CountingSequence.Services
{
    /// <summary>
    /// History service interface
    /// </summary>
    public interface IHistoryService
    {
        /// <summary>
        /// Gets all history
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the categories
        /// </returns>
       // Task<IPagedList<WarehouseStockHistory>> GetAllHistoryAsync(int pageIndex = 0, int pageSize = int.MaxValue);

        Task<IPagedList<HistoryModel>> GetAllHistoryPageAsync(
            int productId = 0,
            int? month = null,
            int? year = null,
            int? unit = null,
            int shipment = 0,
            int pageIndex = 0,
            int pageSize = int.MaxValue);
    }
}
