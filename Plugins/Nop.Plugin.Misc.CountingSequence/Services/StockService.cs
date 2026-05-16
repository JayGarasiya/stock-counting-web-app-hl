using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Plugin.Misc.CountingSequence.Domain;

namespace Nop.Plugin.Misc.CountingSequence.Services
{
    /// <summary>
    /// Represents a stock service 
    /// </summary>
    public class StockService : IStockService
    {
        #region Fields

        protected readonly IRepository<RackStock> _rackStockRepository;
        protected readonly IRepository<PalletStock> _palletStockRepository;
        protected readonly IRepository<Product> _productRepository;
        protected readonly IRepository<Pallet> _palletRepository;
        protected readonly IRepository<Rack> _rackRepository;
        protected readonly IRepository<WarehouseStockHistory> _warehouseStockHistoryRepository;

        #endregion

        #region Ctor

        public StockService(
            IRepository<RackStock> rackStockRepository,
            IRepository<PalletStock> palletStockRepository,
            IRepository<Product> productRepository,
            IRepository<Pallet> palletRepository,
            IRepository<Rack> rackRepository,
            IRepository<WarehouseStockHistory> warehouseStockHistoryRepository)
        {
            _rackStockRepository = rackStockRepository;
            _palletStockRepository = palletStockRepository;
            _productRepository = productRepository;
            _palletRepository = palletRepository;
            _rackRepository = rackRepository;
            _warehouseStockHistoryRepository = warehouseStockHistoryRepository;
        }

        #endregion

        #region Methods

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
        public async Task<IPagedList<PalletStock>> GetPalletStocksAsync(int productId = 0,
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
            int pageSize = int.MaxValue)
        {
            var query = _palletStockRepository.Table;

            if (productId > 0)
            {
                query = query.Where(x => x.ProductId == productId);
            }

            if (palletId > 0)
            {
                query = query.Where(x => x.PalletId == palletId);
            }

            if (warehouseId > 0)
            {
                query = query.Where(x => x.WarehouseId == warehouseId);
            }

            if (shipmentTransitId > 0)
            {
                query = query.Where(x => x.ShipmentTransitId == shipmentTransitId);
            }

            if (unitMin.HasValue)
                query = query.Where(x => x.NoOfUnit >= unitMin.Value);

            if (unitMax.HasValue)
                query = query.Where(x => x.NoOfUnit <= unitMax.Value);

            if (packMin.HasValue)
                query = query.Where(x => x.NoOfPack >= packMin.Value);

            if (packMax.HasValue)
                query = query.Where(x => x.NoOfPack <= packMax.Value);

            return await query.OrderBy(x => x.Id).ToPagedListAsync(pageIndex, pageSize);
        }

        //add new 

        /// <summary>
        /// Gets a pallet stock  by identifier
        /// </summary>
        /// <param name="id">Pallet stock identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the pallet
        /// </returns>
        public async Task<PalletStock> GetPalletStockByIdAsync(int id)
        {
            return await _palletStockRepository.GetByIdAsync(id);
        }


        /// <summary>
        /// Gets a rack stock  by identifier
        /// </summary>
        /// <param name="id">rack stock identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rack stock
        /// </returns>
        public async Task<RackStock> GetRackStockByIdAsync(int id)
        {
            return await _rackStockRepository.GetByIdAsync(id);
        }

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
        /// </returns>>
        public async Task<IPagedList<RackStock>> GetRackStocksAsync(int productId = 0,
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
            int pageSize = int.MaxValue)
        {
            var query = _rackStockRepository.Table;

            if (productId > 0)
            {
                query = query.Where(x => x.ProductId == productId);
            }

            if (rackId > 0)
            {
                query = query.Where(x => x.RackId == rackId);
            }

            if (warehouseId > 0)
            {
                query = query.Where(x => x.WarehouseId == warehouseId);
            }

            if (rackLevelTypeId > 0)
            {
                query = query.Where(x => x.LevelId == rackLevelTypeId);
            }

            if (productPositionId > 0)
            {
                query = query.Where(x => x.ProductPositionId == productPositionId);
            }

            if (unitMin.HasValue)
                query = query.Where(x => x.NoOfUnit >= unitMin.Value);

            if (unitMax.HasValue)
                query = query.Where(x => x.NoOfUnit <= unitMax.Value);

            if (packMin.HasValue)
                query = query.Where(x => x.NoOfPack >= packMin.Value);

            if (packMax.HasValue)
                query = query.Where(x => x.NoOfPack <= packMax.Value);

            return await query.OrderBy(x => x.Id).ToPagedListAsync(pageIndex, pageSize);
        }

        /// <summary>
        /// Inserts a rack stock 
        /// </summary>
        /// <param name="rackStock">Rack stock</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task InsertRackStockAsync(RackStock rackStock)
        {
            await _rackStockRepository.InsertAsync(rackStock);
        }

        public async Task UpdateRackStockAsync(RackStock rackStock)
        {
            await _rackStockRepository.UpdateAsync(rackStock);
        }

