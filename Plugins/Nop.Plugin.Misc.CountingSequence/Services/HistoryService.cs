using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Plugin.Misc.CountingSequence.Domain;
using Nop.Plugin.Misc.CountingSequence.Models.History;

namespace Nop.Plugin.Misc.CountingSequence.Services
{
    /// <summary>
    /// History service
    /// </summary>
    public class HistoryService : IHistoryService
    {
        #region Fields

        protected readonly IRepository<WarehouseStockHistory> _warehouseStockHistoryRepository;
        protected readonly IRepository<ShipmentTransit> _shipmentTransitRepository;
        protected readonly IRepository<ShipmentTransitItems> _shipmentTransitItemsRepository;
        protected readonly IRepository<Product> _productRepository;
        protected readonly IRepository<ShipmentDispatches> _shipmentDispatchesRepository;

        #endregion

        #region Ctor

        public HistoryService(IRepository<WarehouseStockHistory> warehouseStockHistoryRepository,
            IRepository<ShipmentTransit> shipmentTransitRepository,
            IRepository<ShipmentTransitItems> shipmentTransitItemsRepository,
            IRepository<Product> productRepository,
            IRepository<ShipmentDispatches> shipmentDispatchesRepository)
        {
            _warehouseStockHistoryRepository = warehouseStockHistoryRepository;
            _shipmentTransitRepository = shipmentTransitRepository;
            _shipmentTransitItemsRepository = shipmentTransitItemsRepository;
            _productRepository = productRepository;
            _shipmentDispatchesRepository = shipmentDispatchesRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets all history
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the categories
        /// </returns>

        public virtual async Task<IPagedList<HistoryModel>> GetAllHistoryPageAsync(
            int productId = 0,
            int? month = null,
            int? year = null,
            int? unit = null,
            int shipment = 0,
            int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            var query =
                from sti in _shipmentTransitItemsRepository.Table
                join st in _shipmentTransitRepository.Table
                    on sti.ShipmentTransitId equals st.Id
                join fd in _shipmentDispatchesRepository.Table
                    on st.FromDispatchId equals fd.Id into fdGroup
                from fromDispatch in fdGroup.DefaultIfEmpty()
                join td in _shipmentDispatchesRepository.Table
                    on sti.ToDispatchId equals td.Id into tdGroup
                from toDispatch in tdGroup.DefaultIfEmpty()
                select new
                {
                    ShipmentTransitItem = sti,
                    ShipmentTransit = st,
                    FromDispatch = fromDispatch,
                    ToDispatch = toDispatch
                };

            if (productId > 0)
                query = query.Where(x => x.ShipmentTransitItem.ProductId == productId);

            if (unit.HasValue)
                query = query.Where(x => x.ShipmentTransitItem.Quantity == unit);

            if (month.HasValue)
                query = query.Where(x =>
                    (x.FromDispatch != null && x.FromDispatch.ShippedMonth == month));

            if (year.HasValue)
                query = query.Where(x =>
                    (x.FromDispatch != null && x.FromDispatch.ShippedYear == year));

            if (shipment == 1)
                query = query.Where(x => x.ShipmentTransit.FromDispatchId > 0);
            else if (shipment == 2)
                query = query.Where(x => x.ShipmentTransitItem.ToDispatchId > 0);

            var historyQuery = query.Select(x => new HistoryModel
            {
                ProductId = x.ShipmentTransitItem.ProductId,
                NumberOfUnits = x.ShipmentTransitItem.Quantity,
                Month = x.FromDispatch != null ? x.FromDispatch.ShippedMonth : (x.ToDispatch != null ? x.ToDispatch.ShippedMonth : 0),
                Year = x.FromDispatch != null ? x.FromDispatch.ShippedYear : (x.ToDispatch != null ? x.ToDispatch.ShippedYear : 0),
                USShipmentNumber = x.ShipmentTransit.FromDispatchId,
                HLShipmentNumber = x.ShipmentTransitItem.ToDispatchId
            });

            historyQuery = historyQuery
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month);

            return await historyQuery.ToPagedListAsync(pageIndex, pageSize);
        }

        #endregion
    }
}
