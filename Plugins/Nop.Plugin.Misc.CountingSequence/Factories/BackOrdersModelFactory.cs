using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Misc.CountingSequence.Domain;
using Nop.Plugin.Misc.CountingSequence.Models.BackOrders;
using Nop.Plugin.Misc.CountingSequence.Services;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Misc.CountingSequence.Factories
{
    /// <summary>
    /// Back orders model factory
    /// </summary>
    public class BackOrdersModelFactory : IBackOrdersModelFactory
    {
        #region Fields

        protected readonly IBackOrdersService _backOrdersService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IProductService _productService;
        protected readonly IChannelService _channelService;
        protected readonly ICountingSequenceService _countingSequenceService;

        #endregion

        #region Ctor

        public BackOrdersModelFactory(IBackOrdersService backOrdersService,
            ILocalizationService localizationService,
            IProductService productService,
            IChannelService channelService,
            ICountingSequenceService countingSequenceService)
        {
            _backOrdersService = backOrdersService;
            _productService = productService;
            _localizationService = localizationService;
            _channelService = channelService;
            _countingSequenceService = countingSequenceService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare back orders search model
        /// </summary>
        /// <param name="backOrdersSearchModel">Back orders search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the back orders Search Model
        /// </returns>
        public async Task<BackOrdersSearchModel> PrepareBackOrdersSearchModelAsync(BackOrdersSearchModel backOrdersSearchModel)
        {
            backOrdersSearchModel.AvailableStatus.Add(new SelectListItem
            {
                Text = "All",
                Value = "0", 
                Selected = backOrdersSearchModel.SearchSatus == 0
            });

            foreach (BackOrdersEnum item in Enum.GetValues(typeof(BackOrdersEnum)))
            {
                backOrdersSearchModel.AvailableStatus.Add(new SelectListItem
                {
                    Text = await _localizationService.GetLocalizedEnumAsync(item),
                    Value = ((int)item).ToString(),
                    Selected = (int)item == backOrdersSearchModel.SearchSatus
                });
            }
            backOrdersSearchModel.SetGridPageSize();
            return backOrdersSearchModel;
        }

        /// <summary>
        /// Prepare paged back orders list model
        /// </summary>
        /// <param name="backOrdersSearchModel">Back orders search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the back orders list model
        /// </returns>
        public async Task<BackOrdersListModel> PrepareBackOrdersListModelAsync(BackOrdersSearchModel backOrdersSearchModel)
        {
            var backOrders = await _backOrdersService.GetAllBackOrdersPagedAsync(
                referenceNumber : backOrdersSearchModel.SearchRefrenceNumber,
                channelId : backOrdersSearchModel.SearchChennelId,
                searchStatus : backOrdersSearchModel.SearchSatus,
                productId : backOrdersSearchModel.SearchProductId,
                startDate : backOrdersSearchModel.StartDate,
                endDate : backOrdersSearchModel.EndDate,
                minQuantity : backOrdersSearchModel.MinQuantity,
                maxQuantity : backOrdersSearchModel.MaxQuantity,
                pageIndex : backOrdersSearchModel.Page - 1,
                pageSize : backOrdersSearchModel.PageSize);

            var completedStockCount = (await _countingSequenceService.GetAllStockCountAsync(progressStatusId: ProgressStatus.Completed))
                .OrderByDescending(x => x.Id).FirstOrDefault();

            DateTime? completedDate = completedStockCount?.CreatedOnUtc;
            var backOrdersModel = await new BackOrdersListModel().PrepareToGridAsync(backOrdersSearchModel, backOrders, () =>
            {
                return backOrders.SelectAwait(async backOrder =>
                {
                    var model = new BackOrdersModel();
                    model.Id = backOrder.Id;
                    model.ReferenceNo = backOrder.ReferenceNo;
                    model.ChannelId = backOrder.ChannelId;
                    model.ProductId = backOrder.ProductId;
                    model.Quantity = backOrder.Quantity;
                    model.Status = backOrder.Status;
                    model.Statuss = ((BackOrdersEnum)backOrder.Status).ToString();
                    model.OrderDate = backOrder.OrderDate;
                    model.ProductName = (await _productService.GetProductByIdAsync(model.ProductId))?.Sku;
                    model.ChannelName = (await _channelService.GetChannelByIdAsync(model.ChannelId))?.Name;
                    model.CanEditDelete = !(completedDate.HasValue && backOrder.OrderDate <= completedDate.Value);
                    return model;
                });
            });
            return backOrdersModel;
        }

        /// <summary>
        /// Prepare back orders model
        /// </summary>
        /// <param name="backOrdersModel">back orders model</param>
        /// <param name="backOrders">back orders</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the back orders model
        /// </returns>
        public async Task<BackOrdersModel> PrepareBackOrdersModelAsync(BackOrdersModel backOrdersModel, BackOrders backOrders)
        {
            if (backOrders != null)
            {
                backOrdersModel ??= new BackOrdersModel();

                backOrdersModel.Id = backOrders.Id;
                backOrdersModel.ReferenceNo = backOrders.ReferenceNo;
                backOrdersModel.ChannelId = backOrders.ChannelId;
                backOrdersModel.ProductId = backOrders.ProductId;
                backOrdersModel.Quantity = backOrders.Quantity;
                backOrdersModel.Status = backOrders.Status;
                backOrdersModel.OrderDate = backOrders.OrderDate;
            }

            backOrdersModel.AvailableChannel = (await _channelService.GetAllChannelPagedAsync(pageIndex: 0, pageSize: int.MaxValue))
            .Select(channel => new SelectListItem
            {
                Text = channel.Name,
                Value = channel.Id.ToString(),
            }).ToList();

            backOrdersModel.AvailableProduct = (await _productService.SearchProductsAsync()).Select(product => new SelectListItem
            {
                Text = product.Sku,
                Value = product.Id.ToString(),
            }).ToList();

            backOrdersModel.AvailableStatus = new List<SelectListItem>();

            foreach (BackOrdersEnum item in Enum.GetValues(typeof(BackOrdersEnum)))
            {
                backOrdersModel.AvailableStatus.Add(new SelectListItem
                {
                    Text = await _localizationService.GetLocalizedEnumAsync(item),
                    Value = ((int)item).ToString(),
                    Selected = (int)item == backOrdersModel.Status
                });
            }

            return backOrdersModel;
        }
        #endregion
    }
}
