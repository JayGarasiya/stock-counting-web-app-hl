using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.CountingSequence.Factories;
using Nop.Plugin.Misc.CountingSequence.Infrastructure;
using Nop.Plugin.Misc.CountingSequence.Models.History;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.CountingSequence.Controllers
{
    public class HistoryController : BaseAdminController
    {
        #region Fields

        protected readonly IHistoryModelFactory _historyModelFactory;

        #endregion

        #region Ctor

        public HistoryController(IHistoryModelFactory historyModelFactory)
        {
            _historyModelFactory = historyModelFactory;
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
            var model = await _historyModelFactory.PrepareHistorySearchModelAsync(new HistorySearchModel());
            return View("~/Plugins/Misc.CountingSequence/Views/History/List.cshtml", model);
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> List(HistorySearchModel searchModel)
        {
            var model = await _historyModelFactory.PrepareHistoryListModelAsync(searchModel);
            return Json(model);
        }

        #endregion
    }
}
