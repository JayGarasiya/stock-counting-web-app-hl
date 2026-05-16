using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.CountingSequence.Domain;
using Nop.Plugin.Misc.CountingSequence.Factories;
using Nop.Plugin.Misc.CountingSequence.Infrastructure;
using Nop.Plugin.Misc.CountingSequence.Models.Pallet;
using Nop.Plugin.Misc.CountingSequence.Services;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Models.Extensions;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.CountingSequence.Controllers
{
    public class PalletController : BaseAdminController
    {
        #region Fields

        protected readonly IPalletService _palletService;
        protected readonly IPalletModelFactory _palletModelFactory;
        protected readonly IProductService _productService;
        protected readonly ICategoryModelFactory _categoryModelFactory;
        protected readonly IPermissionService _permissionService;
        protected readonly ILocalizationService _localizationService;
        protected readonly INotificationService _notificationService;


        #endregion

        #region Ctor

        public PalletController(IPalletService palletService,
            IPalletModelFactory palletModelFactory,
            IProductService productService,
            ICategoryModelFactory categoryModelFactory,
            IPermissionService permissionService,
            ILocalizationService localizationService,
            INotificationService notificationService)
        {
            _palletService = palletService;
            _palletModelFactory = palletModelFactory;
            _productService = productService;
            _categoryModelFactory = categoryModelFactory;
            _permissionService = permissionService;
            _localizationService = localizationService;
            _notificationService = notificationService;
        }

        #endregion

        #region Methods

        #region Pallet

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> List()
        {
            var model = await _palletModelFactory.PreparePalletSearchModelAsync(new PalletSearchModel());
            return View("~/Plugins/Misc.CountingSequence/Views/Pallet/List.cshtml", model);
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> PalletList(PalletSearchModel searchModel)
        {
            var model = await _palletModelFactory.PreparePalletListModelAsync(searchModel);
            return Json(model);
        }

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> Create()
        {
            var model = await _palletModelFactory.PreparePalletModelAsync(new PalletModel(), null);

            return PartialView("~/Plugins/Misc.CountingSequence/Views/Pallet/Create.cshtml", model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> Create(PalletModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var pallet = new Pallet
                {
                    Name = model.Name,
                    ShipmentDispatchId = model.ShipmentDispatchId,
                    DisplayOrder = model.DisplayOrder,
                    IsVisible = model.IsVisible,
                    Description = model.Description,
                    SequenceOrder = model.SequenceOrder,
                };

                await _palletService.InsertPalletAsync(pallet);

                _notificationService.SuccessNotification
                    (await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Pallet.CreateMessage"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = pallet.Id });
            }

            model = await _palletModelFactory.PreparePalletModelAsync(model, null);
            return View("~/Plugins/Misc.CountingSequence/Views/Pallet/Create.cshtml", model);
        }

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _palletService.GetPalletByIdAsync(id);
            if (entity == null)
                return RedirectToAction("List");

            var model = await _palletModelFactory.PreparePalletModelAsync(null, entity);

            return PartialView("~/Plugins/Misc.CountingSequence/Views/Pallet/Edit.cshtml", model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> Edit(PalletModel model, bool continueEditing)
        {
            var pallet = await _palletService.GetPalletByIdAsync(model.Id);
            if (pallet == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                pallet.Name = model.Name;
                pallet.ShipmentDispatchId = model.ShipmentDispatchId;
                pallet.DisplayOrder = model.DisplayOrder;
                pallet.IsVisible = model.IsVisible;
                pallet.Description = model.Description;
                pallet.SequenceOrder = model.SequenceOrder;

                await _palletService.UpdatePalletAsync(pallet);

                _notificationService.SuccessNotification
                    (await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Pallet.UpdateMessage"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = pallet.Id });
            }

            model = await _palletModelFactory.PreparePalletModelAsync(model, pallet);
            return View("~/Plugins/Misc.CountingSequence/Views/Pallet/Edit.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> Delete(int id)
        {
            var pallet = await _palletService.GetPalletByIdAsync(id);
            if (pallet != null)
                await _palletService.DeletePalletAsync(pallet);

            _notificationService.SuccessNotification
                (await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Pallet.DeleteMessage"));

            return RedirectToAction("List");
        }

        #endregion

        #region Pallet Searching

        public virtual async Task<IActionResult> SearchPalletAutoComplete(string term)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermission.Security.ACCESS_ADMIN_PANEL))
                return Content(string.Empty);

            const int searchTermMinimumLength = 2;
            if (string.IsNullOrWhiteSpace(term) || term.Length < searchTermMinimumLength)
                return Content(string.Empty);

            const int pageSize = 10;

            // Always use pageIndex = 0
            var racks = await _palletService.GetAllPalletPagedAsync(showHidden: true, pageIndex: 0, pageSize: pageSize);

            var result = racks
                .Where(r => r.Name.Contains(term, StringComparison.OrdinalIgnoreCase))
                .Select(r => new
                {
                    label = r.Name,     
                    palletid = r.Id       
                }).ToList();

            return Json(result);
        }

        #endregion

        #region Pallet Product

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> PalletProductList(PalletProductSearchModel palletProductSearchModel)
        {
            palletProductSearchModel.SetGridPageSize();
            var model = await _palletModelFactory.PreparePalletProductListModelAsync(palletProductSearchModel);
            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> DeletePalletProduct(int id)
        {
            var palletProductEntity = await _palletService.GetPalletProductByIdAsync(id);
            if (palletProductEntity != null)
                await _palletService.DeletePalletProductAsync(palletProductEntity);

            return new NullJsonResult();
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> ProductSearchForPallet(AddProductToPalletModel searchModel)
        {
            var products = await _productService.SearchProductsAsync(
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize
            );

            var model = await new ProductListModel().PrepareToGridAsync(searchModel, products, () =>
            {
                return products.SelectAwait(async product => new ProductModel
                {
                    Id = product.Id,
                    Name = product.Name
                });
            });

            return Json(model);
        }

        [HttpGet]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public virtual async Task<IActionResult> PalletProductAddPopup(int palletId)
        {
            //prepare model
            var model = await _categoryModelFactory.PrepareAddProductToCategorySearchModelAsync(new AddProductToCategorySearchModel());

            return View("~/Plugins/Misc.CountingSequence/Views/Pallet/PalletProductAddPopup.cshtml", model);
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public virtual async Task<IActionResult> PalletProductAddPopupList(AddProductToCategorySearchModel searchModel)
        {
            //prepare model
            var model = await _categoryModelFactory.PrepareAddProductToCategoryListModelAsync(searchModel);

            return Json(model);
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public virtual async Task<IActionResult> PalletProductAddPopup(AddProductToPalletModel model)
        {
            //get selected products
            var selectedProducts = await _productService.GetProductsByIdsAsync(model.SelectedProductIds.ToArray());
            if (selectedProducts.Any())
            {
                foreach (var productId in model.SelectedProductIds)
                {
                    var exists = (await _palletService
                        .GetPalletProductsByIdAsync(model.PalletId))
                        .Any(x => x.ProductId == productId);

                    if (!exists)
                    {
                        await _palletService.InsertPalletProductAsync(new PalletProduct
                        {
                            PalletId = model.PalletId,
                            ProductId = productId
                        });
                    }
                }
            }

            ViewBag.RefreshPage = true;

            return View("~/Plugins/Misc.CountingSequence/Views/Pallet/PalletProductAddPopup.cshtml", new AddProductToCategorySearchModel());
        }

        #endregion

        #endregion
    }
}