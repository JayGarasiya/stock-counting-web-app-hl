using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Misc.CountingSequence.Domain;
using Nop.Plugin.Misc.CountingSequence.Factories;
using Nop.Plugin.Misc.CountingSequence.Infrastructure;
using Nop.Plugin.Misc.CountingSequence.Models.ShipmentTransit;
using Nop.Plugin.Misc.CountingSequence.Models.ShipmentTransitItem;
using Nop.Plugin.Misc.CountingSequence.Services;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.CountingSequence.Controllers
{
    public class ShipmentTransitController : BaseAdminController
    {
        #region Fields

        protected readonly IShipmentTransitService _shipmentTransitService;
        protected readonly IShipmentTransitModelFactory _shipmentTransitModelFactory;
        protected readonly IPalletService _palletService;
        protected readonly IProductService _productService;
        protected readonly IShipmentTransitItemService _shipmentTransitItemService;
        protected readonly INotificationService _notificationService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IShipmentDispatchService _shipmentDispatchService;

        #endregion

        #region Ctor

        public ShipmentTransitController(
            IShipmentTransitService shipmentTransitService,
            IShipmentTransitModelFactory shipmentTransitModelFactory,
            IPalletService palletService,
            IProductService productService,
            IShipmentTransitItemService shipmentTransitItemService,
            INotificationService notificationService,
            ILocalizationService localizationService,
            IShipmentDispatchService shipmentDispatchService)
        {
            _shipmentTransitService = shipmentTransitService;
            _shipmentTransitModelFactory = shipmentTransitModelFactory;
            _palletService = palletService;
            _productService = productService;
            _shipmentTransitItemService = shipmentTransitItemService;
            _notificationService = notificationService;
            _localizationService = localizationService;
            _shipmentDispatchService = shipmentDispatchService;
        }

        #endregion

        #region Methods

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> List()
        {
            var model = await _shipmentTransitModelFactory.PrepareShipmentTransitSearchModelAsync(new ShipmentTransitSearchModel());
            return View("~/Plugins/Misc.CountingSequence/Views/ShipmentTransit/List.cshtml", model);
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> List(ShipmentTransitSearchModel searchModel)
        {
            var model = await _shipmentTransitModelFactory.PrepareShipmentTransitListModelAsync(searchModel);
            return Json(model);
        }

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> Create()
        {
            var model = await _shipmentTransitModelFactory.PrepareShipmentTransitModelAsync(new ShipmentTransitModel(), null);
            return View("~/Plugins/Misc.CountingSequence/Views/ShipmentTransit/Create.cshtml", model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> Create(ShipmentTransitModel model, bool continueEditing)
        {
            var allTransits = await _shipmentTransitService.GetShipmentTransitAllAsync(pageIndex: 0, pageSize: int.MaxValue);
            if (allTransits.Any(x => x.FromDispatchId == model.FromDispatchId))
            {
                var message = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.ShipmentTransit.Validtion.Create");
                ModelState.AddModelError("", message);
                _notificationService.ErrorNotification(message);
            }

            if (ModelState.IsValid)
            {
                var shipmentTransit = new ShipmentTransit
                {
                    FromDispatchId = model.FromDispatchId,
                    Status = model.Status,
                    Id = model.PalletCount
                };

                await _shipmentTransitService.InsertShipmentTransitAsync(shipmentTransit);
                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.ShipmentTransit.CreateMessage"));

                if (model.PalletCount > 0)
                {
                    var ShipmentDispatches = await _shipmentDispatchService.GetShipmentDispatchByIdAsync(model.FromDispatchId);
                    var pallets = await _palletService.GetAllPalletPagedAsync();
                    for (int i = 1; i <= model.PalletCount; i++)
                    {
                        var pallet = new Pallet
                        {
                            Name = ShipmentDispatches.Name + "." + i.ToString(),
                            ShipmentDispatchId = ShipmentDispatches.Id,
                            DisplayOrder = (pallets?.Select(x => x.DisplayOrder).DefaultIfEmpty(0).Max() ?? 0) + i,
                            IsVisible = true,
                            Description = null,
                            SequenceOrder = (pallets?.Select(x => x.SequenceOrder).DefaultIfEmpty(0).Max() ?? 0) + i
                        };
                        await _palletService.InsertPalletAsync(pallet);
                    }
                }

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = shipmentTransit.Id });
            }

            model = await _shipmentTransitModelFactory.PrepareShipmentTransitModelAsync(model, null);
            return View("~/Plugins/Misc.CountingSequence/Views/ShipmentTransit/Create.cshtml", model);
        }

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> Edit(int id)
        {
            var shipmentTransit = await _shipmentTransitService.GetShipmentTransitByIdAsync(id);
            if (shipmentTransit == null)
                return RedirectToAction("List");

            var model = await _shipmentTransitModelFactory
                .PrepareShipmentTransitModelAsync(null, shipmentTransit);

            return View("~/Plugins/Misc.CountingSequence/Views/ShipmentTransit/Edit.cshtml", model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> Edit(ShipmentTransitModel model, bool continueEditing)
        {
            var allTransits = await _shipmentTransitService.GetShipmentTransitAllAsync(pageIndex: 0, pageSize: int.MaxValue);
            if (allTransits.Any(x => x.FromDispatchId == model.FromDispatchId && x.Id != model.Id))
            {
                var message = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.ShipmentTransit.Validtion.Edit");
                ModelState.AddModelError("", message);
                _notificationService.ErrorNotification(message);
            }


            var shipmentTransit = await _shipmentTransitService.GetShipmentTransitByIdAsync(model.Id);
            if (shipmentTransit == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                shipmentTransit.FromDispatchId = model.FromDispatchId;
                shipmentTransit.Status = model.Status;

                await _shipmentTransitService.UpdateShipmentTransitAsync(shipmentTransit);
                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.ShipmentTransit.UpdateMessage"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = shipmentTransit.Id });
            }

            model = await _shipmentTransitModelFactory.PrepareShipmentTransitModelAsync(model, shipmentTransit);
            return View("~/Plugins/Misc.CountingSequence/Views/ShipmentTransit/Edit.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> Delete(int id)
        {
            var shipmentTransit = await _shipmentTransitService.GetShipmentTransitByIdAsync(id);

            if (shipmentTransit != null)
                await _shipmentTransitService.DeleteShipmentTransitAsync(shipmentTransit);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.ShipmentTransit.DeleteMessage"));

            return RedirectToAction("List");
        }

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> AddItem(int shipmentTransitId)
        {
            var model = new ShipmentTransitItemModel
            {
                ShipmentTransitId = shipmentTransitId,
                items = new List<TransitItem>
                {
                    new TransitItem()
                }
            };
            
            var shipmentTransit = await _shipmentTransitService.GetShipmentTransitByIdAsync(shipmentTransitId);

            var pallets = await _palletService.GetAllPalletPagedAsync(pageIndex: 0, pageSize: int.MaxValue);

            model.AvailablePallets = pallets.Where(x => x.ShipmentDispatchId == shipmentTransit.FromDispatchId)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList();

            var products = await _productService.SearchProductsAsync();
            model.AvailableProducts = products.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Sku
            }).ToList();

            var dispatches = await _shipmentDispatchService.GetAllShipementDispatchAsync(pageIndex: 0, pageSize: int.MaxValue);
            model.AvailableHAndL = dispatches
                .Where(x => x.DispatchType == (int)ShipmentDispatchType.HLShipment)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList();

            return View("~/Plugins/Misc.CountingSequence/Views/ShipmentTransit/_AddItem.cshtml", model);
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> AddItem(ShipmentTransitItemModel model)
        {
            if (model.items == null || !model.items.Any())
                return Json(new { success = false });

            var shipmentTransits = new List<ShipmentTransitItems>();

            foreach (var item in model.items)
            {
                if (item.NoOfPacks == 0 && item.NoOfUnits == 0)
                    return Json(new { success = false, message = "Enter NoOfPacks or NoOfUnits" });

                var product = await _productService.GetProductByIdAsync(item.ProductId);
                var maxCartQty = product?.OrderMaximumQuantity ?? 1;
                var quantity = (item.NoOfPacks * maxCartQty) + item.NoOfUnits;

                shipmentTransits.Add(new ShipmentTransitItems
                {
                    ShipmentTransitId = model.ShipmentTransitId,
                    PalletId = model.PalletId,
                    ProductId = item.ProductId,
                    NoOfUnits = item.NoOfUnits,
                    NoOfPacks = item.NoOfPacks,
                    ToDispatchId = item.HAndL,
                    Quantity = quantity
                });
            }

            await _shipmentTransitItemService.InsertShipmentTransitItemsAsync(shipmentTransits);

            return Json(new { success = true });
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> ShipmentTransitItemList(ShipmentTransitItemSearchModel searchModel)
        {
            var model = await _shipmentTransitModelFactory.PrepareShipmentTransitItemListModelAsync(searchModel);
            return Json(model);
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> DeleteShipmentTransitItem(int id)
        {
            var shipmentTransit = await _shipmentTransitItemService.GetShipmentTransitItemByIdAsync(id);

            if (shipmentTransit != null)
                await _shipmentTransitItemService.DeleteShipmentTransitItemAsync(shipmentTransit);

            return new NullJsonResult();
        }

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        [HttpPost]
        public async Task<IActionResult> ShipmentTransitPalletList(ShipmentTransitPalletSearchModel searchModel)
        {
            var model = await _shipmentTransitModelFactory.PrepareShipmentTransitPalletListModelAsync(searchModel);
            return Json(model);
        }

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        [HttpPost]
        public async Task<IActionResult> ShipmentTransitItemByPalletList(ShipmentTransitItemSearchModel searchModel)
        {
            var model = await _shipmentTransitModelFactory.PrepareShipmentTransitItemByPalletListModelAsync(searchModel);
            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> UpdateShipmentTransitItem(int id, int noOfPacks, int noOfUnits)
        {
            if (noOfPacks == 0 && noOfUnits == 0)
                return Json(new { success = false, message = "Enter NoOfPacks or NoOfUnits" });

            var shipmentTransititem = await _shipmentTransitItemService.GetShipmentTransitItemByIdAsync(id);
            if (shipmentTransititem == null)
                return Json(new { success = false });

            var product = await _productService.GetProductByIdAsync(shipmentTransititem.ProductId);
            var maxCartQty = product?.OrderMaximumQuantity ?? 1;

            shipmentTransititem.NoOfPacks = noOfPacks;
            shipmentTransititem.NoOfUnits = noOfUnits;
            shipmentTransititem.Quantity = (noOfPacks * maxCartQty) + noOfUnits;

            await _shipmentTransitItemService.UpdateShipmentTransitItemAsync(shipmentTransititem);

            return Json(new { success = true });
        }

        #endregion
    }
}
