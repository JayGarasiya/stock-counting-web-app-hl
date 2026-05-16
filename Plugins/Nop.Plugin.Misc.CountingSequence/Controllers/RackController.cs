using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Misc.CountingSequence.Domain;
using Nop.Plugin.Misc.CountingSequence.Factories;
using Nop.Plugin.Misc.CountingSequence.Infrastructure;
using Nop.Plugin.Misc.CountingSequence.Models.BackOrders;
using Nop.Plugin.Misc.CountingSequence.Models.Rack;
using Nop.Plugin.Misc.CountingSequence.Services;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Models.Extensions;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;
using System.Linq;

namespace Nop.Plugin.Misc.CountingSequence.Controllers
{
    public class RackController : BaseAdminController
    {
        #region Fields

        protected readonly IRackService _rackService;
        protected readonly IRackModelFactory _rackModelFactory;
        protected readonly IProductService _productService;
        protected readonly ICategoryModelFactory _categoryModelFactory;
        protected readonly INotificationService _notificationService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IPermissionService _permissionService;

        #endregion

        #region Ctor

        public RackController(
            IRackService rackService,
            IRackModelFactory rackModelFactory,
            IProductService productService,
            ICategoryModelFactory categoryModelFactory,
            INotificationService notificationService,
            ILocalizationService localizationService,
            IPermissionService permissionService)
        {
            _rackService = rackService;
            _rackModelFactory = rackModelFactory;
            _productService = productService;
            _categoryModelFactory = categoryModelFactory;
            _notificationService = notificationService;
            _localizationService = localizationService;
            _permissionService = permissionService;
        }

        #endregion

        #region Methods

