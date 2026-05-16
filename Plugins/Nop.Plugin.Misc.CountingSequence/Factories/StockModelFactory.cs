using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Plugin.Misc.CountingSequence.Domain;
using Nop.Plugin.Misc.CountingSequence.Models.Stock;
using Nop.Plugin.Misc.CountingSequence.Services;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Shipping;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Misc.CountingSequence.Factories
{
    /// <summary>
    /// Represents a stock model factory
    /// </summary>
    public class StockModelFactory : IStockModelFactory
    {
        #region Fields

        protected readonly IStockService _stockService;
        protected readonly IProductService _productService;
        protected readonly IRackService _rackService;
        protected readonly IPalletService _palletService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IWarehouseService _warehouseService;

        #endregion

        #region Ctor

        public StockModelFactory(
            IStockService stockService,
            IProductService productService,
            IRackService rackService,
            IPalletService palletService,
            ILocalizationService localizationService,
            IWarehouseService warehouseService)
        {
            _stockService = stockService;
            _productService = productService;
            _rackService = rackService;
            _palletService = palletService;
            _localizationService = localizationService;
            _warehouseService = warehouseService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare stock search model
        /// </summary>
        /// <param name="searchModel">Stock search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the stock search model
        /// </returns>
        public async Task<StockSearchModel> PrepareStockSearchModelAsync(StockSearchModel searchModel)
        {
            searchModel ??= new StockSearchModel();

            searchModel.AvailableProducts = (await _productService.SearchProductsAsync()).Select(product => new SelectListItem
            {
                Text = product.Name,
                Value = product.Id.ToString(),
            }).ToList();

            searchModel.AvailableRacks = (await _rackService.GetAllRackPagedAsync())
                .Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Name
                }).ToList();

            searchModel.AvailablePallets = (await _palletService.GetAllPalletPagedAsync())
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList();

            searchModel.SetGridPageSize();
            return searchModel;
        }

        /// <summary>
        /// Prepare paged stock list model
        /// </summary>
        /// <param name="searchModel">Stock search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the stock list model
        /// </returns>
        public async Task<StockListModel> PrepareStockListModelAsync(StockSearchModel searchModel)
        {
            var warehouseStockHistory = await _stockService.GetAllWarehouseStockHistoryAsync(
                productId: searchModel.SearchProductId,
                rackId: searchModel.SearchRackId,
                palletId: searchModel.SearchPalletId,
                unitMax: searchModel.NoOfUnitMax,
                unitMin: searchModel.NoOfUnitMin,
                packMax: searchModel.NoOfPackMax,
                packMin: searchModel.NoOfPackMin,
                pageIndex:searchModel.Page -1,
                pageSize: searchModel.PageSize);

            var model = await new StockListModel().PrepareToGridAsync(searchModel, warehouseStockHistory, () =>
            {
                return warehouseStockHistory.SelectAwait(async item =>
                {
                    return new StockModel
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        ProductName = (await _productService.GetProductByIdAsync(item.ProductId))?.Name ?? "-",
                        RackName = item.PalletId == 0
                        ? (await _rackService.GetRackByIdAsync((await _stockService.GetRackStockByIdAsync(item.RackId))?.RackId ?? 0))?.Name ?? "-"
                        : "-",

                        PalletName = item.PalletId > 0
                        ? (await _palletService.GetPalletByIdAsync((await _stockService.GetPalletStockByIdAsync(item.PalletId))?.PalletId ?? 0))?.Name ?? "-"
                        : "-",

                        WarehouseName = (await _warehouseService.GetWarehouseByIdAsync(item.ProductId))?.Name ?? "-",
                        LevelTypeName = "-",
                        NoOfPack = item.PalletId > 0
                        ? (await _stockService.GetPalletStockByIdAsync(item.PalletId))?.NoOfPack ?? 0
                        : (await _stockService.GetRackStockByIdAsync(item.RackId))?.NoOfPack ?? 0,

                        NoOfUnit = item.PalletId > 0
                        ? (await _stockService.GetPalletStockByIdAsync(item.PalletId))?.NoOfUnit ?? 0
                        : (await _stockService.GetRackStockByIdAsync(item.RackId))?.NoOfUnit ?? 0,

                        BegQuantity = (await _productService.GetAllProductWarehouseInventoryRecordsAsync(item.ProductId))?
                        .Where(x => x.WarehouseId == item.WarehouseId)?.FirstOrDefault()?.StockQuantity ?? 0,

                        Quantity = item.QuantityAdjustment
                    };
                });
            });

            return model;
        }
        #endregion
    }
}