using DocumentFormat.OpenXml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Misc.CountingSequence.Domain;
using Nop.Plugin.Misc.CountingSequence.Factories;
using Nop.Plugin.Misc.CountingSequence.Infrastructure;
using Nop.Plugin.Misc.CountingSequence.Models;
using Nop.Plugin.Misc.CountingSequence.Models.CountingSequence;
using Nop.Plugin.Misc.CountingSequence.Models.Pallet;
using Nop.Plugin.Misc.CountingSequence.Models.ShipmentDispatch;
using Nop.Plugin.Misc.CountingSequence.Services;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Shipping;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Factories;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.CountingSequence.Controllers
{
    public class CountingSequenceController : BaseAdminController
    {
        #region Fields

        protected readonly ISettingService _settingService;
        protected readonly IStoreContext _storeContext;
        protected readonly INotificationService _notificationService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IWarehouseService _warehouseService;
        protected readonly IWorkContext _workContext;
        protected readonly ICountingSequenceService _countingSequenceService;
        protected readonly IProductService _productService;
        protected readonly IProductModelFactory _productModelFactory;
        protected readonly IProductAttributeService _productAttributeService;
        protected readonly ICountingSequenceModelFactory _countingSequenceModelFactory;
        protected readonly ICategoryService _categoryService;
        protected readonly IRackService _rackService;
        protected readonly IShipmentTransitService _shipmentTransitService;
        protected readonly IShipmentTransitItemService _shipmentTransitItemService;
        protected readonly IShipmentDispatchService _shipmentDispatchService;
        protected readonly IPalletService _palletService;
        protected readonly IStockService _stockService;
        protected readonly IBackOrdersService _backOrdersService;
        protected readonly IChannelService _channelService;
        protected readonly CountingSequenceSettings _countingSequenceSettings;

        #endregion

        #region Ctor

        public CountingSequenceController(
            ISettingService settingService,
            IStoreContext storeContext,
            INotificationService notificationService,
            ILocalizationService localizationService,
            IWarehouseService warehouseService,
            IWorkContext workContext,
            ICountingSequenceService countingSequenceService,
            IProductService productService,
            IProductModelFactory productModelFactory,
            IProductAttributeService productAttributeService,
            ICountingSequenceModelFactory countingSequenceModelFactory,
            ICategoryService categoryService,
            IRackService rackService,
            IShipmentTransitService shipmentTransitService,
            IShipmentDispatchService shipmentDispatchService,
            IPalletService palletService,
            IStockService stockService,
            IShipmentTransitItemService shipmentTransitItemService,
            IBackOrdersService backOrdersService,
            IChannelService channelService,
            CountingSequenceSettings countingSequenceSettings)
        {
            _settingService = settingService;
            _storeContext = storeContext;
            _notificationService = notificationService;
            _localizationService = localizationService;
            _warehouseService = warehouseService;
            _workContext = workContext;
            _countingSequenceService = countingSequenceService;
            _productService = productService;
            _productModelFactory = productModelFactory;
            _productAttributeService = productAttributeService;
            _countingSequenceModelFactory = countingSequenceModelFactory;
            _categoryService = categoryService;
            _rackService = rackService;
            _shipmentTransitService = shipmentTransitService;
            _shipmentDispatchService = shipmentDispatchService;
            _palletService = palletService;
            _stockService = stockService;
            _shipmentTransitItemService = shipmentTransitItemService;
            _backOrdersService = backOrdersService;
            _channelService = channelService;
            _countingSequenceSettings = countingSequenceSettings;
        }

        #endregion

        #region Methods

        #region Configuration

        [CheckPermission(StandardPermission.Configuration.MANAGE_PLUGINS)]
        public async Task<IActionResult> Configure()
        {
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var settings = await _settingService.LoadSettingAsync<CountingSequenceSettings>(storeScope);

            var model = new ConfigurationModel
            {
                Enabled = settings.Enabled,
                SaleTypeOrder = settings.SaleTypeOrder,
                PalletCountingOrder = (int)settings.PalletCountingOrder,
                Level = (int)settings.Level,
                Position = (int)settings.Position,
                ActiveStoreScopeConfiguration = storeScope
            };

            model.AvailableLevels = (await Enum.GetValues(typeof(SequenceLevel))
                .Cast<SequenceLevel>()
                .SelectAwait(async x => new SelectListItem
                {
                    Value = ((int)x).ToString(),
                    Text = await _localizationService.GetLocalizedEnumAsync(x)
                }).ToListAsync());

            model.AvailablePositions = (await Enum.GetValues(typeof(SequencePositions))
                .Cast<SequencePositions>()
                .SelectAwait(async x => new SelectListItem
                {
                    Value = ((int)x).ToString(),
                    Text = await _localizationService.GetLocalizedEnumAsync(x)
                }).ToListAsync());

            model.AvailablePalletCountingOrder = (await Enum.GetValues(typeof(PalletCounting))
                .Cast<PalletCounting>()
                .SelectAwait(async x => new SelectListItem
                {
                    Value = ((int)x).ToString(),
                    Text = await _localizationService.GetLocalizedEnumAsync(x)
                }).ToListAsync());

            var channels = await _channelService.GetAllChannelPagedAsync();

            model.AvailableSaleOrder = channels.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = c.Id == model.SaleTypeOrder
            }).ToList();

            if (storeScope > 0)
            {
                model.Enabled_OverrideForStore = await _settingService.SettingExistsAsync(settings, x => x.Enabled, storeScope);
                model.SaleTypeOrder_OverrideForStore = await _settingService.SettingExistsAsync(settings, x => x.SaleTypeOrder, storeScope);
                model.PalletCountingOrder_OverrideForStore = await _settingService.SettingExistsAsync(settings, x => x.PalletCountingOrder, storeScope);
                model.Level_OverrideForStore = await _settingService.SettingExistsAsync(settings, x => x.Level, storeScope);
                model.Position_OverrideForStore = await _settingService.SettingExistsAsync(settings, x => x.Position, storeScope);
            }

            return View("~/Plugins/Misc.CountingSequence/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [CheckPermission(StandardPermission.Configuration.MANAGE_PLUGINS)]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var settings = await _settingService.LoadSettingAsync<CountingSequenceSettings>(storeScope);

            settings.Enabled = model.Enabled;
            settings.SaleTypeOrder = model.SaleTypeOrder;
            settings.PalletCountingOrder = (PalletCounting)model.PalletCountingOrder;
            settings.Level = (SequenceLevel)model.Level;
            settings.Position = (SequencePositions)model.Position;

            await _settingService.SaveSettingOverridablePerStoreAsync(settings, x => x.Enabled, model.Enabled_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(settings, x => x.SaleTypeOrder, model.SaleTypeOrder_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(settings, X => X.PalletCountingOrder, model.PalletCountingOrder_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(settings, x => x.Level, model.Level_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(settings, x => x.Position, model.Position_OverrideForStore, storeScope, false);
            await _settingService.ClearCacheAsync();

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return await Configure();
        }

        #endregion

        #region Counting sequence

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> List()
        {
            var model = await _countingSequenceModelFactory.PrepareCountingSequenceSearchModelAsync(new StockCountSearchModel());
            return View("~/Plugins/Misc.CountingSequence/Views/CountingSequence/List.cshtml", model);
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> List(StockCountSearchModel searchModel)
        {
            var model = await _countingSequenceModelFactory.PrepareCountingSequenceListModelAsync(searchModel);
            return Json(model);
        }

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> WarehousesPopup()
        {
            var warehouses = await _warehouseService.GetAllWarehousesAsync();

            return PartialView("~/Plugins/Misc.CountingSequence/Views/CountingSequence/_WarehousesPopup.cshtml", warehouses);
        }

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> WarehousesPopupSearch(string searchName)
        {
            var warehouses = await _warehouseService.GetAllWarehousesAsync();

            if (!string.IsNullOrWhiteSpace(searchName))
            {
                warehouses = warehouses
                    .Where(x => x.Name.Contains(searchName, StringComparison.InvariantCultureIgnoreCase))
                    .ToList();
            }

            return Json(warehouses);
        }

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> StartCount(int warehouseId)
        {
            var warehouse = await _warehouseService.GetWarehouseByIdAsync(warehouseId);
            if (warehouse == null)
                return Json(new { success = false, error = "Invalid warehouse" });

            var stockCounts = (await _countingSequenceService.GetAllStockCountAsync(warehouseId: warehouseId)).ToList();
            var lastIncompleteStockCount = stockCounts?.Where(x => x.ProgressStatusId != (int)ProgressStatus.Completed)?
                .OrderByDescending(x => x.Id).FirstOrDefault();

            if (lastIncompleteStockCount != null)
            {
                return Json(new
                {
                    success = false,
                    warehouseId = warehouseId,
                    isPallet = true,
                    redirectUrl = ""
                });
            }
            else
            {
                var lastCompleteStockCount = stockCounts?.Where(x => x.ProgressStatusId == (int)ProgressStatus.Completed)?
                    .OrderByDescending(x => x.Id).FirstOrDefault();
                if (lastCompleteStockCount != null)
                {
                    var items = await _countingSequenceService.GetAllStockCountItem(stockCountId: lastCompleteStockCount.Id);

                    bool hasItems = items?.Any() == true;
                    bool hasPalletItems = hasItems && items.Any(i => i.PalletId > 0);

                    if (!hasItems || hasPalletItems)
                    {
                        var stockCount = new StockCount
                        {
                            Name = $"Count - {DateTime.UtcNow.ToString("MM/dd/yyyy")}",
                            CutsomerId = (await _workContext.GetCurrentCustomerAsync()).Id,
                            WarehouseId = warehouseId,
                            ProgressStatusId = (int)ProgressStatus.Pending,
                            CreatedOnUtc = DateTime.UtcNow
                        };

                        await _countingSequenceService.InsertStockCountAsync(stockCount);

                        return Json(new
                        {
                            success = true,
                            isPallet = false,
                            redirectUrl = Url.Action(nameof(CountingSequenceRacksProductDetails), new
                            {
                                stockCountId = stockCount.Id
                            })
                        });
                    }
                }
            }

            // NEW COUNT
            return Json(new
            {
                success = true,
                warehouseId = warehouseId,
                isPallet = true,
                redirectUrl = ""
            });
        }

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> Resume(int id)
        {
            var stockCountId = id;
            var stockCount = await _countingSequenceService.GetStockCountByIdAsync(stockCountId);
            var items = await _countingSequenceService.GetAllStockCountItem(stockCountId: stockCountId);
            if (items.Any() && items.Any(x => x.RackId > 0))
            {
                if (stockCount.ProgressStatusId == (int)ProgressStatus.Completed)
                {
                    return RedirectToAction(nameof(LoadRackSummary), new
                    {
                        stockCountId,
                        isSummaryMode = true
                    });
                }
                var racks = (await _rackService.GetAllRackPagedAsync()).OrderBy(x => x.SequenceOrder).ToList();

                int rackIndex = 0;
                int productIndex = 0;

                var lastItem = items.OrderByDescending(x => x.Id).First();

                // Find rack index
                rackIndex = racks.FindIndex(p => p.Id == lastItem.RackId);
                if (rackIndex < 0)
                    rackIndex = 0;

                // Get ordered products of that rack
                var rackProducts = await _rackService.GetProductByRackIdAsync(lastItem.RackId);
                var productIds = rackProducts.Select(x => x.ProductId).ToList();

                var products = (await _productService.SearchProductsAsync())
                    .Where(p => productIds.Contains(p.Id))
                    .OrderBy(p => productIds.IndexOf(p.Id))
                    .ToList();

                // Find exact product index
                productIndex = products.FindIndex(p => p.Id == lastItem.ProductId);

                if (productIndex < 0)
                    productIndex = 0;

                return RedirectToAction(nameof(CountingSequenceRacksProductDetails), new
                {
                    stockCountId,
                    rackIndex,
                    productIndex
                });
            }
            else if (items.Any() && items.Any(x => x.PalletId > 0))
            {
                if (stockCount.ProgressStatusId == (int)ProgressStatus.Completed)
                {
                    return RedirectToAction(nameof(LoadPalletSummary), new
                    {
                        stockCountId,
                        isSummaryMode = true
                    });
                }

                var pallets = (await _palletService.GetAllPalletPagedAsync()).OrderBy(x => x.SequenceOrder).ToList();

                int palletIndex = 0;
                int productIndex = 0;

                var lastItem = items.OrderByDescending(x => x.Id).First();

                // Find pallet index
                palletIndex = pallets.FindIndex(p => p.Id == lastItem.PalletId);
                if (palletIndex < 0)
                    palletIndex = 0;

                // Get ordered products of that pallet
                var palletProducts = await _palletService.GetProductByPalletIdAsync(lastItem.PalletId);
                var productIds = palletProducts.Select(x => x.ProductId).ToList();

                var products = (await _productService.SearchProductsAsync())
                    .Where(p => productIds.Contains(p.Id))
                    .OrderBy(p => productIds.IndexOf(p.Id))
                    .ToList();

                // Find exact product index
                productIndex = products.FindIndex(p => p.Id == lastItem.ProductId);

                if (productIndex < 0)
                    productIndex = 0;

                return RedirectToAction(nameof(CountingSequencePalletsProductDetails), new
                {
                    stockCountId,
                    palletIndex,
                    productIndex
                });
            }
            else
            {
                var lastItem = (await _countingSequenceService.GetAllStockCountItem()).LastOrDefault();
                if (lastItem != null && lastItem.PalletId > 0)
                    return RedirectToAction(nameof(CountingSequenceRacksProductDetails), new
                    {
                        stockCountId
                    });
                else
                    return RedirectToAction(nameof(CountingSequencePalletsProductDetails), new
                    {
                        stockCountId
                    });
            }
        }

        #endregion

        #region Rack Counting sequence

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> CountingSequenceRacksProductDetails(int stockCountId, int rackIndex = 0, int productIndex = 0)
        {
            var stockCount = await _countingSequenceService.GetStockCountByIdAsync(stockCountId);
            var racks = (await _rackService.GetAllRackPagedAsync()).OrderBy(x => x.SequenceOrder).ToList();

            if (racks.Count == 0)
                throw new ArgumentException("No racks found.");

            if (rackIndex < 0)
                rackIndex = 0;

            if (rackIndex >= racks.Count)
                rackIndex = racks.Count - 1;

            var currentRack = racks[rackIndex];

            var rackProducts = await _rackService.GetProductByRackIdAsync(currentRack.Id);

            var productIds = rackProducts.Select(pp => pp.ProductId).ToList();

            var products = (await _productService.GetProductsByIdsAsync(productIds.ToArray()))
                .OrderBy(p => productIds.IndexOf(p.Id)).ToList();

            // Skip empty rack
            if (!products.Any())
            {
                // check if any rack has products
                bool hasAnyProduct = false;

                foreach (var rack in racks)
                {
                    var rackItems = await _rackService.GetProductByRackIdAsync(rack.Id);

                    if (rackItems.Any())
                    {
                        hasAnyProduct = true;
                        break;
                    }
                }

                if (!hasAnyProduct)
                    throw new ArgumentException("No products found in any rack.");

                return RedirectToAction(nameof(CountingSequenceRacksProductDetails), new
                {
                    stockCountId,
                    rackIndex = rackIndex + 1,
                    productIndex = 0
                });
            }

            if (productIndex < 0)
            {
                rackIndex--;

                if (rackIndex >= 0)
                {
                    var prevRack = racks[rackIndex];

                    var prevRackProducts = await _rackService.GetProductByRackIdAsync(prevRack.Id);

                    var prevProductIds = prevRackProducts.Select(x => x.ProductId).ToList();

                    var prevProducts = (await _productService.GetProductsByIdsAsync(prevProductIds.ToArray()))
                        .OrderBy(p => prevProductIds.IndexOf(p.Id)).ToList();

                    productIndex = prevProducts.Count - 1;

                    products = prevProducts;
                    currentRack = prevRack;
                }
                else
                {
                    rackIndex = 0;
                    productIndex = 0;
                }
            }

            var product = products[productIndex];

            var shipmentTransitItems = await _shipmentTransitService.GetAllShipmentTransitItemsAsync(product.Id);

            // Group by shipment
            var groupedItems = shipmentTransitItems
                .GroupBy(x => x.ShipmentTransitId)
                .Select(g => new
                {
                    ShipmentTransitId = g.Key,
                    TotalQuantity = g.Sum(x => x.Quantity)
                });

            var shipmentList = new List<(ProductShipmentModel Model, DateTime Date, int Status)>();

            foreach (var group in groupedItems)
            {
                var transit = await _shipmentTransitService.GetShipmentTransitByIdAsync(group.ShipmentTransitId);
                if (transit == null)
                    continue;

                var dispatch = await _shipmentDispatchService.GetShipmentDispatchByIdAsync(transit.FromDispatchId);
                if (dispatch == null || dispatch.DispatchType != (int)ShipmentDispatchType.USShipment)
                    continue;

                var date = new DateTime(dispatch.ShippedYear, dispatch.ShippedMonth, 1);

                var productShipmentModel = new ProductShipmentModel
                {
                    ShipmentName = dispatch.Name,
                    ShipmentDispatchesDate = date.ToString("MMM-yyyy"),
                    ShipmentTransitItemsQuantity = group.TotalQuantity
                };

                shipmentList.Add((productShipmentModel, date, transit.Status));
            }

            // Sort by latest
            var sorted = shipmentList.OrderByDescending(x => x.Date).ToList();

            // Separate after sorting
            var recentShipments = sorted?.Where(x => x.Status == (int)ShipmentTransitStatus.Received)?
                .Take(6)?.Select(x => x.Model)?.ToList() ?? new List<ProductShipmentModel>();
            var incomingShipments = sorted?.Where(x => x.Status != (int)ShipmentTransitStatus.Received)?
                .Take(2)?.Select(x => x.Model)?.ToList() ?? new List<ProductShipmentModel>();

            var rackProduct = (await _rackService.GetAllRackProductPageAsync(rackId: currentRack.Id, productId: product.Id))?.FirstOrDefault();
            var model = new CountingSequenceRackProductDetail
            {
                StockCountModel =
                {
                    Id = stockCount.Id,
                    Name = stockCount.Name,
                    WarehouseName = (await _warehouseService.GetWarehouseByIdAsync(stockCount.WarehouseId))?.Name ?? "-",
                    ProgressStatusName = await _localizationService.GetLocalizedEnumAsync(stockCount.ProgressStatus),
                    CountType = "Rack",
                    CreatedOnUtc = stockCount.CreatedOnUtc.ToString("MM/dd/yyyy"),
                },
                SequenceLevelType = await _localizationService.GetLocalizedEnumAsync(_countingSequenceSettings.Level),
                SequencePositionType = await _localizationService.GetLocalizedEnumAsync(_countingSequenceSettings.Position),
                ProductPositionLevel =
                    (await _rackService.GetRackLevelTypeByIdAsync(
                        (await _rackService.GetAllRackProductPageAsync(rackId: currentRack.Id, productId: product.Id)).FirstOrDefault().RackLevelId)).Name,
                ProductDetailsModel = await _productModelFactory.PrepareProductDetailsModelAsync(product, null, false),
                QuantityPerPack = product.OrderMaximumQuantity,

                CategoryName = (await _categoryService
                .GetCategoryByIdAsync((await _categoryService.GetProductCategoriesByProductIdAsync(product.Id)).FirstOrDefault()?.CategoryId ?? 0))?.Name,

                StockCountId = stockCountId,
                CurrentIndex = productIndex,
                RackIndex = rackIndex,
                CurrentRackId = currentRack.Id,
                TotalCount = products.Count,
                TotalRacks = racks.Count,

                AvailableProductRacks = (await _rackService.GetRackByProductIdAsync(product.Id)).Where(x => x.Id != currentRack.Id).ToList(),
                AvailableRacks = racks,
                RecentShipments = recentShipments,
                IncomingShipments = incomingShipments,
            };

            var existingProduct = (await _countingSequenceService.GetAllStockCountItem(productId: product.Id)).ToList();
            var existingProductStockOnRack = existingProduct.Where(x => x.StockCountId == stockCountId && x.RackId == currentRack.Id).FirstOrDefault();
            if (existingProductStockOnRack != null)
            {
                var countSheetModel = CommonHelper.To<List<CountSheetModel>>(existingProductStockOnRack.CountSheetXml);
                model.CountSheetRows = countSheetModel
                .Select(x => new RackCountSheetModel
                {
                    Cases = x.Cases,
                    Units = x.Units,
                }).ToList();
                model.TotalCountingQuantity = existingProductStockOnRack.Quantity;
            }
            else
            {
                model.CountSheetRows.Add(new RackCountSheetModel
                {
                    Cases = 0,
                    Units = 0
                });
            }

            var rackStock = await _stockService.GetRackStocksAsync(productId: product.Id, rackId: currentRack.Id, warehouseId: stockCount.WarehouseId);
            model.TotalCountingQuantityLastMonth = 0;// rackStock?
            //    .Where(x => x.StockDate.Month == DateTime.UtcNow.AddMonths(-1).Month)?.Sum(x => x.Quantity) ?? 0;

            model.TotalProductQuantity = existingProduct?.Where(x => x.StockCountId == stockCountId)?.Sum(x => x.Quantity) ?? 0;

            var lastMonthwarehouseStock = await _stockService.GetAllWarehouseStockHistoryAsync(productId: product.Id, warehouseId: stockCount.WarehouseId,
              createdFrom: new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).AddMonths(-1),
              createdTo: new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).AddTicks(-1));
            model.TotalProductQuantityLastMonth = lastMonthwarehouseStock?.Sum(x => x.StockQuantity) ?? 0;

            //sales
            var begInv = (await _productService.GetAllProductWarehouseInventoryRecordsAsync(product.Id) ?? Enumerable.Empty<ProductWarehouseInventory>())
                        .Where(x => x.WarehouseId == stockCount.WarehouseId)
                        .Sum(x => (int?)x.StockQuantity) ?? 0;

            var shipmentDispatchIds = (await _shipmentDispatchService.GetAllShipementDispatchAsync(
                    shipmentDispatchId: (int)ShipmentDispatchType.USShipment,
                    minMonth: DateTime.UtcNow.Month,
                    maxMonth: DateTime.UtcNow.Month,
                    minYear: DateTime.UtcNow.Year,
                    maxYear: DateTime.UtcNow.Year)
                ?? Enumerable.Empty<ShipmentDispatches>())
                .Select(x => x.Id).ToList();

            var shipmentTransitIds = (await _shipmentTransitService.GetShipmentTransitAllAsync()
                ?? Enumerable.Empty<ShipmentTransit>())
                .Where(x => shipmentDispatchIds.Contains(x.FromDispatchId))
                .Select(x => x.Id).ToList();

            var recieved = (shipmentTransitItems ?? Enumerable.Empty<ShipmentTransitItems>())
                .Where(x => shipmentTransitIds.Contains(x.ShipmentTransitId))
                .Sum(x => (int?)x.Quantity) ?? 0;

            var counted = model?.TotalProductQuantity ?? 0;

            var backOrders = (await _backOrdersService.GetAllBackOrdersPagedAsync(
                    productId: product.Id,
                    startDate: new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1),
                    endDate: DateTime.UtcNow)
                ?? Enumerable.Empty<BackOrders>())
                .Sum(x => (int?)x.Quantity) ?? 0;

            model.TotalSaleThisMonth = begInv + recieved - counted + backOrders;

            var startDate = DateTime.UtcNow.AddMonths(-6);
            var endDate = DateTime.UtcNow;

            // Shipment Dispatch (last 6 months)
            var shipmentDispatchIds6 = (await _shipmentDispatchService.GetAllShipementDispatchAsync(
                    shipmentDispatchId: (int)ShipmentDispatchType.USShipment,
                    minMonth: startDate.Month,
                    maxMonth: endDate.Month,
                    minYear: startDate.Year,
                    maxYear: endDate.Year)
                ?? Enumerable.Empty<ShipmentDispatches>())
                .Select(x => x.Id)
                .ToList();

            // Shipment Transit
            var shipmentTransitIds6 = (await _shipmentTransitService.GetShipmentTransitAllAsync()
                    ?? Enumerable.Empty<ShipmentTransit>())
                .Where(x => shipmentDispatchIds.Contains(x.FromDispatchId) && x.Status == (int)ShipmentTransitStatus.Received)
                .Select(x => x.Id)
                .ToList();

            // Received Quantity
            var received6 = (shipmentTransitItems ?? Enumerable.Empty<ShipmentTransitItems>())
                .Where(x => shipmentTransitIds.Contains(x.ShipmentTransitId))
                .Sum(x => (int?)x.Quantity) ?? 0;

            // Counted
            var counted6 = model?.TotalProductQuantity ?? 0;

            // BackOrders (last 6 months)
            var backOrders6 = (await _backOrdersService.GetAllBackOrdersPagedAsync(
                    productId: product.Id,
                    startDate: startDate,
                    endDate: endDate)
                ?? Enumerable.Empty<BackOrders>())
                .Sum(x => (int?)x.Quantity) ?? 0;

            // Final Total
            model.TotalSaleThisLastSixMonth = begInv + received6 - counted6 + backOrders6;

            return View("~/Plugins/Misc.CountingSequence/Views/CountingSequence/CountingSequenceRackProductDetails.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> LoadRackCountingSequenceStatistics(int rackId, int productId)
        {
            var result = new List<object>();

            var nowUtc = DateTime.UtcNow;
            var currentMonthStart = new DateTime(nowUtc.Year, nowUtc.Month, 1);
            var startMonth = currentMonthStart.AddMonths(-5);

            for (int i = 0; i < 6; i++)
            {
                var from = startMonth.AddMonths(i);
                var to = from.AddMonths(1);

                // Stock count
                var qty = (await _countingSequenceService.GetAllStockCountItem(
                                productId: productId,
                                startDate: from,
                                endDate: to)
                           ?? Enumerable.Empty<StockCountItem>())
                    .Sum(x => (int?)x.Quantity) ?? 0;

                var begInv = (await _productService.GetAllProductWarehouseInventoryRecordsAsync(productId) ?? Enumerable.Empty<ProductWarehouseInventory>())
                        //.Where(x => x.WarehouseId == stockCount.WarehouseId)
                        .Sum(x => (int?)x.StockQuantity) ?? 0;

                // BackOrders
                var backOrdersQty = (await _backOrdersService.GetAllBackOrdersPagedAsync(
                                        productId: productId,
                                        startDate: from,
                                        endDate: to)
                                     ?? Enumerable.Empty<BackOrders>())
                    .Sum(x => (int?)x.Quantity) ?? 0;

                var shipmentDispatchIds = (await _shipmentDispatchService.GetAllShipementDispatchAsync(
                        shipmentDispatchId: (int)ShipmentDispatchType.USShipment,
                        minMonth: from.Month,
                        maxMonth: from.Month,
                        minYear: from.Year,
                        maxYear: from.Year)
                    ?? Enumerable.Empty<ShipmentDispatches>())
                    .Select(x => x.Id)
                    .ToList();

                var shipmentTransitIds = (await _shipmentTransitService.GetShipmentTransitAllAsync()
                        ?? Enumerable.Empty<ShipmentTransit>())
                    .Where(x => shipmentDispatchIds.Contains(x.FromDispatchId) && x.Status == (int)ShipmentTransitStatus.Received)
                    .Select(x => x.Id)
                    .ToList();

                var shipmentTransitItems = await _shipmentTransitService.GetAllShipmentTransitItemsAsync(productId);

                var received = (shipmentTransitItems ?? Enumerable.Empty<ShipmentTransitItems>())
                    .Where(x => shipmentTransitIds.Contains(x.ShipmentTransitId))
                    .Sum(x => (int?)x.Quantity) ?? 0;

                var counted = qty;

                var sales = begInv + received - counted + backOrdersQty;

                // Final output
                result.Add(new SaleStatistics
                {
                    Month = from.ToString("MMM yyyy"),
                    Count = qty,
                    Sales = sales
                });
            }

            return Json(result);
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> SaveRackCountSheet(CountingSequenceRackProductDetail model)
        {
            var countSheetItems = model.CountSheetRows
                .Select(x => new CountSheetModel
                {
                    Cases = x.Cases,
                    Units = x.Units
                }).ToList();

            var countSheetItemsXML = CommonHelper.To<string>(countSheetItems);

            var racks = (await _rackService.GetAllRackPagedAsync()).OrderBy(x => x.SequenceOrder).ToList();

            var currentRack = racks[model.RackIndex];

            var existingStockCountItem = (await _countingSequenceService
                .GetAllStockCountItem(stockCountId: model.StockCountId, rackId: currentRack.Id, productId: model.ProductDetailsModel.Id))
                .FirstOrDefault();

            if (existingStockCountItem == null)
            {
                var stockCountItem = new StockCountItem
                {
                    RackId = currentRack.Id,
                    StockCountId = model.StockCountId,
                    ProductId = model.ProductDetailsModel.Id,
                    CountSheetXml = countSheetItemsXML,
                    CreatedOnUtc = DateTime.UtcNow,
                    Quantity = model.TotalCountingQuantity
                };

                await _countingSequenceService.InsertStockCountItemAsync(stockCountItem);
            }
            else
            {
                existingStockCountItem.Quantity = model.TotalCountingQuantity;
                existingStockCountItem.CountSheetXml = countSheetItemsXML;
                await _countingSequenceService.UpdateStockCountItemAsync(existingStockCountItem);
            }

            var rackProducts = await _rackService.GetProductByRackIdAsync(currentRack.Id);

            var productIds = rackProducts.Select(pp => pp.ProductId).ToList();

            var products = (await _productService.GetProductsByIdsAsync(productIds.ToArray()))
                .OrderBy(p => productIds.IndexOf(p.Id)).ToList();

            int nextProductIndex = model.CurrentIndex + 1;
            int nextRackIndex = model.RackIndex;

            // move to next rack if current finished
            if (nextProductIndex >= products.Count)
            {
                nextRackIndex++;
                nextProductIndex = 0;

                // skip empty racks
                while (nextRackIndex < racks.Count)
                {
                    var nextRack = racks[nextRackIndex];
                    var nextRackProducts = await _rackService.GetProductByRackIdAsync(nextRack.Id);

                    if (nextRackProducts.Any())
                        break;

                    nextRackIndex++;
                }
            }

            // END FLOW -> SUMMARY
            if (nextRackIndex >= racks.Count)
            {
                var summaryUrl = Url.Action(nameof(LoadRackSummary), new
                {
                    stockCountId = model.StockCountId
                });

                if (nextRackIndex >= racks.Count)
                {
                    return Json(new
                    {
                        success = true,
                        isCompleted = true // key flag
                    });
                }
            }

            var nextUrl = Url.Action(nameof(CountingSequenceRacksProductDetails), new
            {
                stockCountId = model.StockCountId,
                rackIndex = nextRackIndex,
                productIndex = nextProductIndex
            });

            return Json(new { success = true, nextUrl });
        }

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> LoadRackSummary(int stockCountId, bool isSummaryMode = false)
        {
            var stockCountItems = await _countingSequenceService.GetAllStockCountItem(stockCountId);
            var stockCount = await _countingSequenceService.GetStockCountByIdAsync(stockCountId);

            var productIds = stockCountItems.Select(x => x.ProductId).Distinct().ToArray();

            // Load all products at once
            var products = await _productService.GetProductsByIdsAsync(productIds);
            var productDict = products.ToDictionary(p => p.Id);

            var groupedItems = stockCountItems.GroupBy(x => x.ProductId);

            var stockCountItemModel = new List<StockCountItemModel>();

            foreach (var group in groupedItems)
            {
                if (!productDict.TryGetValue(group.Key, out var product))
                    continue;

                var stockQuantity = product.ManageInventoryMethod == ManageInventoryMethod.DontManageStock ? 0
                                    : product.ManageInventoryMethod == ManageInventoryMethod.ManageStock
                                    ? (await _productService.GetTotalStockQuantityAsync(product: product, warehouseId: stockCount.WarehouseId))
                                    : (await _productAttributeService.GetAllProductAttributeCombinationsAsync(product.Id)).Sum(x => x.StockQuantity);

                stockCountItemModel.Add(new StockCountItemModel
                {
                    ProductId = group.Key,
                    ProductName = product.Sku,
                    StockCountId = stockCountId,

                    // aggregated quantity
                    Quantity = group.Sum(x => x.Quantity),

                    RackId = group.First().RackId,
                    PalletId = group.First().PalletId,

                    LastMonthQuantity = stockQuantity,

                    CountSheetXml = group.First().CountSheetXml,
                    ProgressStatusId = group.First().ProgressStatusId,
                    CreatedOnUtc = group.Min(x => x.CreatedOnUtc)
                });
            }

            var model = new CountingSequenceRackProductDetail
            {
                IsSummaryMode = isSummaryMode,
                SummaryItems = stockCountItemModel,
                StockCountId = stockCountId
            };

            return View("~/Plugins/Misc.CountingSequence/Views/CountingSequence/RackCountingSequenceSummary.cshtml", model);
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> CompleteRackCount(int stockCountId)
        {
            var stockCount = await _countingSequenceService.GetStockCountByIdAsync(stockCountId);
            var isJustCompleted = stockCount.ProgressStatusId != (int)ProgressStatus.Completed;

            stockCount.ProgressStatusId = (int)ProgressStatus.Completed;
            await _countingSequenceService.UpdateStockCountAsync(stockCount);

            var existingStockCountItem = (await _countingSequenceService.GetAllStockCountItem(stockCountId)).ToList();
            if (existingStockCountItem.Any())
            {
                // RACK DATA
                if (existingStockCountItem.Any(x => x.RackId > 0))
                {
                    var groupedByRackProduct = existingStockCountItem
                        .Where(x => x.RackId > 0).GroupBy(x => new { x.RackId, x.ProductId });

                    var productTotals = new Dictionary<int, int>();

                    foreach (var group in groupedByRackProduct)
                    {
                        var product = await _productService.GetProductByIdAsync(group.Key.ProductId);
                        var rackProduct = (await _rackService
                            .GetAllRackProductPageAsync(rackId: group.Key.RackId, productId: group.Key.ProductId)).FirstOrDefault();
                        var totalQuantity = group.Sum(x => x.Quantity); // SUM

                        var totalPacks = group.Sum(item =>
                        {
                            var list = CommonHelper.To<List<CountSheetModel>>(item.CountSheetXml);
                            return list?.Sum(x => x.Cases) ?? 0;
                        });

                        var totalUnits = group.Sum(item =>
                        {
                            var list = CommonHelper.To<List<CountSheetModel>>(item.CountSheetXml);
                            return list?.Sum(x => x.Units) ?? 0;
                        });

                        var existingRackStock = (await _stockService.GetRackStocksAsync(rackId: group.Key.RackId,
                            productId: group.Key.ProductId,
                            warehouseId: stockCount.WarehouseId,
                            rackLevelTypeId: rackProduct.RackLevelId,
                            productPositionId: rackProduct.ProductPositionId)).FirstOrDefault();

                        if (existingRackStock == null)
                        {
                            var rackStock = new RackStock
                            {
                                RackId = group.Key.RackId,
                                LevelId = rackProduct.RackLevelId,
                                ProductId = group.Key.ProductId,
                                WarehouseId = stockCount.WarehouseId,
                                NoOfPack = totalPacks,
                                NoOfUnit = totalUnits,
                                Quantity = totalQuantity,
                                //StockDate = DateTime.UtcNow,
                                //StockCountId = stockCount.Id,
                                ProductPositionId = rackProduct.ProductPositionId,
                            };
                            await _stockService.InsertRackStockAsync(rackStock);
                        }
                        else
                        {
                            existingRackStock.NoOfPack = totalPacks;
                            existingRackStock.NoOfUnit = totalUnits;
                            existingRackStock.Quantity = totalQuantity;
                            await _stockService.UpdateRackStockAsync(existingRackStock);
                        }

                        var warehouseStockHistory = new WarehouseStockHistory
                        {
                            ProductId = group.Key.ProductId,
                            WarehouseId = stockCount.WarehouseId,
                            RackId = group.Key.RackId,
                            QuantityAdjustment = totalQuantity - (existingRackStock?.Quantity ?? 0),
                            StockQuantity = totalQuantity,
                            Message = null,
                            StockDate = DateTime.UtcNow,
                            StockCountId = stockCount.Id
                        };
                        await _stockService.InsertWarehouseStockHistoryAsync(warehouseStockHistory);

                        if (!productTotals.ContainsKey(group.Key.ProductId))
                        {
                            productTotals[group.Key.ProductId] = 0;
                        }

                        productTotals[group.Key.ProductId] += totalQuantity;
                    }

                    if (isJustCompleted && stockCount.ProgressStatusId == (int)ProgressStatus.Completed)
                    {
                        foreach (var kvp in productTotals)
                        {
                            var productId = kvp.Key;
                            var totalQty = kvp.Value;
                            var product = await _productService.GetProductByIdAsync(productId);

                            var lastPalletStockCount = (await _countingSequenceService
                                .GetAllStockCountAsync(progressStatusId: ProgressStatus.Completed, countTypeId: 2 /*pallet Type*/))
                                .OrderByDescending(x => x.Id).FirstOrDefault();

                            var previousPalletCountItems = lastPalletStockCount != null
                                ? await _countingSequenceService.GetAllStockCountItem(productId: product.Id, stockCountId: lastPalletStockCount.Id)
                                : null;
                            var previousPalletCountQuantity = previousPalletCountItems != null ? previousPalletCountItems?.Sum(x => x.Quantity) ?? 0 : 0;

                            var begStockQuantity = product.ManageInventoryMethod == ManageInventoryMethod.DontManageStock ? 0
                                    : product.ManageInventoryMethod == ManageInventoryMethod.ManageStock
                                    ? (await _productService.GetTotalStockQuantityAsync(product: product, warehouseId: stockCount.WarehouseId))
                                    : (await _productAttributeService.GetAllProductAttributeCombinationsAsync(product.Id)).Sum(x => x.StockQuantity);

                            //add pallet count if completed
                            var existingPwI = (await _productService.GetAllProductWarehouseInventoryRecordsAsync(product.Id))
                                .FirstOrDefault(x => x.WarehouseId == stockCount.WarehouseId);

                            if (existingPwI != null)
                            {
                                existingPwI.StockQuantity = totalQty + previousPalletCountQuantity;
                                await _productService.UpdateProductWarehouseInventoryAsync(existingPwI);
                            }
                            else
                            {
                                existingPwI = new ProductWarehouseInventory
                                {
                                    WarehouseId = stockCount.WarehouseId,
                                    ProductId = product.Id,
                                    StockQuantity = totalQty + previousPalletCountQuantity,
                                    ReservedQuantity = 0
                                };
                                await _productService.InsertProductWarehouseInventoryAsync(existingPwI);
                            }

                            var lastStockCount = (await _countingSequenceService.GetAllStockCountAsync())
                                                    .OrderByDescending(x => x.CreatedOnUtc).ToList();

                            var lastStockCountWSH = lastStockCount != null && lastStockCount.Count >= 3 ?
                                (await _stockService.GetAllWarehouseStockHistoryAsync(stockCountId: lastStockCount[2].Id)).FirstOrDefault()
                                : null;

                            var backOrders = lastStockCountWSH != null ?
                                await _backOrdersService.GetAllBackOrdersPagedAsync(startDate: lastStockCountWSH.StockDate, searchStatus: (int)BackOrdersEnum.Fulfilled)
                                : null;

                            if (backOrders != null)
                            {
                                await _productService.AddStockQuantityHistoryEntryAsync(product, -backOrders.Sum(x => x.Quantity),
                                    begStockQuantity - backOrders.Sum(x => x.Quantity), stockCount.WarehouseId,
                                    string.Format(await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Fields.BackOrder.PlaceOrder"), stockCount.Name));

                                begStockQuantity = begStockQuantity - backOrders.Sum(x => x.Quantity);
                            }

                            await _productService.AddStockQuantityHistoryEntryAsync(product, existingPwI.StockQuantity - begStockQuantity,
                                existingPwI.StockQuantity, stockCount.WarehouseId,
                                string.Format(await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Fields.CountComplete"), stockCount.Name));
                        }
                    }
                }
            }

            return RedirectToAction("List");
        }

        #endregion

        #region Shipement Dispatch
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> ShipementDispatchPopup(int warehouseId)
        {
            var shipmentDispatches = (await _shipmentDispatchService.GetShipmentDispatchesAsync(warehouseId)).ToList();
            var model = new List<ShipmentDispatchModel>();
            foreach (var shipmentDispatch in shipmentDispatches)
            {
                var shipmentDispatchModel = new ShipmentDispatchModel();
                shipmentDispatchModel.Id = shipmentDispatch.Id;
                shipmentDispatchModel.Name = shipmentDispatch.Name;
                model.Add(shipmentDispatchModel);
            }

            return PartialView("~/Plugins/Misc.CountingSequence/Views/CountingSequence/_ShipmentDispatch.cshtml", model);
        }
        #endregion

        #region pallet Counting sequence

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> InUsePallet(int[] shipmentDispatchIds)
        {
            if (shipmentDispatchIds == null || shipmentDispatchIds.Length == 0)
                return PartialView("~/Plugins/Misc.CountingSequence/Views/CountingSequence/_InUsePallet.cshtml", new List<PalletModel>());

            var pallets = await _palletService.GetPalletByShipmentDispatchIdsAsync(shipmentDispatchIds);
            if (_countingSequenceSettings.PalletCountingOrder == PalletCounting.NewToOld)
                pallets = pallets.OrderByDescending(x => x.SequenceOrder).ToList();
            else
                pallets = pallets.OrderBy(x => x.SequenceOrder).ToList();

            var model = new List<PalletModel>();
            foreach (var p in pallets)
            {
                model.Add(new PalletModel
                {
                    Id = p.Id,
                    Name = p.Name,
                });
            }

            return PartialView("~/Plugins/Misc.CountingSequence/Views/CountingSequence/_InUsePallet.cshtml", model);
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> InUsePallet(int[] palletIds, int[] shipmentDispatchIds, int warehouseId)
        {
            var stockCount = new StockCount
            {
                Name = $"Count - {DateTime.UtcNow.ToString("MM/dd/yyyy")}",
                CutsomerId = (await _workContext.GetCurrentCustomerAsync()).Id,
                WarehouseId = warehouseId,
                ProgressStatusId = (int)ProgressStatus.Pending,
                CreatedOnUtc = DateTime.UtcNow,
                DiscontinuePalletIds = palletIds != null ? string.Join(",", palletIds) : null,
                DeliveredShipmentIds = shipmentDispatchIds != null ? string.Join(",", shipmentDispatchIds) : null,
            };

            await _countingSequenceService.InsertStockCountAsync(stockCount);

            var allPallets = await _palletService.GetPalletByShipmentDispatchIdsAsync(shipmentDispatchIds);
            if (_countingSequenceSettings.PalletCountingOrder == PalletCounting.NewToOld)
                allPallets = allPallets.OrderByDescending(x => x.SequenceOrder).ToList();
            else
                allPallets = allPallets.OrderBy(x => x.SequenceOrder).ToList();

            var palletIdSet = new HashSet<int>(palletIds);

            // Only run this if pallets are selected
            if (palletIds != null && palletIds.Length > 0)
            {
                var selectedPallets = allPallets.Where(p => palletIdSet.Contains(p.Id)).ToList();

                foreach (var pallet in selectedPallets)
                {
                    // Update pallet
                    pallet.IsVisible = false;
                    await _palletService.UpdatePalletAsync(pallet);
                }
            }

            var url = Url.Action(nameof(CountingSequencePalletsProductDetails), new
            {
                stockCountId = stockCount.Id
            });

            return Json(new { redirectUrl = url });
        }

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> CountingSequencePalletsProductDetails(int stockCountId, int palletIndex = 0, int productIndex = 0)
        {
            var stockCount = await _countingSequenceService.GetStockCountByIdAsync(stockCountId);
            int[] deliveredShipmentIds = stockCount.DeliveredShipmentIds
                                    .Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            var pallets = await _palletService.GetPalletByShipmentDispatchIdsAsync(deliveredShipmentIds);
            if (_countingSequenceSettings.PalletCountingOrder == PalletCounting.NewToOld)
                pallets = pallets.OrderByDescending(x => x.SequenceOrder).ToList();
            else
                pallets = pallets.OrderBy(x => x.SequenceOrder).ToList();

            if (!pallets.Any())
                throw new ArgumentException("No pallets found.");

            if (palletIndex < 0)
                palletIndex = 0;

            if (palletIndex >= pallets.Count)
                palletIndex = pallets.Count - 1;

            var currentPallet = pallets[palletIndex];

            var palletProducts = await _palletService.GetProductByPalletIdAsync(currentPallet.Id);
            var productIds = palletProducts.Select(pp => pp.ProductId).ToList();
            var products = (await _productService.GetProductsByIdsAsync(productIds.ToArray()))
                .OrderBy(p => productIds.IndexOf(p.Id)).ToList();

            // Skip empty pallet
            if (!products.Any())
            {
                // check if any pallet has products
                bool hasAnyProduct = false;

                foreach (var pallet in pallets)
                {
                    var palletItems = await _palletService.GetProductByPalletIdAsync(pallet.Id);

                    if (palletItems.Any())
                    {
                        hasAnyProduct = true;
                        break;
                    }
                }

                if (hasAnyProduct)
                    return RedirectToAction(nameof(CountingSequencePalletsProductDetails), new
                    {
                        stockCountId,
                        palletIndex = palletIndex + 1,
                        productIndex = 0
                    });
            }

            if (productIndex < 0)
            {
                palletIndex--;

                if (palletIndex >= 0)
                {
                    var prevPallet = pallets[palletIndex];

                    var prevPalletProducts = await _palletService.GetProductByPalletIdAsync(prevPallet.Id);
                    var prevProductIds = prevPalletProducts.Select(x => x.ProductId).ToList();
                    var prevProducts = (await _productService.GetProductsByIdsAsync(prevProductIds.ToArray()))
                        .OrderBy(p => prevProductIds.IndexOf(p.Id)).ToList();

                    productIndex = prevProducts.Count - 1;

                    products = prevProducts;
                    currentPallet = prevPallet;
                }
                else
                {
                    palletIndex = 0;
                    productIndex = 0;
                }
            }

            var product = products[productIndex];

            var shipmentTransitItems = await _shipmentTransitService.GetAllShipmentTransitItemsAsync(product.Id);

            // Group by shipment
            var groupedItems = shipmentTransitItems
                .GroupBy(x => x.ShipmentTransitId)
                .Select(g => new
                {
                    ShipmentTransitId = g.Key,
                    TotalQuantity = g.Sum(x => x.Quantity)
                });

            var shipmentList = new List<(ProductShipmentModel Model, DateTime Date, int Status)>();

            foreach (var group in groupedItems)
            {
                var transit = await _shipmentTransitService.GetShipmentTransitByIdAsync(group.ShipmentTransitId);
                if (transit == null)
                    continue;

                var dispatch = await _shipmentDispatchService.GetShipmentDispatchByIdAsync(transit.FromDispatchId);
                if (dispatch == null || dispatch.DispatchType != (int)ShipmentDispatchType.USShipment)
                    continue;

                var date = new DateTime(dispatch.ShippedYear, dispatch.ShippedMonth, 1);

                var productShipmentModel = new ProductShipmentModel
                {
                    ShipmentName = dispatch.Name,
                    ShipmentDispatchesDate = date.ToString("MMM-yyyy"),
                    ShipmentTransitItemsQuantity = group.TotalQuantity
                };

                shipmentList.Add((productShipmentModel, date, transit.Status));
            }

            // Sort by latest
            var sorted = shipmentList.OrderByDescending(x => x.Date).ToList();

            // Separate after sorting
            var recentShipments = sorted?.Where(x => x.Status == (int)ShipmentTransitStatus.Received)?
                .Take(6)?.Select(x => x.Model)?.ToList() ?? new List<ProductShipmentModel>();
            var incomingShipments = sorted?.Where(x => x.Status != (int)ShipmentTransitStatus.Received)?
                .Take(2)?.Select(x => x.Model)?.ToList() ?? new List<ProductShipmentModel>();

            var model = new CountingSequencePalletProductDetail
            {
                StockCountModel =
                {
                    Id = stockCount.Id,
                    Name = stockCount.Name,
                    WarehouseName = (await _warehouseService.GetWarehouseByIdAsync(stockCount.WarehouseId))?.Name ?? "-",
                    ProgressStatusName = await _localizationService.GetLocalizedEnumAsync(stockCount.ProgressStatus),
                    CountType = "Pallet",
                    CreatedOnUtc = stockCount.CreatedOnUtc.ToString("MM/dd/yyyy"),
                },
                PalletCountingOrder = await _localizationService.GetLocalizedEnumAsync(_countingSequenceSettings.PalletCountingOrder),
                ProductDetailsModel = await _productModelFactory.PrepareProductDetailsModelAsync(product, null, false),
                QuantityPerPack = product.OrderMaximumQuantity,
                CategoryName = (await _categoryService
                    .GetCategoryByIdAsync((await _categoryService.GetProductCategoriesByProductIdAsync(product.Id)).FirstOrDefault()?.CategoryId ?? 0))?.Name,

                StockCountId = stockCountId,
                CurrentIndex = productIndex,
                PalletIndex = palletIndex,
                CurrentPalletId = currentPallet.Id,
                TotalCount = products.Count,
                TotalPallets = pallets.Count,

                AvailableProductPallets = (await _palletService.GetPalletByProductIdAsync(product.Id)).Where(x => x.Id != currentPallet.Id).ToList(),
                AvailablePallets = pallets,
                RecentShipments = recentShipments,
                IncomingShipments = incomingShipments,
            };

            var existingProduct = (await _countingSequenceService.GetAllStockCountItem(productId: product.Id)).ToList();
            var existingProductStockOnPallet = existingProduct.Where(x => x.StockCountId == stockCountId && x.PalletId == currentPallet.Id).FirstOrDefault();
            if (existingProductStockOnPallet != null && existingProductStockOnPallet.CountSheetXml != null)
            {
                var countSheetModel = CommonHelper.To<List<CountSheetModel>>(existingProductStockOnPallet.CountSheetXml);
                model.CountSheetRows = countSheetModel
                .Select(x => new PalletCountSheetModel
                {
                    Cases = x.Cases,
                    Units = x.Units,
                }).ToList();
                model.TotalCountingQuantity = existingProductStockOnPallet?.Quantity ?? 0;
            }
            else
            {
                model.CountSheetRows.Add(new PalletCountSheetModel
                {
                    Cases = 0,
                    Units = 0
                });
            }

            model.TotalCountingQuantityLastMonth = existingProduct?
                .Where(x => x.CreatedOnUtc.Month == DateTime.UtcNow.AddMonths(-1).Month && x.PalletId == currentPallet.Id)?.Sum(x => x.Quantity) ?? 0;

            model.TotalProductQuantity = existingProduct?
                .Where(x => x.CreatedOnUtc.Month == DateTime.UtcNow.Month && x.StockCountId == stockCountId)?.Sum(x => x.Quantity) ?? 0;

            var lastMonthwarehouseStock = await _stockService.GetAllWarehouseStockHistoryAsync(productId: product.Id, warehouseId: stockCount.WarehouseId,
                createdFrom: new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).AddMonths(-1),
                createdTo: new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).AddTicks(-1));
            model.TotalProductQuantityLastMonth = lastMonthwarehouseStock?.Sum(x => x.StockQuantity) ?? 0;

            var begInv = (await _productService.GetAllProductWarehouseInventoryRecordsAsync(product.Id) ?? Enumerable.Empty<ProductWarehouseInventory>())
                        .Where(x => x.WarehouseId == stockCount.WarehouseId)
                        .Sum(x => (int?)x.StockQuantity) ?? 0;

            var shipmentDispatchIds = (await _shipmentDispatchService.GetAllShipementDispatchAsync(
                    shipmentDispatchId: (int)ShipmentDispatchType.USShipment,
                    minMonth: DateTime.UtcNow.Month,
                    maxMonth: DateTime.UtcNow.Month,
                    minYear: DateTime.UtcNow.Year,
                    maxYear: DateTime.UtcNow.Year)
                ?? Enumerable.Empty<ShipmentDispatches>())
                .Select(x => x.Id).ToList();

            var shipmentTransitIds = (await _shipmentTransitService.GetShipmentTransitAllAsync()
                ?? Enumerable.Empty<ShipmentTransit>())
                .Where(x => shipmentDispatchIds.Contains(x.FromDispatchId) && x.Status == (int)ShipmentTransitStatus.Received)
                .Select(x => x.Id).ToList();

            var received = (shipmentTransitItems ?? Enumerable.Empty<ShipmentTransitItems>())
                .Where(x => shipmentTransitIds.Contains(x.ShipmentTransitId))
                .Sum(x => (int?)x.Quantity) ?? 0;

            var counted = model?.TotalProductQuantity ?? 0;

            var backOrders = (await _backOrdersService.GetAllBackOrdersPagedAsync(
                    productId: product.Id,
                    startDate: new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1),
                    endDate: DateTime.UtcNow)
                ?? Enumerable.Empty<BackOrders>())
                .Sum(x => (int?)x.Quantity) ?? 0;

            model.TotalSaleThisMonth = begInv + received - counted + backOrders;

            var startDate = DateTime.UtcNow.AddMonths(-6);
            var endDate = DateTime.UtcNow;

            // Shipment Dispatch (last 6 months)
            var shipmentDispatchIds6 = (await _shipmentDispatchService.GetAllShipementDispatchAsync(
                    shipmentDispatchId: (int)ShipmentDispatchType.USShipment,
                    minMonth: startDate.Month,
                    maxMonth: endDate.Month,
                    minYear: startDate.Year,
                    maxYear: endDate.Year)
                ?? Enumerable.Empty<ShipmentDispatches>())
                .Select(x => x.Id)
                .ToList();

            // Shipment Transit
            var shipmentTransitIds6 = (await _shipmentTransitService.GetShipmentTransitAllAsync()
                    ?? Enumerable.Empty<ShipmentTransit>())
                .Where(x => shipmentDispatchIds.Contains(x.FromDispatchId) && x.Status == (int)ShipmentTransitStatus.Received)
                .Select(x => x.Id)
                .ToList();

            // Received Quantity
            var received6 = (shipmentTransitItems ?? Enumerable.Empty<ShipmentTransitItems>())
                .Where(x => shipmentTransitIds.Contains(x.ShipmentTransitId))
                .Sum(x => (int?)x.Quantity) ?? 0;

            // Counted
            var counted6 = model?.TotalProductQuantity ?? 0;

            // BackOrders (last 6 months)
            var backOrders6 = (await _backOrdersService.GetAllBackOrdersPagedAsync(
                    productId: product.Id,
                    startDate: startDate,
                    endDate: endDate)
                ?? Enumerable.Empty<BackOrders>())
                .Sum(x => (int?)x.Quantity) ?? 0;

            // Final Total
            model.TotalSaleThisLastSixMonth = begInv + received6 - counted6 + backOrders6;

            return View("~/Plugins/Misc.CountingSequence/Views/CountingSequence/CountingSequencePalletProductDetails.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> LoadPalletCountingSequenceStatistics(int palletId, int productId)
        {
            var result = new List<object>();

            var nowUtc = DateTime.UtcNow;
            var currentMonthStart = new DateTime(nowUtc.Year, nowUtc.Month, 1);
            var startMonth = currentMonthStart.AddMonths(-5);

            for (int i = 0; i < 6; i++)
            {
                var from = startMonth.AddMonths(i);
                var to = from.AddMonths(1);

                var qty = (await _countingSequenceService.GetAllStockCountItem(
                                productId: productId,
                                startDate: from,
                                endDate: to)
                           ?? Enumerable.Empty<StockCountItem>())
                    .Sum(x => (int?)x.Quantity) ?? 0;

                var begInv = (await _productService.GetAllProductWarehouseInventoryRecordsAsync(productId) ?? Enumerable.Empty<ProductWarehouseInventory>())
                        //.Where(x => x.WarehouseId == stockCount.WarehouseId)
                        .Sum(x => (int?)x.StockQuantity) ?? 0;

                var backOrdersQty = (await _backOrdersService.GetAllBackOrdersPagedAsync(
                                        productId: productId,
                                        startDate: from,
                                        endDate: to)
                                     ?? Enumerable.Empty<BackOrders>())
                    .Sum(x => (int?)x.Quantity) ?? 0;

                var shipmentDispatchIds = (await _shipmentDispatchService.GetAllShipementDispatchAsync(
                        shipmentDispatchId: (int)ShipmentDispatchType.USShipment,
                        minMonth: from.Month,
                        maxMonth: from.Month,
                        minYear: from.Year,
                        maxYear: from.Year)
                    ?? Enumerable.Empty<ShipmentDispatches>())
                    .Select(x => x.Id)
                    .ToList();

                var shipmentTransitIds = (await _shipmentTransitService.GetShipmentTransitAllAsync()
                        ?? Enumerable.Empty<ShipmentTransit>())
                    .Where(x => shipmentDispatchIds.Contains(x.FromDispatchId) && x.Status == (int)ShipmentTransitStatus.Received)
                    .Select(x => x.Id)
                    .ToList();

                var shipmentTransitItems = await _shipmentTransitService.GetAllShipmentTransitItemsAsync(productId);

                var received = (shipmentTransitItems ?? Enumerable.Empty<ShipmentTransitItems>())
                    .Where(x => shipmentTransitIds.Contains(x.ShipmentTransitId))
                    .Sum(x => (int?)x.Quantity) ?? 0;

                var counted = qty;

                var sales = begInv + received - counted + backOrdersQty;

                // Final push
                result.Add(new SaleStatistics
                {
                    Month = from.ToString("MMM yyyy"),
                    Count = qty,
                    Sales = sales
                });
            }

            return Json(result);
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> SavePalletCountSheet(CountingSequencePalletProductDetail model)
        {
            var countSheetItems = model.CountSheetRows
                .Select(x => new CountSheetModel
                {
                    Cases = x.Cases,
                    Units = x.Units
                }).ToList();

            var countSheetItemsXML = CommonHelper.To<string>(countSheetItems);

            var stockCount = await _countingSequenceService.GetStockCountByIdAsync(model.StockCountId);
            int[] deliveredShipmentIds = stockCount.DeliveredShipmentIds
                                    .Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            var pallets = await _palletService.GetPalletByShipmentDispatchIdsAsync(deliveredShipmentIds);
            if (_countingSequenceSettings.PalletCountingOrder == PalletCounting.NewToOld)
                pallets = pallets.OrderByDescending(x => x.SequenceOrder).ToList();
            else
                pallets = pallets.OrderBy(x => x.SequenceOrder).ToList();

            var currentPallet = pallets[model.PalletIndex];

            var existingStockCountItem = (await _countingSequenceService
                .GetAllStockCountItem(stockCountId: model.StockCountId, palletId: currentPallet.Id, productId: model.ProductDetailsModel.Id))
                .FirstOrDefault();

            if (existingStockCountItem == null)
            {
                var stockCountItem = new StockCountItem
                {
                    PalletId = currentPallet.Id,
                    StockCountId = model.StockCountId,
                    ProductId = model.ProductDetailsModel.Id,
                    CountSheetXml = countSheetItemsXML,
                    CreatedOnUtc = DateTime.UtcNow,
                    Quantity = model.TotalCountingQuantity
                };

                await _countingSequenceService.InsertStockCountItemAsync(stockCountItem);
            }
            else
            {
                existingStockCountItem.Quantity = model.TotalCountingQuantity;
                existingStockCountItem.CountSheetXml = countSheetItemsXML;
                await _countingSequenceService.UpdateStockCountItemAsync(existingStockCountItem);
            }

            var palletProducts = await _palletService.GetProductByPalletIdAsync(currentPallet.Id);

            var productIds = palletProducts.Select(pp => pp.ProductId).ToList();

            var products = (await _productService.GetProductsByIdsAsync(productIds.ToArray()))
                .OrderBy(p => productIds.IndexOf(p.Id)).ToList();

            int nextProductIndex = model.CurrentIndex + 1;
            int nextPalletIndex = model.PalletIndex;

            // move to next pallet if current finished
            if (nextProductIndex >= products.Count)
            {
                nextPalletIndex++;
                nextProductIndex = 0;

                // skip empty pallets
                while (nextPalletIndex < pallets.Count)
                {
                    var nextPallet = pallets[nextPalletIndex];
                    var nextPalletProducts = await _palletService.GetProductByPalletIdAsync(nextPallet.Id);

                    if (nextPalletProducts.Any())
                        break;

                    nextPalletIndex++;
                }
            }

            // END FLOW -> SUMMARY
            if (nextPalletIndex >= pallets.Count)
            {
                var summaryUrl = Url.Action(nameof(LoadPalletSummary), new
                {
                    stockCountId = model.StockCountId
                });

                if (nextPalletIndex >= pallets.Count)
                {
                    return Json(new
                    {
                        success = true,
                        isCompleted = true // key flag
                    });
                }
            }

            var nextUrl = Url.Action(nameof(CountingSequencePalletsProductDetails), new
            {
                stockCountId = model.StockCountId,
                palletIndex = nextPalletIndex,
                productIndex = nextProductIndex
            });

            return Json(new { success = true, nextUrl });
        }

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> LoadPalletSummary(int stockCountId, bool isSummaryMode = false)
        {
            var stockCountItems = await _countingSequenceService.GetAllStockCountItem(stockCountId);
            var stockCount = await _countingSequenceService.GetStockCountByIdAsync(stockCountId);

            var productIds = stockCountItems.Select(x => x.ProductId).Distinct().ToArray();

            // Load all products at once
            var products = await _productService.GetProductsByIdsAsync(productIds);
            var productDict = products.ToDictionary(p => p.Id);

            var groupedItems = stockCountItems.GroupBy(x => x.ProductId);

            var stockCountItemModel = new List<StockCountItemModel>();

            foreach (var group in groupedItems)
            {
                if (!productDict.TryGetValue(group.Key, out var product))
                    continue;

                var stockQuantity = product.ManageInventoryMethod == ManageInventoryMethod.DontManageStock ? 0
                                    : product.ManageInventoryMethod == ManageInventoryMethod.ManageStock
                                    ? (await _productService.GetTotalStockQuantityAsync(product: product, warehouseId: stockCount.WarehouseId))
                                    : (await _productAttributeService.GetAllProductAttributeCombinationsAsync(product.Id)).Sum(x => x.StockQuantity);

                stockCountItemModel.Add(new StockCountItemModel
                {
                    ProductId = group.Key,
                    ProductName = product.Sku,
                    StockCountId = stockCountId,

                    // aggregated quantity
                    Quantity = group.Sum(x => x.Quantity),

                    RackId = group.First().RackId,
                    PalletId = group.First().PalletId,

                    LastMonthQuantity = stockQuantity,

                    CountSheetXml = group.First().CountSheetXml,
                    ProgressStatusId = group.First().ProgressStatusId,
                    CreatedOnUtc = group.Min(x => x.CreatedOnUtc)
                });
            }

            var model = new CountingSequencePalletProductDetail
            {
                IsSummaryMode = isSummaryMode,
                SummaryItems = stockCountItemModel,
                StockCountId = stockCountId,
            };

            return View("~/Plugins/Misc.CountingSequence/Views/CountingSequence/PalletCountingSequenceSummary.cshtml", model);
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> CompletePalletCount(int stockCountId)
        {
            var stockCount = await _countingSequenceService.GetStockCountByIdAsync(stockCountId);
            stockCount.ProgressStatusId = (int)ProgressStatus.Completed;
            await _countingSequenceService.UpdateStockCountAsync(stockCount);

            var existingStockCountItem = (await _countingSequenceService.GetAllStockCountItem(stockCountId)).ToList();
            if (existingStockCountItem.Count != 0)
            {
                // Pallet DATA
                if (existingStockCountItem.Any(x => x.PalletId > 0))
                {
                    var groupedByPalletProduct = existingStockCountItem
                        .Where(x => x.PalletId > 0).GroupBy(x => new { x.PalletId, x.ProductId });

                    foreach (var group in groupedByPalletProduct)
                    {
                        var product = await _productService.GetProductByIdAsync(group.Key.ProductId);
                        var totalQuantity = group.Sum(x => x.Quantity);

                        var totalPacks = group.Sum(item =>
                        {
                            var list = CommonHelper.To<List<CountSheetModel>>(item.CountSheetXml);
                            return list?.Sum(x => x.Cases) ?? 0;
                        });

                        var totalUnits = group.Sum(item =>
                        {
                            var list = CommonHelper.To<List<CountSheetModel>>(item.CountSheetXml);
                            return list?.Sum(x => x.Units) ?? 0;
                        });

                        var shipmentTransitItem = (await _shipmentTransitService
                            .GetAllShipmentTransitItemsAsync(productId: group.Key.ProductId, palletId: group.Key.PalletId)).FirstOrDefault();

                        var stockQuantity = (await _stockService.GetPalletStocksAsync(productId: group.Key.ProductId,
                            palletId: group.Key.PalletId,
                            warehouseId: stockCount.WarehouseId,
                            shipmentTransitId: shipmentTransitItem?.ShipmentTransitId ?? 0)).LastOrDefault();

                        var existingPalletStock = (await _stockService.GetPalletStocksAsync(productId: group.Key.ProductId, palletId: group.Key.PalletId,
                            warehouseId: stockCount.WarehouseId, shipmentTransitId: shipmentTransitItem?.ShipmentTransitId ?? 0)).FirstOrDefault();

                        if (existingPalletStock == null)
                        {
                            var palletStock = new PalletStock
                            {
                                PalletId = group.Key.PalletId,
                                ProductId = group.Key.ProductId,
                                WarehouseId = stockCount.WarehouseId,
                                NoOfPack = totalPacks,
                                NoOfUnit = totalUnits,
                                Quantity = totalQuantity,
                                //StockDate = DateTime.UtcNow,
                                ShipmentTransitId = shipmentTransitItem?.ShipmentTransitId ?? 0,
                                ToDispatchId = shipmentTransitItem?.ToDispatchId ?? 0,
                                //StockCountId = stockCount.Id,
                                AvailableQuantity = totalQuantity
                            };
                            await _stockService.InsertPalletStockAsync(palletStock);
                        }
                        else
                        {
                            existingPalletStock.NoOfPack = totalPacks;
                            existingPalletStock.NoOfUnit = totalUnits;
                            existingPalletStock.Quantity = totalQuantity;
                            existingPalletStock.ToDispatchId = shipmentTransitItem?.ToDispatchId ?? 0;
                            existingPalletStock.AvailableQuantity = totalQuantity;
                            await _stockService.UpdatePalletStockAsync(existingPalletStock);
                        }

                        var warehouseStockHistory = new WarehouseStockHistory
                        {
                            ProductId = group.Key.ProductId,
                            WarehouseId = stockCount.WarehouseId,
                            PalletId = group.Key.PalletId,
                            QuantityAdjustment = totalQuantity - (stockQuantity?.Quantity ?? 0),
                            StockQuantity = totalQuantity,
                            Message = null,
                            StockDate = DateTime.UtcNow,
                            StockCountId = stockCount.Id
                        };
                        await _stockService.InsertWarehouseStockHistoryAsync(warehouseStockHistory);
                    }

                    int[] deliveredShipments = stockCount.DeliveredShipmentIds
                                    .Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                    foreach (var deliveredShipment in deliveredShipments)
                    {
                        var shipmentTransit = await _shipmentTransitService.GetShipmentTransitByIdAsync(deliveredShipment);
                        shipmentTransit.Status = (int)ShipmentTransitStatus.Received;
                        await _shipmentTransitService.UpdateShipmentTransitAsync(shipmentTransit);
                    }

                    int[] discontinuePallets = stockCount.DiscontinuePalletIds
                                    .Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                    foreach (var discontinuePallet in discontinuePallets)
                    {
                        var existingPalletStocks = (await _stockService.GetPalletStocksAsync(palletId: discontinuePallet,
                            warehouseId: stockCount.WarehouseId)).ToList();

                        foreach (var existingPalletStock in existingPalletStocks)
                        {
                            existingPalletStock.NoOfPack = 0;
                            existingPalletStock.NoOfUnit = 0;
                            existingPalletStock.Quantity = 0;
                            existingPalletStock.AvailableQuantity = existingPalletStock.AvailableQuantity;
                            await _stockService.UpdatePalletStockAsync(existingPalletStock);

                            var warehouseStockHistory = new WarehouseStockHistory
                            {
                                ProductId = existingPalletStock.ProductId,
                                WarehouseId = stockCount.WarehouseId,
                                PalletId = discontinuePallet,
                                QuantityAdjustment = -existingPalletStock.Quantity,
                                StockQuantity = 0,
                                Message = null,
                                StockDate = DateTime.UtcNow,
                                StockCountId = stockCount.Id
                            };
                            await _stockService.InsertWarehouseStockHistoryAsync(warehouseStockHistory);
                        }
                    }
                }
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> DeleteCount(int id)
        {
            var stockCount = await _countingSequenceService.GetStockCountByIdAsync(id);
            if (stockCount != null)
            {
                if (stockCount.DiscontinuePalletIds != null)
                {
                    int[] palletIds = stockCount.DiscontinuePalletIds
                                    .Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

                    foreach (var palletId in palletIds)
                    {
                        var blockedPallet = await _palletService.GetPalletByIdAsync(palletId);
                        blockedPallet.IsVisible = true;
                        await _palletService.UpdatePalletAsync(blockedPallet);
                    }
                }

                var stockCountItems = await _countingSequenceService.GetAllStockCountItem(stockCountId: stockCount.Id);
                if (stockCountItems != null && stockCountItems.Count > 0)
                    foreach (var stockCountItem in stockCountItems)
                        await _countingSequenceService.DeleteStockCountItemAsync(stockCountItem);

                await _countingSequenceService.DeleteStockCountAsync(stockCount);
            }

            return Json(new { success = true, message = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.StockCount.DeleteMessage") });
        }

        #endregion

        #endregion
    }
}
