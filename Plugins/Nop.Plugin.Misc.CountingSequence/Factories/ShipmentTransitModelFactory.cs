using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Data;
using Nop.Plugin.Misc.CountingSequence.Domain;
using Nop.Plugin.Misc.CountingSequence.Models.ShipmentTransit;
using Nop.Plugin.Misc.CountingSequence.Models.ShipmentTransitItem;
using Nop.Plugin.Misc.CountingSequence.Services;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Misc.CountingSequence.Factories
{
    /// <summary>
    /// Shipment transit model factory 
    /// </summary>
    public class ShipmentTransitModelFactory : IShipmentTransitModelFactory
    {
        #region Fields

        protected readonly IShipmentTransitService _service;
        protected readonly IShipmentDispatchService _dispatchService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IShipmentTransitItemService _shipmentTransitItemService;
        protected readonly IPalletService _palletService;
        protected readonly IProductService _productService;
        protected readonly IRepository<ShipmentTransit> _shipmentTramsitRepository;
        protected readonly IRepository<ShipmentDispatches> _shipmentDispatchesRepository;
        protected readonly ISpecificationAttributeService _specificationAttributeService;
        protected readonly ICategoryService _categoryService;

        #endregion

        #region Ctor

        public ShipmentTransitModelFactory(
            IShipmentTransitService service,
            IShipmentDispatchService dispatchService,
            ILocalizationService localizationService,
            IShipmentTransitItemService shipmentTransitItemService,
            IPalletService palletService,
            IProductService productService,
            IRepository<ShipmentTransit> shipmentTramsitRepository,
            IRepository<ShipmentDispatches> shipmentDispatchesRepository,
            ISpecificationAttributeService specificationAttributeService,
            ICategoryService categoryService)
        {
            _service = service;
            _dispatchService = dispatchService;
            _localizationService = localizationService;
            _shipmentTransitItemService = shipmentTransitItemService;
            _palletService = palletService;
            _productService = productService;
            _shipmentTramsitRepository = shipmentTramsitRepository;
            _shipmentDispatchesRepository = shipmentDispatchesRepository;
            _specificationAttributeService = specificationAttributeService;
            _categoryService = categoryService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare shipment transit search model
        /// </summary>
        /// <param name="searchModel">Shipment transit search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the shipment transit search model
        /// </returns>
        public async Task<ShipmentTransitSearchModel> PrepareShipmentTransitSearchModelAsync(ShipmentTransitSearchModel searchModel)
        {
            searchModel ??= new ShipmentTransitSearchModel();
            var shipmentDispatches = await _shipmentDispatchesRepository.Table.ToListAsync();
            searchModel.AvailableFromDisptch = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "All",
                    Value = "0",
                    Selected = searchModel.SearchFromDisptchId == 0
                }
            };

            foreach (var x in shipmentDispatches
                .Where(x => x.DispatchType == (int)ShipmentDispatchType.USShipment))
            {
                searchModel.AvailableFromDisptch.Add(new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                    Selected = x.Id == searchModel.SearchFromDisptchId
                });
            }

            // Add "All" option
            searchModel.AvailableStatus.Add(new SelectListItem
            {
                Text = "All",
                Value = "0",
                Selected = searchModel.SearchStatus == 0
            });

            // Add enum values
            var statuses = await Enum.GetValues(typeof(ShipmentTransitStatus))
                .Cast<ShipmentTransitStatus>()
                .SelectAwait(async x => new SelectListItem
                {
                    Value = ((int)x).ToString(),
                    Text = await _localizationService.GetLocalizedEnumAsync(x),
                    Selected = (int)x == searchModel.SearchStatus
                })
                .ToListAsync();

            searchModel.AvailableStatus.AddRange(statuses);
            searchModel.SetGridPageSize();
            return searchModel;
        }

        /// <summary>
        /// Prepare pallet product search model
        /// </summary>
        /// <param name="shipmentTransitPalletSearchModel">Shipment Transit pallet search model</param>
        /// <param name="shipmentTransit">Shipment Transit model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the Shipment Transit pallet search model
        /// </returns>
        protected virtual ShipmentTransitPalletSearchModel PrepareShipmentTransitPalletSearchModelAsync(ShipmentTransitPalletSearchModel shipmentTransitPalletSearchModel, ShipmentTransit shipmentTransit)
        {
            ArgumentNullException.ThrowIfNull(shipmentTransitPalletSearchModel);

            ArgumentNullException.ThrowIfNull(shipmentTransit);

            shipmentTransitPalletSearchModel.ShipmentTransitId = shipmentTransit.Id;


            shipmentTransitPalletSearchModel.SetGridPageSize();


            return shipmentTransitPalletSearchModel;
        }

        /// <summary>
        /// Prepare shipment transit list model
        /// </summary>
        /// <param name="searchModel">Shipment transit search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the shipment transit list model
        /// </returns>
        public async Task<ShipmentTransitListModel> PrepareShipmentTransitListModelAsync(ShipmentTransitSearchModel searchModel)
        {
            var entities = await _service.GetShipmentTransitAllAsync(
                showHidden: true,
                fromDisptchId: searchModel.SearchFromDisptchId,
                status: searchModel.SearchStatus,
                productId: searchModel.SearchProductId,
                palletId: searchModel.SearchPalletId,
                pageIndex: searchModel.Page - 1,
                searchModel.PageSize);

            var dispatches = await _dispatchService.GetAllShipementDispatchAsync(pageIndex: 0, pageSize: int.MaxValue);

            var model = await new ShipmentTransitListModel().PrepareToGridAsync(searchModel, entities, () =>
                {
                    return entities.SelectAwait(async x =>
                    {
                        var fromDispatch = dispatches.FirstOrDefault(d => d.Id == x.FromDispatchId);

                        var statusEnum = (ShipmentTransitStatus)x.Status;

                        return new ShipmentTransitModel
                        {
                            Id = x.Id,
                            FromDispatchId = x.FromDispatchId,
                            FromDispatchName = fromDispatch?.Name,
                            Status = x.Status,
                            StatusName = statusEnum.ToString()

                        };
                    });
                });

            return model;
        }

        /// <summary>
        /// Prepare shipment transit model
        /// </summary>
        /// <param name="model">Shipment transit model</param>
        /// <param name="shipmentTransit">Shipment transit </param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the shipment transit model
        /// </returns>
        public async Task<ShipmentTransitModel> PrepareShipmentTransitModelAsync(ShipmentTransitModel model, ShipmentTransit shipmentTransit)
        {
            if (shipmentTransit != null)
            {
                model ??= new ShipmentTransitModel();
                model.Id = shipmentTransit.Id;
                model.FromDispatchId = shipmentTransit.FromDispatchId;
                model.Status = shipmentTransit.Status;

                //prepare nested search model
                PrepareShipmentTransitPalletSearchModelAsync(model.ShipmentTransitPalletSearchModel, shipmentTransit);
            }

            var dispatches = await _dispatchService.GetAllShipementDispatchAsync(pageIndex: 0, pageSize: int.MaxValue);

            model.AvailableFromDispatches = dispatches.Where(x => x.DispatchType == (int)ShipmentDispatchType.USShipment)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList();

            model.AvailableToDispatches = dispatches.Where(x => x.DispatchType == (int)ShipmentDispatchType.HLShipment)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList();

            model.AvailableStatuses = await Enum.GetValues(typeof(ShipmentTransitStatus))
                .Cast<ShipmentTransitStatus>()
                .SelectAwait(async x => new SelectListItem
                {
                    Value = ((int)x).ToString(),
                    Text = await _localizationService.GetLocalizedEnumAsync(x)
                }).ToListAsync();

            return model;
        }

        /// <summary>
        /// Prepare shipment transit item list model
        /// </summary>
        /// <param name="searchModel">Shipment transit item search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the shipment transit item list model
        /// </returns>
        public async Task<ShipmentTransitItemListModel> PrepareShipmentTransitItemListModelAsync(ShipmentTransitItemSearchModel searchModel)
        {
            var items = await _shipmentTransitItemService.GetShipmentTransitItemsByShipmentTransitIdAsync(searchModel.ShipmentTransitId);

            var model = await new ShipmentTransitItemListModel().PrepareToGridAsync(searchModel, items, () =>
            {
                return items.SelectAwait(async item =>
                {
                    var product = await _productService.GetProductByIdAsync(item.ProductId);
                    var pallet = await _palletService.GetPalletByIdAsync(item.PalletId);

                    return new ShipmentTransitItemModel
                    {
                        Id = item.Id,
                        ShipmentTransitId = item.ShipmentTransitId,
                        ProductName = product?.Name,
                        PalletName = pallet?.Name,
                        Quantity = item.Quantity,
                        NoOfUnits = item.NoOfUnits,
                        NoOfPacks = item.NoOfPacks,
                        ToDispatchId = item.ToDispatchId
                    };
                });
            });

            return model;
        }

        /// <summary>
        /// Prepare shipment transit pallet list model
        /// </summary>
        /// <param name="searchModel">Shipment transit pallet search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the shipment transit pallet list model
        /// </returns>
        public async Task<ShipmentTransitPalletListModel> PrepareShipmentTransitPalletListModelAsync(ShipmentTransitPalletSearchModel searchModel)
        {
            ArgumentNullException.ThrowIfNull(searchModel);

            searchModel.SetGridPageSize();

            var items = await _shipmentTransitItemService.GetShipmentTransitItemsByShipmentTransitIdAsync(searchModel.ShipmentTransitId);
            var grouped = items.GroupBy(x => x.PalletId).ToList();
            var palletIds = grouped.Select(x => x.Key).ToArray();
            var pallets = await _palletService.GetPalletsByIdsAsync(palletIds);
            var palletDict = pallets.ToDictionary(x => x.Id, x => x.Name);

            var data = grouped.ToPagedList(searchModel);

            var model = await new ShipmentTransitPalletListModel().PrepareToGridAsync(searchModel, data, () =>
            {
                return grouped.Select(g => new ShipmentTransitPalletModel
                {
                    PalletId = g.Key,
                    PalletName = palletDict.TryGetValue(g.Key, out var name) ? name : string.Empty
                }).ToAsyncEnumerable();
            });

            return model;
        }
        /// <summary>
        /// Prepare shipment transit item by pallet list model
        /// </summary>
        /// <param name="searchModel">Shipment transit item by pallet search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the shipment transit item by pallet list model
        /// </returns>
        public async Task<ShipmentTransitItemListModel> PrepareShipmentTransitItemByPalletListModelAsync(ShipmentTransitItemSearchModel searchModel)
        {
            ArgumentNullException.ThrowIfNull(searchModel);

            searchModel.SetGridPageSize();

            var items = await _shipmentTransitItemService.GetShipmentTransitItemsByShipmentTransitIdAsync(searchModel.ShipmentTransitId);
            var filtered = items.Where(x => x.PalletId == searchModel.PalletId).ToList();
            var paged = new PagedList<ShipmentTransitItems>(filtered, searchModel.Page - 1, searchModel.PageSize);
            var dispatches = await _dispatchService.GetAllShipementDispatchAsync(pageIndex: 0, pageSize: int.MaxValue);
            var model = await new ShipmentTransitItemListModel().PrepareToGridAsync(searchModel, paged, () =>
            {
                return paged.SelectAwait(async item =>
                {
                    var product = await _productService.GetProductByIdAsync(item.ProductId);
                    var productCategories = await _categoryService.GetProductCategoriesByProductIdAsync(item.ProductId);
                    var category = productCategories.FirstOrDefault();

                    string categoryName = null;

                    if (category != null)
                    {
                        var cat = await _categoryService.GetCategoryByIdAsync(category.CategoryId);
                        categoryName = cat?.Name;
                    }

                    var specs = await _specificationAttributeService.GetProductSpecificationAttributesAsync(item.ProductId);
                    var specList = new List<string>();

                    foreach (var spec in specs)
                    {
                        var option = await _specificationAttributeService.GetSpecificationAttributeOptionByIdAsync(spec.SpecificationAttributeOptionId);

                        if (option != null)
                        {
                            var attribute = await _specificationAttributeService.GetSpecificationAttributeByIdAsync(option.SpecificationAttributeId);

                            if (attribute != null)
                                specList.Add($"{attribute.Name} : {option.Name}");
                        }
                    }
                    
                    var toDispatchName = dispatches.FirstOrDefault(d => d.Id == item.ToDispatchId);

                    return new ShipmentTransitItemModel
                    {
                        Id = item.Id,
                        ProductName = product?.Name,
                        Quantity = item.Quantity,
                        NoOfUnits = item.NoOfUnits,
                        NoOfPacks = item.NoOfPacks,
                        ToDispatchId = item.ToDispatchId,
                        ToDispatchName = toDispatchName?.Name ?? "-",
                        CategoryId = item.ProductId,
                        CategoryName = categoryName,
                        Specifications = string.Join(", ", specList)
                    };
                });
            });

            return model;
        }

        #endregion
    }
}