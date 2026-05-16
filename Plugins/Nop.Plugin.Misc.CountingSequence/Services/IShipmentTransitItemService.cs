using Nop.Core;
using Nop.Plugin.Misc.CountingSequence.Domain;

namespace Nop.Plugin.Misc.CountingSequence.Services
{
    /// <summary>
    /// Shipment transit item service interface
    /// </summary>
    public interface IShipmentTransitItemService
    {
        /// <summary>
        /// Insert the shipment transit item
        /// <param name="items">ShipmentTransitItems</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertShipmentTransitItemsAsync(List<ShipmentTransitItems> items);

        /// <summary>
        /// Gets a shipment transit item by identifier
        /// </summary>
        /// <param name="shipmentTransitId">ShipmentTransitItems identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the shipment transit item 
        /// </returns>
        Task<IPagedList<ShipmentTransitItems>> GetShipmentTransitItemsByShipmentTransitIdAsync(int shipmentTransitId = 0);

        /// <summary>
        /// Gets a shipment transit item by pallet identifier
        /// </summary>
        /// <param name="palletId">pallet identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the shipment transit items 
        /// </returns>
        Task<IList<ShipmentTransitItems>> GetShipmentTransitItemsByPalletIdAsync(int palletId);


        /// <summary>
        /// Gets a shipment transit item by identifier
        /// </summary>
        /// <param name="id">ShipmentTransitItems identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the shipment transit item 
        /// </returns>
        Task<ShipmentTransitItems> GetShipmentTransitItemByIdAsync(int id);

        /// <summary>
        /// Deletes the shipment transit item 
        /// </summary>
        /// <param name="shipmentTransititem">ShipmentTransitItems</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteShipmentTransitItemAsync(ShipmentTransitItems shipmentTransititem);

        /// <summary>
        /// Updates the shipment transit item 
        /// </summary>
        /// <param name="shipmentTransititem">ShipmentTransitItems</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateShipmentTransitItemAsync(ShipmentTransitItems shipmentTransititem);
    }
}
