using Nop.Plugin.Misc.CountingSequence.Domain;
using Nop.Plugin.Misc.CountingSequence.Models.ShipmentTransit;
using Nop.Plugin.Misc.CountingSequence.Models.ShipmentTransitItem;

namespace Nop.Plugin.Misc.CountingSequence.Factories
{
    /// <summary>
    /// Shipment transit model factory 
    /// </summary>
    public interface IShipmentTransitModelFactory
    {
        /// <summary>
        /// Prepare shipment transit search model
        /// </summary>
        /// <param name="searchModel">Shipment transit search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the shipment transit search model
        /// </returns>
        Task<ShipmentTransitSearchModel> PrepareShipmentTransitSearchModelAsync(ShipmentTransitSearchModel searchModel);

        /// <summary>
        /// Prepare shipment transit list model
        /// </summary>
        /// <param name="searchModel">Shipment transit search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the shipment transit list model
        /// </returns>
        Task<ShipmentTransitListModel> PrepareShipmentTransitListModelAsync(ShipmentTransitSearchModel searchModel);

        /// <summary>
        /// Prepare shipment transit model
        /// </summary>
        /// <param name="model">Shipment transit model</param>
        /// <param name="shipmentTransit">Shipment transit </param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the shipment transit model
        /// </returns>
        Task<ShipmentTransitModel> PrepareShipmentTransitModelAsync(ShipmentTransitModel model, ShipmentTransit shipmentTransit);

        /// <summary>
        /// Prepare shipment transit item list model
        /// </summary>
        /// <param name="searchModel">Shipment transit item search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the shipment transit item list model
        /// </returns>
        Task<ShipmentTransitItemListModel> PrepareShipmentTransitItemListModelAsync(ShipmentTransitItemSearchModel searchModel);

        /// <summary>
        /// Prepare shipment transit pallet list model
        /// </summary>
        /// <param name="searchModel">Shipment transit pallet search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the shipment transit pallet list model
        /// </returns>
        Task<ShipmentTransitPalletListModel> PrepareShipmentTransitPalletListModelAsync(ShipmentTransitPalletSearchModel searchModel);

        /// <summary>
        /// Prepare shipment transit item by pallet list model
        /// </summary>
        /// <param name="searchModel">Shipment transit item by pallet search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the shipment transit item by pallet list model
        /// </returns>
        Task<ShipmentTransitItemListModel> PrepareShipmentTransitItemByPalletListModelAsync(ShipmentTransitItemSearchModel searchModel);
    }
}
