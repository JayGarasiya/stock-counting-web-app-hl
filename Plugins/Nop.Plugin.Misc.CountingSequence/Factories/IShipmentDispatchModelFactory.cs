using Nop.Plugin.Misc.CountingSequence.Domain;
using Nop.Plugin.Misc.CountingSequence.Models.ShipmentDispatch;

namespace Nop.Plugin.Misc.CountingSequence.Factories
{
    /// <summary>
    /// Shipment dispatch model factory interface
    /// </summary>
    public interface IShipmentDispatchModelFactory
    {
        /// <summary>
        /// Prepare shipment dispatch search model
        /// </summary>
        /// <param name="searchModel">Shipment dispatch search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the shipment dispatch search model
        /// </returns>
        Task<ShipmentDispatchSearchModel> PrepareShipmentDispatchSearchModelAsync(ShipmentDispatchSearchModel searchModel);

        /// <summary>
        /// Prepare shipment dispatch list model
        /// </summary>
        /// <param name="searchModel">Shipment dispatch search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the shipment dispatch list model
        /// </returns>
        Task<ShipmentDispatchListModel> PrepareShipmentDispatchListModelAsync(ShipmentDispatchSearchModel searchModel);

        /// <summary>
        /// Prepare shipment dispatch model
        /// </summary>
        /// <param name="model">Shipment dispatch model</param>
        /// <param name="shipmentDispatch">Shipment dispatch entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the shipment dispatch model
        /// </returns>
        Task<ShipmentDispatchModel> PrepareShipmentDispatchModelAsync(ShipmentDispatchModel model, ShipmentDispatches shipmentDispatch);
    }
}
