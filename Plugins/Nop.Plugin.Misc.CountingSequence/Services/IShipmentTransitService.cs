using Nop.Core;
using Nop.Plugin.Misc.CountingSequence.Domain;

namespace Nop.Plugin.Misc.CountingSequence.Services
{
    /// <summary>
    /// Shipment transit service interface
    /// </summary>
    public interface IShipmentTransitService
    {
        /// <summary>
        /// Insert the shipment transit 
        /// <param name="shipmentTransit">ShipmentTransit</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertShipmentTransitAsync(ShipmentTransit shipmentTransit);

        /// <summary>
        /// Updates the shipment transit 
        /// </summary>
        /// <param name="shipmentTransit">ShipmentTransit</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateShipmentTransitAsync(ShipmentTransit shipmentTransit);

        /// <summary>
        /// Deletes the shipment transit 
        /// </summary>
        /// <param name="shipmentTransit">ShipmentTransit</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteShipmentTransitAsync(ShipmentTransit shipmentTransit);

        /// <summary>
        /// Gets a shipment transit by identifier
        /// </summary>
        /// <param name="id">ShipmentTransit identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the shipment transit 
        /// </returns>
        Task<ShipmentTransit> GetShipmentTransitByIdAsync(int id);

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
        Task<IPagedList<ShipmentTransit>> GetShipmentTransitAllAsync(bool showHidden = false,
            int fromDisptchId = 0,
            int status = 0,
            int productId = 0,
            int palletId = 0,
            int pageIndex = 0,
            int pageSize = int.MaxValue);

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
        Task<IPagedList<ShipmentTransitItems>> GetAllShipmentTransitItemsAsync(int productId = 0, int palletId = 0, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}