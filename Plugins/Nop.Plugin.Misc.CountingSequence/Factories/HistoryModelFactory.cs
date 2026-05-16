using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Plugin.Misc.CountingSequence.Domain;
using Nop.Plugin.Misc.CountingSequence.Models.History;
using Nop.Plugin.Misc.CountingSequence.Services;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Shipping;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Misc.CountingSequence.Factories
{
    /// <summary>
    /// History model factory 
    /// </summary>
    public class HistoryModelFactory : IHistoryModelFactory
    {
        #region Fields

        protected readonly IHistoryService _historyService;
        protected readonly IProductService _productService;
        protected readonly IWarehouseService _warehouseService;
        protected readonly IShipmentTransitItemService _shipmentTransitItemService;
        protected readonly IShipmentDispatchService _shipmentDispatchService;
        protected readonly IShipmentTransitService _shipmentTransitService;
        protected readonly ILocalizationService _localizationService;


        #endregion

        #region Ctor

        public HistoryModelFactory(
            IHistoryService historyService,
            IProductService productService,
            IWarehouseService warehouseService,
            IShipmentTransitItemService shipmentTransitItemService,
            IShipmentDispatchService shipmentDispatchService,
            IShipmentTransitService shipmentTransitService,
            ILocalizationService localizationService)
        {
            _historyService = historyService;
            _productService = productService;
            _warehouseService = warehouseService;
            _shipmentTransitItemService = shipmentTransitItemService;
            _shipmentDispatchService = shipmentDispatchService;
            _shipmentTransitService = shipmentTransitService;
            _localizationService = localizationService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare history search model
        /// </summary>
        /// <param name="searchModel">History search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the history search model
        /// </returns>
        public async Task<HistorySearchModel> PrepareHistorySearchModelAsync(HistorySearchModel searchModel)
        {
            //Us 
            searchModel.AvailableShipment.Add(new SelectListItem
            {
                Value = "0",
                Text = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Search.All")
            });
            searchModel.AvailableShipment.Add(new SelectListItem
            {
                Value = "1",
                Text = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.SearchHistory.UsOnly")
            });
            searchModel.AvailableShipment.Add(new SelectListItem
            {
                Value = "2",
                Text = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.SearchHistory.H&LOnly")
            });

            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare history list model
        /// </summary>
        /// <param name="searchModel">History search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the history list model
        /// </returns>
        public async Task<HistoryListModel> PrepareHistoryListModelAsync(HistorySearchModel searchModel)
        {
            var history = await _historyService.GetAllHistoryPageAsync(
                productId: searchModel.SearchProductId,
                month: searchModel.SearchMonth,
                year: searchModel.SearchYear,
                unit: searchModel.SearchOfUnit,
                shipment: searchModel.SearchShipment,
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize);

            var dispatches = await _shipmentDispatchService
                .GetAllShipementDispatchAsync(pageSize: int.MaxValue);

            var model = await new HistoryListModel().PrepareToGridAsync(searchModel, history, () =>
            {
                return history.SelectAwait(async historyItem =>
                {
                    var product = historyItem.ProductId > 0
                        ? await _productService.GetProductByIdAsync(historyItem.ProductId)
                        : null;

                    var fromDispatch = dispatches
                        .FirstOrDefault(x => x.Id == historyItem.USShipmentNumber);
                    var toDispatch = dispatches
                        .FirstOrDefault(x => x.Id == historyItem.HLShipmentNumber);

                    return new HistoryModel
                    {
                        Id = historyItem.Id,
                        // Product
                        ProductId = historyItem.ProductId,
                        ProductName = product?.Name,
                        // Quantity
                        QuantityAdjustment = historyItem.NumberOfUnits,
                        StockQuantity = 0,
                        StockDate = DateTime.Now,
                        // Month/Year (already resolved in the query)
                        Month = historyItem.Month,
                        Year = historyItem.Year,
                        Type = "-",
                        // US / HL names
                        USShipmentName = !string.IsNullOrEmpty(fromDispatch?.Name) ? fromDispatch.Name : "-",
                        HLShipmentName = !string.IsNullOrEmpty(toDispatch?.Name) ? toDispatch.Name : "-"
                    };
                });
            });

            return model;
        }

        #endregion
    }
}