        /// <summary>
        /// Inserts a pallet stock
        /// </summary>
        /// <param name="palletStock">Pallet stock</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task InsertPalletStockAsync(PalletStock palletStock)
        {
            await _palletStockRepository.InsertAsync(palletStock);
        }

        public async Task UpdatePalletStockAsync(PalletStock palletStock)
        {
            await _palletStockRepository.UpdateAsync(palletStock);
        }

        /// <summary>
        /// Deletes the rack stock
        /// </summary>
        /// <param name="rackStock">Rack stock</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task DeleteRackStock(RackStock rackStock)
        {
            await _rackStockRepository.DeleteAsync(rackStock);
        }

        /// <summary>
        /// Deletes the pallet stock
        /// </summary>
        /// <param name="palletStock">Pallet stock</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task DeletePalletStock(PalletStock palletStock)
        {
            await _palletStockRepository.DeleteAsync(palletStock);
        }

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
        public async Task<IPagedList<WarehouseStockHistory>> GetAllWarehouseStockHistoryAsync(
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
            int pageSize = int.MaxValue)
        {
            var query = _warehouseStockHistoryRepository.Table;

            if (productId > 0)
            {
                query = query.Where(wsh => wsh.ProductId == productId);
            }

            if (warehouseId > 0)
            {
                query = query.Where(wsh => wsh.WarehouseId == warehouseId);
            }

            if (stockCountId > 0)
            {
                query = query.Where(wsh => wsh.StockCountId == stockCountId);
            }

            if (palletId > 0)
            {
                query = from wsh in query
                        join ps in _palletRepository.Table
                        on wsh.PalletId equals ps.Id
                        where ps.Id == palletId
                        select wsh;
            }

            if (rackId > 0)
            {
                query = from wsh in query
                        join rs in _rackRepository.Table
                        on wsh.RackId equals rs.Id
                        where rs.Id == rackId
                        select wsh;
            }

            //unit
            if (unitMin.HasValue)
            {
                query =
                    from wsh in query

                    join rs in _rackStockRepository.Table
                        on wsh.RackId equals rs.Id into rsGroup
                    from rs in rsGroup.DefaultIfEmpty()

                    join ps in _palletStockRepository.Table
                        on wsh.PalletId equals ps.Id into psGroup
                    from ps in psGroup.DefaultIfEmpty()

                    where (rs != null && rs.NoOfUnit >= unitMin.Value)
                       || (ps != null && ps.NoOfUnit >= unitMin.Value)

                    select wsh;
            }

            if (unitMax.HasValue)
            {
                query =
                    from wsh in query

                    join rs in _rackStockRepository.Table
                        on wsh.RackId equals rs.Id into rsGroup
                    from rs in rsGroup.DefaultIfEmpty()

                    join ps in _palletStockRepository.Table
                        on wsh.PalletId equals ps.Id into psGroup
                    from ps in psGroup.DefaultIfEmpty()

                    where (rs != null && rs.NoOfUnit <= unitMax.Value)
                       || (ps != null && ps.NoOfUnit <= unitMax.Value)

                    select wsh;
            }

            //max
            if (packMin.HasValue)
            {
                query =
                    from wsh in query

                    join rs in _rackStockRepository.Table
                        on wsh.RackId equals rs.Id into rsGroup
                    from rs in rsGroup.DefaultIfEmpty()

                    join ps in _palletStockRepository.Table
                        on wsh.PalletId equals ps.Id into psGroup
                    from ps in psGroup.DefaultIfEmpty()

                    where (rs != null && rs.NoOfPack >= packMin.Value)
                       || (ps != null && ps.NoOfPack >= packMin.Value)

                    select wsh;
            }
            if (packMax.HasValue)
            {
                query =
                    from wsh in query

                    join rs in _rackStockRepository.Table
                        on wsh.RackId equals rs.Id into rsGroup
                    from rs in rsGroup.DefaultIfEmpty()

                    join ps in _palletStockRepository.Table
                        on wsh.PalletId equals ps.Id into psGroup
                    from ps in psGroup.DefaultIfEmpty()

                    where (rs != null && rs.NoOfPack <= packMax.Value)
                       || (ps != null && ps.NoOfPack <= packMax.Value)

                    select wsh;
            }

            if (createdFrom.HasValue)
            {
                query = query.Where(wsh => wsh.StockDate >= createdFrom);
            }

            if (createdTo.HasValue)
            {
                query = query.Where(wsh => wsh.StockDate <= createdTo);
            }

            return await query.ToPagedListAsync(pageIndex, pageSize);
        }

        /// <summary>
        /// Inserts a warehouse stock history
        /// </summary>
        /// <param name="warehouseStockHistory">Warehouse stock history</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task InsertWarehouseStockHistoryAsync(WarehouseStockHistory warehouseStockHistory)
        {
            await _warehouseStockHistoryRepository.InsertAsync(warehouseStockHistory);
        }

        #endregion
    }
}
