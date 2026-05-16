using Nop.Core;
using Nop.Data;
using Nop.Plugin.Misc.CountingSequence.Domain;

namespace Nop.Plugin.Misc.CountingSequence.Services
{
    /// <summary>
    /// Shipment transit service
    /// </summary>
    public class ShipmentTransitService : IShipmentTransitService
    {
        #region Fields

        protected readonly IRepository<ShipmentTransit> _shipmentTransitRepository;
        protected readonly IRepository<ShipmentTransitItems> _shipmentTransitItemsRepository;
        protected readonly IRepository<Pallet> _palletRepository;

        #endregion

        #region Ctor

        public ShipmentTransitService(IRepository<ShipmentTransit> shipmentTransitRepository,
            IRepository<ShipmentTransitItems> shipmentTransitItemsRepository,
            IRepository<ShipmentTransitItems> shipmentTransitItems,
            IRepository<Pallet> palletRepository)
        {
            _shipmentTransitRepository = shipmentTransitRepository;
            _shipmentTransitItemsRepository = shipmentTransitItemsRepository;
            _shipmentTransitItemsRepository = shipmentTransitItems;
            _palletRepository = palletRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Insert the shipment transit 
        /// <param name="shipmentTransit">ShipmentTransit</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task InsertShipmentTransitAsync(ShipmentTransit shipmentTransit)
        {
            await _shipmentTransitRepository.InsertAsync(shipmentTransit);
        }

        /// <summary>
        /// Updates the shipment transit 
        /// </summary>
        /// <param name="shipmentTransit">ShipmentTransit</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task UpdateShipmentTransitAsync(ShipmentTransit shipmentTransit)
        {
            await _shipmentTransitRepository.UpdateAsync(shipmentTransit);
        }

        /// <summary>
        /// Deletes the shipment transit 
        /// </summary>
        /// <param name="shipmentTransit">ShipmentTransit</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task DeleteShipmentTransitAsync(ShipmentTransit shipmentTransit)
        {
            await _shipmentTransitRepository.DeleteAsync(shipmentTransit);
        }

        /// <summary>
        /// Gets a shipment transit by identifier
        /// </summary>
        /// <param name="id">ShipmentTransit identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the shipment transit 
        /// </returns>
        public async Task<ShipmentTransit> GetShipmentTransitByIdAsync(int id)
        {
            return await _shipmentTransitRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Get all shipment transit entries
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">Show Hidden</param>
        /// <param name="fromDisptchId">Disptch identifier</param>
        /// <param name="status">Status</param>
        /// <param name="productId">Product identifier</param>
        /// <param name="palletId">Pallet identifier</param>
        /// <param name="minQuantity">Maximum Month</param>
        /// <param name="maxQuantity">Minimum Year</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the paged list of entity entries
        /// </returns>
        public async Task<IPagedList<ShipmentTransit>> GetShipmentTransitAllAsync(
            bool showHidden = false,
            int fromDisptchId =0,
            int status=0,
            int productId = 0,
            int palletId = 0,
            int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            var query = _shipmentTransitRepository.Table;
            if (status > 0)
            {
                query = query.Where(x => x.Status == status);
            }
            if (fromDisptchId > 0)
                query = query.Where(x => x.FromDispatchId == fromDisptchId);

            if (productId > 0)
            {
                query = from x in query
                        join st in _shipmentTransitItemsRepository.Table
                            on x.Id equals st.ShipmentTransitId
                        where st.ProductId == productId
                        select x;
            }

            if (palletId > 0)
            {
                query = query.Where(x =>
                    _shipmentTransitItemsRepository.Table
                        .Any(st => st.ShipmentTransitId == x.Id && st.PalletId == palletId));
            }

            return await query.ToPagedListAsync(pageIndex, pageSize);
        }

        /// <summary>
        /// Get all shipment transit entries
        /// </summary>
        /// <param name="productId">ProductId</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the paged list of entity entries
        /// </returns>
        public async Task<IPagedList<ShipmentTransitItems>> GetAllShipmentTransitItemsAsync(int productId = 0, int palletId = 0, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _shipmentTransitItemsRepository.Table;

            if (productId > 0)
                query = from sti in query
                        where sti.ProductId == productId
                        select sti;

            if (palletId > 0)
                query = from sti in query
                        where sti.PalletId == palletId
                        select sti;

            return await query.ToPagedListAsync(pageIndex, pageSize);
        }

        #endregion
    }
}
