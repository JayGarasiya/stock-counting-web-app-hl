using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.CountingSequence.Domain;
using Nop.Plugin.Misc.CountingSequence.Factories;
using Nop.Plugin.Misc.CountingSequence.Infrastructure;
using Nop.Plugin.Misc.CountingSequence.Models.ShipmentDispatch;
using Nop.Plugin.Misc.CountingSequence.Services;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.CountingSequence.Controllers
{
    public class ShipmentDispatchController : BaseAdminController
    {
        #region Fields

        protected readonly IShipmentDispatchService _shipmentDispatchService;
        protected readonly IShipmentDispatchModelFactory _shipmentDispatchModelFactory;
        protected readonly ILocalizationService _localizationService;

        #endregion

        #region Ctor

        public ShipmentDispatchController(
            IShipmentDispatchService shipmentDispatchService,
            IShipmentDispatchModelFactory shipmentDispatchModelFactory,
            ILocalizationService localizationService)
        {
            _shipmentDispatchService = shipmentDispatchService;
            _shipmentDispatchModelFactory = shipmentDispatchModelFactory;
            _localizationService = localizationService;
        }

        #endregion

        #region Methods

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> List()
        {
            var model = await _shipmentDispatchModelFactory.PrepareShipmentDispatchSearchModelAsync(new ShipmentDispatchSearchModel());
            return View("~/Plugins/Misc.CountingSequence/Views/ShipmentDispatch/List.cshtml", model);
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> List(ShipmentDispatchSearchModel searchModel)
        {
            var model = await _shipmentDispatchModelFactory.PrepareShipmentDispatchListModelAsync(searchModel);
            return Json(model);
        }

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> Create()
        {
            var model = await _shipmentDispatchModelFactory.PrepareShipmentDispatchModelAsync(new ShipmentDispatchModel(), null);
            return View("~/Plugins/Misc.CountingSequence/Views/ShipmentDispatch/_CreateOrUpdate.cshtml", model);
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> Create(ShipmentDispatchModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await _shipmentDispatchModelFactory.PrepareShipmentDispatchModelAsync(model, null);
                return PartialView("~/Plugins/Misc.CountingSequence/Views/ShipmentDispatch/_CreateOrUpdate.cshtml", model);
            }

            await _shipmentDispatchService.InsertShipmentDispatchAsync(new ShipmentDispatches
            {
                Name = model.Name,
                DispatchType = model.DispatchType,
                ShippedMonth = model.ShippedMonth,
                ShippedYear = model.ShippedYear,
                DisplayOrder = model.DisplayOrder,
                Visible = model.Visible

            });

            return Json(new { success = true, message = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.ShipmentDispatch.CreateMessage") });
        }

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> Edit(int id)
        {
            var shipmentDispatch = await _shipmentDispatchService.GetShipmentDispatchByIdAsync(id);
            if (shipmentDispatch == null)
                return RedirectToAction("List");

            var model = await _shipmentDispatchModelFactory.PrepareShipmentDispatchModelAsync(new ShipmentDispatchModel(), shipmentDispatch);
            return View("~/Plugins/Misc.CountingSequence/Views/ShipmentDispatch/_CreateOrUpdate.cshtml", model);
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> Edit(ShipmentDispatchModel model)
        {
            var shipmentDispatch = await _shipmentDispatchService.GetShipmentDispatchByIdAsync(model.Id);

            if (!ModelState.IsValid)
            {
                model = await _shipmentDispatchModelFactory.PrepareShipmentDispatchModelAsync(model, shipmentDispatch);
                return PartialView("~/Plugins/Misc.CountingSequence/Views/ShipmentDispatch/_CreateOrUpdate.cshtml", model);
            }

            shipmentDispatch.Name = model.Name;
            shipmentDispatch.DispatchType = model.DispatchType;
            shipmentDispatch.ShippedMonth = model.ShippedMonth;
            shipmentDispatch.ShippedYear = model.ShippedYear;
            shipmentDispatch.DisplayOrder = model.DisplayOrder;
            shipmentDispatch.Visible = model.Visible;

            await _shipmentDispatchService.UpdateShipmentDispatchAsync(shipmentDispatch);

            return Json(new { success = true, message = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.ShipmentDispatch.UpdateMessage") });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> Delete(int id)
        {
            var shipmentDispatch = await _shipmentDispatchService.GetShipmentDispatchByIdAsync(id);
            if (shipmentDispatch != null)
                await _shipmentDispatchService.DeleteShipmentDispatchAsync(shipmentDispatch);

            return Json(new { success = true, message = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.ShipmentDispatch.DeleteMessage") });
        }

        #endregion
    }
}
