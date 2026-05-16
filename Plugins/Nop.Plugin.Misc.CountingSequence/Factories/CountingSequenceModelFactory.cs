using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Misc.CountingSequence.Domain;
using Nop.Plugin.Misc.CountingSequence.Models.CountingSequence;
using Nop.Plugin.Misc.CountingSequence.Models.Pallet;
using Nop.Plugin.Misc.CountingSequence.Services;
using Nop.Services;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Shipping;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Misc.CountingSequence.Factories
{
    /// <summary>
    /// Prepare counting sequence model factory
    /// </summary>
    public class CountingSequenceModelFactory : ICountingSequenceModelFactory
    {
        #region Fields

        protected readonly ICountingSequenceService _countingSequenceService;
        protected readonly ICustomerService _customerService;
        protected readonly IWarehouseService _warehouseService;
        protected readonly ILocalizationService _localizationService;

        #endregion

        #region Ctor

        public CountingSequenceModelFactory(
            ICustomerService customerService,
            IWarehouseService warehouseService,
            ICountingSequenceService countingSequenceService,
            ILocalizationService localizationService)
        {
            _customerService = customerService;
            _warehouseService = warehouseService;
            _countingSequenceService = countingSequenceService;
            _localizationService = localizationService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare counting sequence search model
        /// </summary>
        /// <param name="searchModel">Pallet search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the pallet search model
        /// </returns>
        public async Task<StockCountSearchModel> PrepareCountingSequenceSearchModelAsync(StockCountSearchModel searchModel)
        {
            ArgumentNullException.ThrowIfNull(searchModel);

            searchModel.AvailableWarehouse = (await _warehouseService.GetAllWarehousesAsync()).Select(warehouse => new SelectListItem
            {
                Text = warehouse.Name,
                Value = warehouse.Id.ToString()
            }).ToList();
            searchModel.AvailableWarehouse.Insert(0, new SelectListItem { Text = "All", Value = "0" });

            searchModel.AvailableCountType.Insert(0, new SelectListItem { Text = "All", Value = "0" });
            searchModel.AvailableCountType.Insert(1, new SelectListItem { Text = "Rack", Value = "1" });
            searchModel.AvailableCountType.Insert(2, new SelectListItem { Text = "Pallet", Value = "2" });

            foreach (var item in Enum.GetValues(typeof(ProgressStatus)))
            {
                searchModel.AvailableProgressStatusType.Add(new SelectListItem
                {
                    Text = item.ToString(),
                    Value = ((int)item).ToString()
                });
            }
            searchModel.AvailableProgressStatusType.Insert(0, new SelectListItem { Text = "All", Value = "0" });

            searchModel.SetGridPageSize();
            return searchModel;
        }

        /// <summary>
        /// Prepare counting sequence list model
        /// </summary>
        /// <param name="searchModel">Pallet search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the pallet list model
        /// </returns>
        public async Task<StockCountListModel> PrepareCountingSequenceListModelAsync(StockCountSearchModel searchModel)
        {
            var stockCounts = await _countingSequenceService.GetAllStockCountAsync(name: searchModel.SearchName,
                warehouseId: searchModel.SearchWarehouseId,
                countTypeId: searchModel.SearchCountTypeId,
                progressStatusId: searchModel.SearchProgressStatusTypeId > 0 ? (ProgressStatus)searchModel.SearchProgressStatusTypeId : null,
                createdFromUtc: searchModel.SearchFromDate,
                createdToUtc: searchModel.SearchToDate,
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize);

            var model = await new StockCountListModel().PrepareToGridAsync(searchModel, stockCounts, () =>
            {
                return stockCounts.SelectAwait(async entity =>
                {
                    var countType = "-";
                    var customer = await _customerService.GetCustomerByIdAsync(entity.CutsomerId);
                    var warehouse = await _warehouseService.GetWarehouseByIdAsync(entity.WarehouseId);
                    var stockItem = (await _countingSequenceService.GetAllStockCountItem(stockCountId: entity.Id)).FirstOrDefault();
                    if (stockItem != null)
                    {
                        if (stockItem.PalletId > 0)
                            countType = "Pallet";
                        else if (stockItem.RackId > 0)
                            countType = "Rack";
                    }
                    else
                    {
                        var lastStockItem = (await _countingSequenceService.GetAllStockCountItem()).LastOrDefault();
                        if (lastStockItem != null && lastStockItem.PalletId > 0)
                            countType = "Rack";
                        else
                            countType = "Pallet";
                    }

                    return new StockCountModel
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        CustomerId = entity.CutsomerId,
                        CustomerName = customer?.Email,
                        WarehouseId = entity.WarehouseId,
                        WarehouseName = warehouse?.Name,
                        CountType = countType,
                        ProgressStatusId = entity.ProgressStatusId,
                        ProgressStatusName = await _localizationService.GetLocalizedEnumAsync((ProgressStatus)entity.ProgressStatusId),
                        CreatedOnUtc = entity.CreatedOnUtc.ToString("MM/dd/yyyy")
                    };
                });
            });

            return model;
        }

        #endregion
    }
}
