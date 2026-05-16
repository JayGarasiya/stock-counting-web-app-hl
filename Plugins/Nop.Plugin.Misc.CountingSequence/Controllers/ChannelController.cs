using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.CountingSequence.Domain;
using Nop.Plugin.Misc.CountingSequence.Factories;
using Nop.Plugin.Misc.CountingSequence.Infrastructure;
using Nop.Plugin.Misc.CountingSequence.Models.Channel;
using Nop.Plugin.Misc.CountingSequence.Services;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.CountingSequence.Controllers
{
    public class ChannelController : BaseAdminController
    {
        #region Fields

        protected readonly IChannelService _channelService;
        protected readonly IChannelModelFactory _channelModelFactory;
        protected readonly IPermissionService _permissionService;
        protected readonly INotificationService _notificationService;
        protected readonly ILocalizationService _localizationService;

        #endregion

        #region Ctor
        public ChannelController(
            IChannelService channelService,
            IChannelModelFactory channelModelFactory,
            IPermissionService permissionService,
            INotificationService notificationService,
            ILocalizationService localizationService)
        {
            _channelService = channelService;
            _channelModelFactory = channelModelFactory;
            _permissionService = permissionService;
            _notificationService = notificationService;
            _localizationService = localizationService;
        }

        #endregion

        #region Methods

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> List()
        {
            var model = await _channelModelFactory.PrepareChannelSearchModelAsync(new ChannelSearchModel());
            return View("~/Plugins/Misc.CountingSequence/Views/Channel/List.cshtml", model);
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> List(ChannelSearchModel searchModel)
        {
            var model = await _channelModelFactory.PrepareChannelListModelAsync(searchModel);
            return Json(model);
        }

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> Create()
        {
            var model = await _channelModelFactory.PrepareChannelModelAsync(new ChannelModel(), null);
            return View("~/Plugins/Misc.CountingSequence/Views/Channel/_CreateOrUpdate.cshtml", model);
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> Create(ChannelModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await _channelModelFactory.PrepareChannelModelAsync(model, null);
                return PartialView("~/Plugins/Misc.CountingSequence/Views/Channel/_CreateOrUpdate.cshtml", model);
            }

            await _channelService.InsertChannelAsync(new Channel
            {
                Name = model.Name,
                ChannelId = model.ChannelId,
                Description = model.Description,
                DisplayOrder = model.DisplayOrder,
                Visible = model.Visible

            });

            return Json(new { success = true, message = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Channel.CreateMessage") });
        }

        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> Edit(int id)
        {
            var channel = await _channelService.GetChannelByIdAsync(id);
            if (channel == null)
                return RedirectToAction("List");

            var model = await _channelModelFactory.PrepareChannelModelAsync(new ChannelModel(), channel);
            return View("~/Plugins/Misc.CountingSequence/Views/Channel/_CreateOrUpdate.cshtml", model);
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> Edit(ChannelModel model)
        {
            var channel = await _channelService.GetChannelByIdAsync(model.Id);

            if (!ModelState.IsValid)
            {
                model = await _channelModelFactory.PrepareChannelModelAsync(model, channel);
                return PartialView("~/Plugins/Misc.CountingSequence/Views/Channel/_CreateOrUpdate.cshtml", model);
            }

            channel.Name = model.Name;
            channel.ChannelId = model.ChannelId;
            channel.Description = model.Description;
            channel.DisplayOrder = model.DisplayOrder;
            channel.Visible = model.Visible;

            await _channelService.UpdateChannelAsync(channel);

            return Json(new { success = true, message = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Channel.UpdateMessage") });
        }

        [HttpPost]
        [CheckPermission(CountingSequencePermissionConfigManager.COUNTING_SEQUENCE_TABLIST)]
        public async Task<IActionResult> Delete(int id)
        {
            var channel = await _channelService.GetChannelByIdAsync(id);

            if (channel != null)
                await _channelService.DeleteChannelAsync(channel);

            return Json(new { success = true, message = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Channel.DeleteMessage") });
        }

        public virtual async Task<IActionResult> SearchChannelAutoComplete(string term)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermission.Security.ACCESS_ADMIN_PANEL))
                return Content(string.Empty);

            const int searchTermMinimumLength = 2;
            if (string.IsNullOrWhiteSpace(term) || term.Length < searchTermMinimumLength)
                return Content(string.Empty);

            const int pageSize = 10;

            var racks = await _channelService.GetAllChannelPagedAsync(showHidden: true, pageIndex: 0, pageSize: pageSize);

            var result = racks
                .Where(r => r.Name.Contains(term, StringComparison.OrdinalIgnoreCase))
                .Select(r => new
                {
                    label = r.Name,
                    channelid = r.Id
                }).ToList();

            return Json(result);
        }

        #endregion
    }
}