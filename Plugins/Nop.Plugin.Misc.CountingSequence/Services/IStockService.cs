using Nop.Core;
using Nop.Plugin.Misc.CountingSequence.Domain;

namespace Nop.Plugin.Misc.CountingSequence.Services
{
    /// <summary>
    /// Represents a stock service interface
    /// </summary>
    public interface IStockService
    {
        /// <summary>
        /// Get all rack stock
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="rackId">Rack identifier</param>
        /// <param name="warehouseId">Warehouse identifier</param>
        /// <param name="minValue">Minimum value</param>
        /// <param name="maxValue">Maximum value</param>
        /// <param name="unitMin">Minimum unit</param>
        /// <param name="unitMax">Maximum unit</param>
        /// <param name="packMin">Minimum pack</param>
        /// <param name="packMax">Maximum pack</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the paged list of entity entries
        /// </returns>
        Task<IPagedList<RackStock>> GetRackStocksAsync(int productId = 0,
            int rackId = 0,
            int warehouseId = 0,
            int rackLevelTypeId = 0,
            int productPositionId = 0,
            int? minValue = null,
            int? maxValue = null,
            int? unitMin = null,
            int? unitMax = null,
            int? packMin = null,
            int? packMax = null,
            int pageIndex = 0,
            int pageSize = int.MaxValue);

        /// <summary>
        /// Get all pallet stock
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="palletId">Pallet identifier</param>
        /// <param name="warehouseId">Warehouse identifier</param>
        /// <param name="minValue">Minimum value</param>
        /// <param name="maxValue">Maximum value</param>
        /// <param name="unitMin">Minimum unit</param>
        /// <param name="unitMax">Maximum unit</param>
        /// <param name="packMin">Minimum pack</param>
        /// <param name="packMax">Maximum pack</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the paged list of entity entries
        /// </returns>
        Task<IPagedList<PalletStock>> GetPalletStocksAsync(int productId = 0,
            int palletId = 0,
            int warehouseId = 0,
            int shipmentTransitId = 0,
            int? minValue = null,
            int? maxValue = null,
            int? unitMin = null,
            int? unitMax = null,
            int? packMin = null,
            int? packMax = null,
            int pageIndex = 0,
            int pageSize = int.MaxValue);

        /// <summary>
        /// Gets a pallet stock  by identifier
        /// </summary>
        /// <param name="id">Pallet stock identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the pallet
        /// </returns>
        Task<PalletStock> GetPalletStockByIdAsync(int id);

        /// <summary>
        /// Gets a rack stock  by identifier
        /// </summary>
        /// <param name="id">rack stock identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rack stock
        /// </returns>
        Task<RackStock> GetRackStockByIdAsync(int id);
        /// <summary>
        /// Inserts a rack stock 
        /// </summary>
        /// <param name="rackStock">Rack stock</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertRackStockAsync(RackStock rackStock);

        Task UpdateRackStockAsync(RackStock rackStock);

        /// <summary>
        /// Delete the rack stock
        /// </summary>
        /// <param name="rackStock">Rack stock</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteRackStock(RackStock rackStock);

        /// <summary>
        /// Inserts a pallet stock
        /// </summary>
        /// <param name="palletStock">Pallet stock</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertPalletStockAsync(PalletStock palletStock);

        Task UpdatePalletStockAsync(PalletStock palletStock);

        /// <summary>
        /// Delete the pallet stock
        /// </summary>
        /// <param name="palletStock">Pallet stock</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeletePalletStock(PalletStock palletStock);

        /// <summary>
        /// Inserts a warehouse stock history
        /// </summary>
        /// <param name="warehouseStockHistory">Warehouse stock history</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertWarehouseStockHistoryAsync(WarehouseStockHistory warehouseStockHistory);

        /// <summary>
        /// Get all warehouse stock history
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="warehouseId">Warehouse identifier</param>
        /// <param name="createdFrom">Created from</param>
        /// <param name="createdTo">Created to</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the paged list of entity entries
        /// </returns>
        Task<IPagedList<WarehouseStockHistory>> GetAllWarehouseStockHistoryAsync(
            int productId = 0,
            int warehouseId = 0,
            int rackId = 0,
            int palletId = 0,
            int stockCountId = 0,
            int? minValue = null,
            int? maxValue = null,
            int? unitMin = null,
            int? unitMax = null,
            int? packMin = null,
            int? packMax = null,
            DateTime? createdFrom = null,
            DateTime? createdTo = null,
            int pageIndex = 0,
            int pageSize = int.MaxValue);
    }
}
