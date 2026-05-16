using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.CountingSequence.Factories;
using Nop.Plugin.Misc.CountingSequence.Infrastructure;
using Nop.Plugin.Misc.CountingSequence.Models.Stock;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.CountingSequence.Controllers
{
    public class StockController : BaseAdminController
    {
        #region Fields

        protected readonly IStockModelFactory _stockModelFactory;

        #endregion

        #region Ctor

        public StockController(IStockModelFactory stockModelFactory)
        {
            _stockModelFactory = stockModelFactory;
        }

        #endregion

        #region Methods

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> List()
        {
            var model = await _stockModelFactory.PrepareStockSearchModelAsync(new StockSearchModel());
            return View("~/Plugins/Misc.CountingSequence/Views/Stock/List.cshtml", model);
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> List(StockSearchModel searchModel)
        {
            var model = await _stockModelFactory.PrepareStockListModelAsync(searchModel);
            return Json(model);
        }

        #endregion
    }
}
