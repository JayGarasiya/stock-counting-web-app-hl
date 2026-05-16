using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.CountingSequence.Domain;
using Nop.Plugin.Misc.CountingSequence.Factories;
using Nop.Plugin.Misc.CountingSequence.Infrastructure;
using Nop.Plugin.Misc.CountingSequence.Models.BackOrders;
using Nop.Plugin.Misc.CountingSequence.Services;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.CountingSequence.Controllers
{
    public class BackOrdersController : BaseAdminController
    {
        #region Fields

        protected readonly IBackOrdersService _backOrdersService;
        protected readonly IBackOrdersModelFactory _backOrdersModelFactory;
        protected readonly ILocalizationService _localizationService;
        protected readonly IProductService _productService;

        #endregion

        #region Ctor

        public BackOrdersController(
            IBackOrdersService backOrdersService,
            IBackOrdersModelFactory backOrdersModelFactory,
            ILocalizationService localizationService,
            IProductService productService)
        {

            _backOrdersService = backOrdersService;
            _backOrdersModelFactory = backOrdersModelFactory;
            _localizationService = localizationService;
            _productService = productService;
        }

        #endregion

        #region Methods

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> List()
        {
            var model = await _backOrdersModelFactory.PrepareBackOrdersSearchModelAsync(new BackOrdersSearchModel());
            return View("~/Plugins/Misc.CountingSequence/Views/BackOrders/List.cshtml", model);
        }

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        [HttpPost]
        public async Task<IActionResult> BackOrdersList(BackOrdersSearchModel searchModel)
        {
            var model = await _backOrdersModelFactory.PrepareBackOrdersListModelAsync(searchModel);
            return Json(model);
        }

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> Create()
        {
            var model = await _backOrdersModelFactory.PrepareBackOrdersModelAsync(new BackOrdersModel(), null);

            return PartialView("~/Plugins/Misc.CountingSequence/Views/BackOrders/_CreateOrUpdate.cshtml", model);
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> Create(BackOrdersModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await _backOrdersModelFactory.PrepareBackOrdersModelAsync(model, null);
                return PartialView("~/Plugins/Misc.CountingSequence/Views/BackOrders/_CreateOrUpdate.cshtml", model);
            }

            var backorder = new BackOrders
            {
                ReferenceNo = model.ReferenceNo,
                ChannelId = model.ChannelId,
                ProductId = model.ProductId,
                Quantity = model.Quantity,
                Status = model.Status,
                OrderDate = model.OrderDate.Date.Add(DateTime.Now.TimeOfDay),
            };
            await _backOrdersService.InsertBackOrdersAsync(backorder);

            return Json(new { success = true, message = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.BackOrder.CreateMessage") });
        }

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> Edit(int id)
        {
            var backOrder = await _backOrdersService.GetBackOrderByIdAsync(id);
            if (backOrder == null)
                return RedirectToAction("List");

            var model = await _backOrdersModelFactory.PrepareBackOrdersModelAsync(new BackOrdersModel(), backOrder);

            return PartialView("~/Plugins/Misc.CountingSequence/Views/BackOrders/_CreateOrUpdate.cshtml", model);
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> Edit(BackOrdersModel model)
        {
            var backOrder = await _backOrdersService.GetBackOrderByIdAsync(model.Id);

            if (!ModelState.IsValid)
            {
                model = await _backOrdersModelFactory.PrepareBackOrdersModelAsync(model, backOrder);
                return PartialView("~/Plugins/Misc.CountingSequence/Views/BackOrders/_CreateOrUpdate.cshtml", model);
            }

            if (backOrder.OrderDate != model.OrderDate)
                model.OrderDate = model.OrderDate.Date.Add(DateTime.Now.TimeOfDay);

            backOrder.ReferenceNo = model.ReferenceNo;
            backOrder.ChannelId = model.ChannelId;
            backOrder.ProductId = model.ProductId;
            backOrder.Quantity = model.Quantity;
            backOrder.Status = model.Status;
            backOrder.OrderDate = model.OrderDate;
            await _backOrdersService.UpdateBackOrdersAsync(backOrder);

            return Json(new { success = true, message = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.BackOrder.UpdateMessage") });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> Delete(int id)
        {
            var backOrder = await _backOrdersService.GetBackOrderByIdAsync(id);
            if (backOrder != null)
                await _backOrdersService.DeleteBackOrdersAsync(backOrder);

            return Json(new { success = true, message = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.BackOrder.DeleteMessage") });
        }

        #endregion
    }
}
