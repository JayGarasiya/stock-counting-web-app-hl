using Nop.Core;
using Nop.Data;
using Nop.Plugin.Misc.CountingSequence.Domain;

namespace Nop.Plugin.Misc.CountingSequence.Services
{
    /// <summary>
    /// Shipment dispatch service
    /// </summary>
    public class ShipmentDispatchService : IShipmentDispatchService
    {
        #region Fields

        protected readonly IRepository<ShipmentDispatches> _repository;
        protected readonly IRepository<ShipmentTransit> _shipmentTransitRepository;
        protected readonly IRepository<Pallet> _palletRepository;
        protected readonly IRepository<ShipmentTransitItems> _shipmentTransitItemsRepository;
        protected readonly IRepository<PalletStock> _palletStockRepository;

        #endregion

        #region Ctor

        public ShipmentDispatchService(IRepository<ShipmentDispatches> repository,
            IRepository<ShipmentTransit> shipmentTransitRepository,
            IRepository<Pallet> palletRepository,
            IRepository<ShipmentTransitItems> shipmentTransitItemsRepository,
            IRepository<PalletStock> palletStockRepository)
        {
            _repository = repository;
            _shipmentTransitRepository = shipmentTransitRepository;
            _palletRepository = palletRepository;
            _shipmentTransitItemsRepository = shipmentTransitItemsRepository;
            _palletStockRepository = palletStockRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Insert the shipment dispatch 
        /// </summary>
        /// <param name="shipmentdispatch">ShipmentDispatch</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task InsertShipmentDispatchAsync(ShipmentDispatches shipmentdispatch)
        {
            await _repository.InsertAsync(shipmentdispatch);
        }

        /// <summary>
        /// Updates the shipment dispatch 
        /// </summary>
        /// <param name="shipmentdispatch">ShipmentDispatch</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task UpdateShipmentDispatchAsync(ShipmentDispatches shipmentdispatch)
        {
            await _repository.UpdateAsync(shipmentdispatch);
        }

        /// <summary>
        /// Deletes the shipment dispatch 
        /// </summary>
        /// <param name="shipmentdispatch">ShipmentDispatch</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task DeleteShipmentDispatchAsync(ShipmentDispatches shipmentdispatch)
        {
            await _repository.DeleteAsync(shipmentdispatch);
        }

        /// <summary>
        /// Gets a shipment dispatch by identifier
        /// </summary>
        /// <param name="id">ShipmentDispatch identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the shipment dispatch 
        /// </returns>
        public async Task<ShipmentDispatches> GetShipmentDispatchByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        /// <summary>
        /// Get all shipment dispatch entries
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">Show Hidden</param>
        /// <param name="visible">Visible</param>
        /// <param name="name">shipment dispatch name</param>
        /// <param name="shipmentDispatchId">shipment dispatch identifier</param>
        /// <param name="minMonth">Minimum Month</param>
        /// <param name="maxMonth">Maximum Month</param>
        /// <param name="minYear">Minimum Year</param>
        /// <param name="maxYear">Maximum Year</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the paged list of entity entries
        /// </returns>
        public async Task<IPagedList<ShipmentDispatches>> GetAllShipementDispatchAsync(bool showHidden = false,
            bool? visible = null,
            string name = null,
            int shipmentDispatchId= 0,
            int? minMonth = null,
            int? maxMonth = null,
            int? minYear = null,
            int? maxYear = null,
            int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            var query = _repository.Table;

            if (!showHidden)
                query = query.Where(x => x.Visible);
            else if (visible.HasValue)
                query = query.Where(x => x.Visible == visible.Value);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => x.Name.Contains(name));

            if (shipmentDispatchId > 0)
            {
                query = query.Where(x => x.DispatchType == shipmentDispatchId);
            }
            if (minMonth.HasValue)
                query = query.Where(b => b.ShippedMonth >= minMonth);

            if (maxMonth.HasValue)
                query = query.Where(b => b.ShippedMonth <= maxMonth);

            if (minYear.HasValue)
                query = query.Where(b => b.ShippedYear >= minYear);

            if (maxYear.HasValue)
                query = query.Where(b => b.ShippedYear <= maxYear);

            return await query
                .OrderBy(x => x.DisplayOrder)
                .ToPagedListAsync(pageIndex, pageSize);
        }

        /// <summary>
        /// Get Shipment Dispatches Async
        /// </summary>
        /// <param name="warehouseId">warehouse id</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the paged list of entity entries
        /// </returns>
        public async Task<IList<ShipmentDispatches>> GetShipmentDispatchesAsync(int warehouseId = 0)
        {
            //var pallets = _palletRepository.Table;
            //var palletStocks = _palletStockRepository.Table;
            var shipmentDispatches = _repository.Table;
            var shipmentTransits = _shipmentTransitRepository.Table;
            //var shipmentTransitItems = _shipmentTransitItemsRepository.Table;

            var query = from sd in shipmentDispatches
                        //join p in pallets on sd.Id equals p.ShipmentDispatchId

                        // LEFT JOIN palletStocks (to detect missing)
                        //join ps in palletStocks on p.Id equals ps.PalletId into psGroup
                        //from ps in psGroup.DefaultIfEmpty()

                            // INNER JOIN (must exist)
                        //join sti in shipmentTransitItems on p.Id equals sti.PalletId

                        join st in shipmentTransits on sd.Id equals st.FromDispatchId

                        where //p.IsVisible
                              //&& sd.DispatchType == (int)ShipmentDispatchType.USShipment
                              //&&
                              sd.Visible
                              && st.Status != (int)ShipmentTransitStatus.Received

                              // KEY CONDITION
                              //&& ps == null   // stock not counted yet

                        select sd;

            return await query.Distinct().ToListAsync();
        }

        #endregion
    }
}
