using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Misc.CountingSequence.Domain;
using Nop.Plugin.Misc.CountingSequence.Models.Channel;
using Nop.Plugin.Misc.CountingSequence.Services;
using Nop.Services.Localization;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Misc.CountingSequence.Factories
{
    /// <summary>
    /// Channel model factory 
    /// </summary>
    public class ChannelModelFactory : IChannelModelFactory
    {
        #region Fields

        protected readonly IChannelService _channelService;
        protected readonly ILocalizationService _localizationService;

        #endregion

        #region Ctor

        public ChannelModelFactory(IChannelService channelService, 
            ILocalizationService localizationService)
        {
            _channelService = channelService;
            _localizationService = localizationService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare channel search model
        /// </summary>
        /// <param name="searchModel">Channel search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the channel search model
        /// </returns>
        public async Task<ChannelSearchModel> PrepareChannelSearchModelAsync(ChannelSearchModel searchModel)
        {
            searchModel ??= new ChannelSearchModel();

            searchModel.AvailableVisibleOptions.Add(new SelectListItem
            {
                Value = "0",
                Text = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Search.All")
            });
            searchModel.AvailableVisibleOptions.Add(new SelectListItem
            {
                Value = "1",
                Text = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Search.VisibleOnly")
            });
            searchModel.AvailableVisibleOptions.Add(new SelectListItem
            {
                Value = "2",
                Text = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Search.UnvisibleOnly")
            });

            //Channel Type dropdown
            searchModel.AvailableChannelTypes.Add(new SelectListItem
            {
                Text = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Search.All"),
                Value = "0"
            });

            foreach (var item in Enum.GetValues(typeof(ChannelType)))
            {
                searchModel.AvailableChannelTypes.Add(new SelectListItem
                {
                    Text = item.ToString(),
                    Value = ((int)item).ToString()
                });
            }

            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare channel list model
        /// </summary>
        /// <param name="searchModel">Channel search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the channel list model
        /// </returns>
        public async Task<ChannelListModel> PrepareChannelListModelAsync(ChannelSearchModel searchModel)
        {
            var channels = await _channelService.GetAllChannelPagedAsync(
                showHidden: true,
                channelName: searchModel.SearchChannelName,
                visible: searchModel.SearchVisibleId == 0 ? null : (bool?)(searchModel.SearchVisibleId == 1),
                channelTypeId: searchModel.SearchChannelTypeId == 0 ? null : (int?)searchModel.SearchChannelTypeId,
                pageIndex: searchModel.Page - 1, 
                pageSize: searchModel.PageSize);

            var model = await new ChannelListModel().PrepareToGridAsync(searchModel, channels, () =>
            {
                return channels.Select(channel =>
                {
                    var enumValue = (ChannelType)channel.ChannelId;

                    return new ChannelModel
                    {
                        Id = channel.Id,
                        Name = channel.Name,
                        ChannelId = channel.ChannelId,
                        ChannelTypeName = enumValue.ToString(),
                        Description = channel.Description,
                        DisplayOrder = channel.DisplayOrder,
                        Visible = channel.Visible
                    };
                }).ToAsyncEnumerable();
            });

            return model;
        }

        /// <summary>
        /// Prepare channel model
        /// </summary>
        /// <param name="model">Channel model</param>
        /// <param name="channel">Channel entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the channel model
        /// </returns>
        public async Task<ChannelModel> PrepareChannelModelAsync(ChannelModel model, Channel channel)
        {
            if (channel != null)
            {
                model ??= new ChannelModel();

                model.Id = channel.Id;
                model.Name = channel.Name;
                model.ChannelId = channel.ChannelId;
                model.Description = channel.Description;
                model.DisplayOrder = channel.DisplayOrder;
                model.Visible = channel.Visible;
            }

            model.AvailableChannelTypes = await Enum.GetValues(typeof(ChannelType))
                .Cast<ChannelType>()
                .SelectAwait( async x => new SelectListItem
                {
                    Value = ((int)x).ToString(),
                    Text = await _localizationService.GetLocalizedEnumAsync(x)
                }).ToListAsync();

            return model;
        }

        #endregion
    }
}
