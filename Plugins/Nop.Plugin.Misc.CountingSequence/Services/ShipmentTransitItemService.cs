using Nop.Core;
using Nop.Data;
using Nop.Plugin.Misc.CountingSequence.Domain;

namespace Nop.Plugin.Misc.CountingSequence.Services
{
    /// <summary>
    /// Shipment transit item service 
    /// </summary>
    public class ShipmentTransitItemService : IShipmentTransitItemService
    {
        #region Fields

        protected readonly IRepository<ShipmentTransitItems> _shipmentTransitItemRepository;

        #endregion

        #region Ctor

        public ShipmentTransitItemService(IRepository<ShipmentTransitItems> shipmentTransitItemRepository)
        {
            _shipmentTransitItemRepository = shipmentTransitItemRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Insert the shipment transit item
        /// <param name="items">ShipmentTransitItems</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task InsertShipmentTransitItemsAsync(List<ShipmentTransitItems> items)
        {
            await _shipmentTransitItemRepository.InsertAsync(items);
        }

        /// <summary>
        /// Gets a shipment transit item by identifier
        /// </summary>
        /// <param name="shipmentTransitId">ShipmentTransitItems identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the shipment transit item 
        /// </returns>
        public async Task<IPagedList<ShipmentTransitItems>> GetShipmentTransitItemsByShipmentTransitIdAsync(int shipmentTransitId = 0)
        {
            var query = _shipmentTransitItemRepository.Table;

            if (shipmentTransitId > 0)
                query = query.Where(x => x.ShipmentTransitId == shipmentTransitId);

            return await query.ToPagedListAsync(0, int.MaxValue);
        }

        /// <summary>
        /// Gets a shipment transit item by pallet identifier
        /// </summary>
        /// <param name="palletId">pallet identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the shipment transit items 
        /// </returns>
        public async Task<IList<ShipmentTransitItems>> GetShipmentTransitItemsByPalletIdAsync(int palletId)
        {
            var query = _shipmentTransitItemRepository.Table.Where(x => x.PalletId == palletId);

            return await query.ToListAsync();
        }

        /// <summary>
        /// Gets a shipment transit item by identifier
        /// </summary>
        /// <param name="id">ShipmentTransitItems identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the shipment transit item 
        /// </returns>
        public async Task<ShipmentTransitItems> GetShipmentTransitItemByIdAsync(int id)
        {
            return await _shipmentTransitItemRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Deletes the shipment transit item 
        /// </summary>
        /// <param name="shipmentTransititem">ShipmentTransitItems</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task DeleteShipmentTransitItemAsync(ShipmentTransitItems shipmentTransititem)
        {
            await _shipmentTransitItemRepository.DeleteAsync(shipmentTransititem);
        }

        /// <summary>
        /// Updates the shipment transit item 
        /// </summary>
        /// <param name="shipmentTransititem">ShipmentTransitItems</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task UpdateShipmentTransitItemAsync(ShipmentTransitItems shipmentTransititem)
        {
            await _shipmentTransitItemRepository.UpdateAsync(shipmentTransititem);
        }
        #endregion
    }
}
