using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Misc.CountingSequence.Domain;
using Nop.Plugin.Misc.CountingSequence.Models.ShipmentDispatch;
using Nop.Plugin.Misc.CountingSequence.Services;
using Nop.Services.Localization;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Misc.CountingSequence.Factories
{
    /// <summary>
    /// Shipment dispatch model factory 
    /// </summary>
    public class ShipmentDispatchModelFactory : IShipmentDispatchModelFactory
    {
        #region Fields

        protected readonly IShipmentDispatchService _shipmentDispatchService;
        protected readonly ILocalizationService _localizationService;

        #endregion

        #region Ctor

        public ShipmentDispatchModelFactory(
            IShipmentDispatchService shipmentDispatchService,
            ILocalizationService localizationService)
        {
            _shipmentDispatchService = shipmentDispatchService;
            _localizationService = localizationService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare shipment dispatch search model
        /// </summary>
        /// <param name="searchModel">Shipment dispatch search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the shipment dispatch search model
        /// </returns>
        public async Task<ShipmentDispatchSearchModel> PrepareShipmentDispatchSearchModelAsync(ShipmentDispatchSearchModel model)
        {
            model ??= new ShipmentDispatchSearchModel();

            model.AvailableDispatchType = new List<SelectListItem>();
            model.AvailableVisibleType.Add(new SelectListItem
            {
                Value = "0",
                Text = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Search.All")
            });
            model.AvailableVisibleType.Add(new SelectListItem
            {
                Value = "1",
                Text = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Search.VisibleOnly")
            });
            model.AvailableVisibleType.Add(new SelectListItem
            {
                Value = "2",
                Text = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Search.UnvisibleOnly")
            });
            model.AvailableDispatchType.Add(new SelectListItem
            {
                Text = "All",
                Value = "0",
                Selected = model.SearchDispatchTypeId == 0
            });

            foreach (ShipmentDispatchType item in Enum.GetValues(typeof(ShipmentDispatchType)))
            {
                model.AvailableDispatchType.Add(new SelectListItem
                {
                    Text = await _localizationService.GetLocalizedEnumAsync(item),
                    Value = ((int)item).ToString(),
                    Selected = (int)item == model.SearchDispatchTypeId
                });
            }

            model.SetGridPageSize();
            return model;
        }

        /// <summary>
        /// Prepare shipment dispatch list model
        /// </summary>
        /// <param name="searchModel">Shipment dispatch search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the shipment dispatch list model
        /// </returns>
        public async Task<ShipmentDispatchListModel> PrepareShipmentDispatchListModelAsync(ShipmentDispatchSearchModel searchModel)
        {
            var entities = await _shipmentDispatchService.GetAllShipementDispatchAsync(
                showHidden : true,
                visible: searchModel.SearchVisible == 0 ? null : (bool?)(searchModel.SearchVisible == 1),
                name: searchModel.SearchName,
                shipmentDispatchId:searchModel.SearchDispatchTypeId,
                minMonth: searchModel.SearchMinMonth,
                maxMonth: searchModel.SearchMaxMonth,
                minYear: searchModel.SearchMinYear,
                maxYear: searchModel.SearchMaxYear,
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize);

            var model = await new ShipmentDispatchListModel().PrepareToGridAsync(searchModel, entities, () =>
                {
                    return entities.SelectAwait(async shipmentDispatch =>
                    {
                        var enumValue = (ShipmentDispatchType)shipmentDispatch.DispatchType;

                        return new ShipmentDispatchModel
                        {
                            Id = shipmentDispatch.Id,
                            Name = shipmentDispatch.Name,
                            DispatchType = shipmentDispatch.DispatchType,
                            DispatchTypeName = enumValue.ToString(),
                            ShippedMonth = shipmentDispatch.ShippedMonth,
                            ShippedYear = shipmentDispatch.ShippedYear,
                            DisplayOrder = shipmentDispatch.DisplayOrder,
                            Visible = shipmentDispatch.Visible
                        };
                    });
                });

            return model;
        }

        /// <summary>
        /// Prepare shipment dispatch model
        /// </summary>
        /// <param name="model">Shipment dispatch model</param>
        /// <param name="shipmentDispatch">Shipment dispatch entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the shipment dispatch model
        /// </returns>
        public async Task<ShipmentDispatchModel> PrepareShipmentDispatchModelAsync(ShipmentDispatchModel model, ShipmentDispatches shipmentDispatch)
        {
            if (shipmentDispatch != null)
            {
                model ??= new ShipmentDispatchModel();

                model.Id = shipmentDispatch.Id;
                model.Name = shipmentDispatch.Name;
                model.DispatchType = shipmentDispatch.DispatchType;
                model.ShippedMonth = shipmentDispatch.ShippedMonth;
                model.ShippedYear = shipmentDispatch.ShippedYear;
                model.DisplayOrder = shipmentDispatch.DisplayOrder;
                model.Visible = shipmentDispatch.Visible;
            }

            model.AvailableDispatchTypes = await Enum.GetValues(typeof(ShipmentDispatchType))
                .Cast<ShipmentDispatchType>()
                .SelectAwait(async x => new SelectListItem
                {
                    Value = ((int)x).ToString(),
                    Text = await _localizationService.GetLocalizedEnumAsync(x)
                }).ToListAsync();

            return model;
        }

        #endregion
    }
}