        #region Rack

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        [HttpGet]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> List()
        {
            var model = await _rackModelFactory.PrepareRackSearchModelAsync(new RackSearchModel());
            return View("~/Plugins/Misc.CountingSequence/Views/Rack/List.cshtml", model);
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> RackList(RackSearchModel rackSearchModel)
        {
            var model = await _rackModelFactory.PrepareRackListModelAsync(rackSearchModel);
            return Json(model);
        }

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public virtual async Task<IActionResult> Create()
        {
            var model = await _rackModelFactory.PrepareRackModelAsync(new RackModel(), null);
            return View("~/Plugins/Misc.CountingSequence/Views/Rack/Create.cshtml", model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public virtual async Task<IActionResult> Create(RackModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var rack = new Rack
                {
                    Name = model.Name,
                    Description = model.Description,
                    SequenceOrder = model.SequenceOrder,
                    DisplayOrder = model.DisplayOrder,
                    IsVisible = model.IsVisible,
                    FunctionTypeId = model.FunctionTypeId,
                };

                await _rackService.InsertRackAsync(rack);

                // Levels
                if (model.LevelTypeIds != null)
                {
                    foreach (var levelId in model.LevelTypeIds)
                    {
                        await _rackService.InsertRackLevelAsync(new RackLevel
                        {
                            RackId = rack.Id,
                            LevelId = levelId
                        });
                    }
                }
                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Rack.CreateMessage"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = rack.Id });
            }

            model = await _rackModelFactory.PrepareRackModelAsync(model, null);
            return View("~/Plugins/Misc.CountingSequence/Views/Rack/Create.cshtml", model);
        }

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public virtual async Task<IActionResult> Edit(int id)
        {
            var rack = await _rackService.GetRackByIdAsync(id);
            if (rack == null)
                return RedirectToAction("List");

            var model = await _rackModelFactory.PrepareRackModelAsync(null, rack);
            return View("~/Plugins/Misc.CountingSequence/Views/Rack/Edit.cshtml", model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public virtual async Task<IActionResult> Edit(RackModel model, bool continueEditing)
        {
            var rack = await _rackService.GetRackByIdAsync(model.Id);
            if (rack == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                rack.Name = model.Name;
                rack.Description = model.Description;
                rack.SequenceOrder = model.SequenceOrder;
                rack.DisplayOrder = model.DisplayOrder;
                rack.IsVisible = model.IsVisible;
                rack.FunctionTypeId = model.FunctionTypeId;

                await _rackService.UpdateRackAsync(rack);

                var existing = await _rackService.GetRackLevelsByRackIdAsync(rack.Id);

                foreach (var e in existing)
                {
                    if (model.LevelTypeIds == null || !model.LevelTypeIds.Contains(e.LevelId))
                        await _rackService.DeleteRackLevelAsync(e);
                }

                if (model.LevelTypeIds != null)
                {
                    foreach (var levelId in model.LevelTypeIds)
                    {
                        if (!existing.Any(x => x.LevelId == levelId))
                        {
                            await _rackService.InsertRackLevelAsync(new RackLevel
                            {
                                RackId = rack.Id,
                                LevelId = levelId
                            });
                        }
                    }
                }
                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Rack.UpdateMessage"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = rack.Id });
            }

            model = await _rackModelFactory.PrepareRackModelAsync(model, rack);
            return View("~/Plugins/Misc.CountingSequence/Views/Rack/Edit.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> Delete(int id)
        {
            var rack = await _rackService.GetRackByIdAsync(id);
            if (rack != null)
                await _rackService.DeleteRackAsync(rack);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Rack.DeleteMessage"));

            return RedirectToAction("List");
        }

        #endregion

        #region Rack Product

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> RackProductList(RackProductSearchModel rackProductSearchModel)
        {
            rackProductSearchModel.SetGridPageSize();
            var model = await _rackModelFactory.PrepareRackProductListModelAsync(rackProductSearchModel);

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> DeleteRackProduct(int id)
        {
            var rackProduct = await _rackService.GetRackProductByIdAsync(id);
            if (rackProduct != null)
                await _rackService.DeleteRackProductAsync(rackProduct);

            return new NullJsonResult();
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> ProductSearchForRack(AddProductToRackModel searchModel)
        {

            var products = await _productService.SearchProductsAsync(
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize
            );

            var model = await new ProductListModel().PrepareToGridAsync(searchModel, products, () =>
            {
                return products.SelectAwait(async p => new ProductModel
                {
                    Id = p.Id,
                    Name = p.Name
                });
            });

            return Json(model);
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> RackProductAddPopupList(AddProductToCategorySearchModel searchModel)
        {
            var model = await _categoryModelFactory
                .PrepareAddProductToCategoryListModelAsync(searchModel);

            return Json(model);
        }

        [HttpGet]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> RackProductAddPopup(int rackId, int levelId, int position)
        {
            var product = await _categoryModelFactory
        .PrepareAddProductToCategorySearchModelAsync(new AddProductToCategorySearchModel());

            // Build the AddProductToRackModel with enum populated
            var addModel = new AddProductToRackModel
            {
                RackId = rackId,
                LevelId = levelId,
                ProductPositionId = position,
                AvailableProductPositions = Enum.GetValues(typeof(RackProductPosition))
                    .Cast<RackProductPosition>()
                    .Select(x => new SelectListItem
                    {
                        Value = ((int)x).ToString(),
                        Text = x.ToString()
                    }).ToList()
            };

            ViewBag.AddModel = addModel;

            return View("~/Plugins/Misc.CountingSequence/Views/Rack/RackProductAddPopup.cshtml", product);
        }

        [HttpPost]
        [FormValueRequired("save")]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> RackProductAddPopup(AddProductToRackModel model)
        {
            if (model.SelectedProductIds.Any())
            {
                var existing = await _rackService.GetProductByRackIdAsync(model.RackId);
                foreach (var productId in model.SelectedProductIds)
                {
                    var positionId = 0;

                    var key = $"ProductPositions[{productId}]";

                    if (Request.Form.ContainsKey(key))
                        int.TryParse(Request.Form[key], out positionId);

                    var alreadyExists = existing.Any(x => x.ProductId == productId && x.ProductPositionId == positionId && x.RackLevelId == model.LevelId);

                    if (!alreadyExists)
                    {
                        await _rackService.InsertRackProductAsync(new RackProduct
                        {
                            RackId = model.RackId,
                            RackLevelId = model.LevelId,
                            ProductId = productId,
                            ProductPositionId = positionId
                        });
                    }
                }
            }

            ViewBag.RefreshPage = true;

            return View("~/Plugins/Misc.CountingSequence/Views/Rack/RackProductAddPopup.cshtml", new AddProductToCategorySearchModel());
        }

        [HttpPost]
        public async Task<IActionResult> RackLevelList(LevelSearchModel searchModel)
        {
            searchModel.SetGridPageSize();

            var levels = (await _rackService.GetRackLevelsByRackIdAsync(searchModel.RackId)).ToPagedList(searchModel);

            var model = await new LevelListModel().PrepareToGridAsync(searchModel, levels, () =>
            {
                return levels.SelectAwait(async x =>
                {
                    var rackLevelType = await _rackService.GetRackLevelTypeByIdAsync(x.LevelId);

                    return new LevelModel
                    {
                        Id = x.Id,
                        RackId = x.RackId,
                        LevelId = x.LevelId,
                        Name = rackLevelType?.Name
                    };
                });
            });

            return Json(model);
        }

        #endregion

        #region Rack Searching & Sku

        public virtual async Task<IActionResult> SearchRackAutoComplete(string term)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermission.Security.ACCESS_ADMIN_PANEL))
                return Content(string.Empty);

            const int searchTermMinimumLength = 2;
            if (string.IsNullOrWhiteSpace(term) || term.Length < searchTermMinimumLength)
                return Content(string.Empty);

            const int pageSize = 10;

            // Always use pageIndex = 0
            var racks = await _rackService.GetAllRackPagedAsync(showHidden: true, pageIndex: 0, pageSize: pageSize);

            var result = racks
                .Where(r => r.Name.Contains(term, StringComparison.OrdinalIgnoreCase))
                .Select(r => new
                {
                    label = r.Name,
                    rackid = r.Id
                }).ToList();

            return Json(result);
        }

        public virtual async Task<IActionResult> SearchProductSku(string term)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermission.Security.ACCESS_ADMIN_PANEL))
                return Content(string.Empty);

            const int searchTermMinimumLength = 2;
            if (string.IsNullOrWhiteSpace(term) || term.Length < searchTermMinimumLength)
                return Content(string.Empty);

            //products 
            const int productNumber = 15;
            var products = await _productService.SearchProductsAsync(0,
                keywords: term,
                pageSize: productNumber,
                showHidden: true);

            var result = (from p in products
                          select new
                          {
                              label = p.Sku,
                              productid = p.Id
                          }).ToList();

            return Json(result);
        }

        #endregion

        #region Rack Level Type

        [HttpGet]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> LevelList()
        {
            var model = await _rackModelFactory.PrepareRackLevelTypeSearchModelAsync(new RackLevelTypeSearchModel());

            return View("~/Plugins/Misc.CountingSequence/Views/Rack/RackLevelTypeList.cshtml", model);
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> RackLevelTypeList(RackLevelTypeSearchModel searchModel)
        {
            var model = await _rackModelFactory.PrepareRackLevelTypeListModelAsync(searchModel);

            return Json(model);
        }

        #region Create

        [HttpGet]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> RackLevelCreate()
        {
            var model = await _rackModelFactory.PrepareRackLevelTypeModelAsync(new RackLevelTypeModel(), null);

            return PartialView("~/Plugins/Misc.CountingSequence/Views/Rack/_CreateOrUpdate.RackLevelType.cshtml", model);
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> RackLevelCreate(RackLevelTypeModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await _rackModelFactory.PrepareRackLevelTypeModelAsync(model, null);

                return PartialView("~/Plugins/Misc.CountingSequence/Views/Rack/_CreateOrUpdate.RackLevelType.cshtml", model);
            }

            var rackLevelType = new RackLevelType
            {
                Name = model.Name,
                FunctionTypeId = model.FunctionTypeId,
                DisplayOrder = model.DisplayOrder,
                IsVisible = model.IsVisible
            };

            await _rackService.InsertRackLevelTypeAsync(rackLevelType);

            return Json(new
            {
                success = true,
                message = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.RackLevelType.CreateMessage")
            });
        }

        #endregion

        #region Edit

        [HttpGet]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> RackLevelEdit(int id)
        {
            var rackLevelType = await _rackService.GetRackLevelTypeByIdAsync(id);

            if (rackLevelType == null)
                return RedirectToAction("LevelList");

            var model = await _rackModelFactory.PrepareRackLevelTypeModelAsync(new RackLevelTypeModel(), rackLevelType);

            return PartialView("~/Plugins/Misc.CountingSequence/Views/Rack/_CreateOrUpdate.RackLevelType.cshtml", model);
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> RackLevelEdit(RackLevelTypeModel model)
        {
            var rackLevelType = await _rackService.GetRackLevelTypeByIdAsync(model.Id);

            if (rackLevelType == null)
                return Json(new
                {
                    success = false,
                    message = "Rack level type not found"
                });

            if (!ModelState.IsValid)
            {
                model = await _rackModelFactory.PrepareRackLevelTypeModelAsync(model, rackLevelType);

                return PartialView("~/Plugins/Misc.CountingSequence/Views/Rack/_CreateOrUpdate.RackLevelType.cshtml", model);
            }

            rackLevelType.Name = model.Name;
            rackLevelType.FunctionTypeId = model.FunctionTypeId;
            rackLevelType.DisplayOrder = model.DisplayOrder;
            rackLevelType.IsVisible = model.IsVisible;

            await _rackService.UpdateRackLevelTypeAsync(rackLevelType);

            return Json(new
            {
                success = true,
                message = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.RackLevelType.UpdateMessage")
            });
        }

        #endregion

        #region Delete

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> RackLevelDelete(int id)
        {
            var rackLevelType = await _rackService.GetRackLevelTypeByIdAsync(id);

            if (rackLevelType != null)
                await _rackService.DeleteRackLevelTypeAsync(rackLevelType);

            return Json(new
            {
                success = true,
                message = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.RackLevelType.DeleteMessage")
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetLevelTypesByFunctionType(int functionTypeId)
        {
            var rackLevelTypes = await _rackService.GetAllRackLevelTypePagedAsync(
                showHidden: false,
                functionTypeId: functionTypeId > 0 ? functionTypeId : null);

            var result = rackLevelTypes.Select(x => new
            {
                value = x.Id.ToString(),
                text = x.Name
            });

            return Json(result);
        }

        #endregion

        #endregion

        #endregion


    }
}
