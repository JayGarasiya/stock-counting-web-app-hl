using Nop.Core;
using Nop.Plugin.Misc.CountingSequence.Domain;

namespace Nop.Plugin.Misc.CountingSequence.Services
{
    /// <summary>
    /// Shipment dispatch service interface 
    /// </summary>
    public interface IShipmentDispatchService
    {
        /// <summary>
        /// Insert the shipment dispatch 
        /// <param name="shipmentdispatch">ShipmentDispatch</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertShipmentDispatchAsync(ShipmentDispatches shipmentdispatch);

        /// <summary>
        /// Updates the shipment dispatch 
        /// </summary>
        /// <param name="shipmentdispatch">ShipmentDispatch</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateShipmentDispatchAsync(ShipmentDispatches shipmentdispatch);

        /// <summary>
        /// Deletes the shipment dispatch 
        /// </summary>
        /// <param name="shipmentdispatch">ShipmentDispatch</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteShipmentDispatchAsync(ShipmentDispatches shipmentdispatch);

        /// <summary>
        /// Gets a shipment dispatch by identifier
        /// </summary>
        /// <param name="id">ShipmentDispatch identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the shipment dispatch 
        /// </returns>
        Task<ShipmentDispatches> GetShipmentDispatchByIdAsync(int id);

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
        Task<IPagedList<ShipmentDispatches>> GetAllShipementDispatchAsync(bool showHidden = false,
            bool? visible = null,
            string name = null,
            int shipmentDispatchId = 0,
             int? minMonth = null,
            int? maxMonth = null,
            int? minYear = null,
            int? maxYear = null,
            int pageIndex = 0,
            int pageSize = int.MaxValue);

        /// <summary>
        /// Get Shipment Dispatches Async
        /// </summary>
        /// <param name="warehouseId">warehouse id</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the paged list of entity entries
        /// </returns>
        Task<IList<ShipmentDispatches>> GetShipmentDispatchesAsync(int warehouseId = 0);
    }
}
