using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Misc.CountingSequence.Domain;
using Nop.Plugin.Misc.CountingSequence.Models.Pallet;
using Nop.Plugin.Misc.CountingSequence.Services;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Misc.CountingSequence.Factories
{
    /// <summary>
    /// Pallet model factory 
    /// </summary>
    public class PalletModelFactory : IPalletModelFactory
    {
        #region Fields
        protected readonly IPalletService _palletService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IProductService _productService;
        protected readonly IShipmentDispatchService _shipmentDispatchService;
        protected readonly ICategoryService _categoryService;
        protected readonly ISpecificationAttributeService _specificationAttributeService;
        #endregion

        #region Ctor
        public PalletModelFactory(IPalletService palletService,
            ILocalizationService localizationService,
            IProductService productService,
            IShipmentDispatchService shipmentDispatchService,
            ICategoryService categoryService,
            ISpecificationAttributeService specificationAttributeService)
        {
            _palletService = palletService;
            _localizationService = localizationService;
            _productService = productService;
            _shipmentDispatchService = shipmentDispatchService;
            _categoryService = categoryService;
            _specificationAttributeService = specificationAttributeService;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Prepare pallet search model
        /// </summary>
        /// <param name="palletSearchModel">Pallet search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the pallet search model
        /// </returns>
        public async Task<PalletSearchModel> PreparePalletSearchModelAsync(PalletSearchModel palletSearchModel)
        {
            palletSearchModel.AvailableVisibleOptions.Add(new SelectListItem
            {
                Value = "0",
                Text = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Search.All")
            });
            palletSearchModel.AvailableVisibleOptions.Add(new SelectListItem
            {
                Value = "1",
                Text = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Search.VisibleOnly")
            });
            palletSearchModel.AvailableVisibleOptions.Add(new SelectListItem
            {
                Value = "2",
                Text = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Search.UnvisibleOnly")
            });

            palletSearchModel.AvailableShipmentDispatch.Add(new SelectListItem
            {
                Text = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Search.All"),
                Value = "0"
            });

            foreach (var item in (await _shipmentDispatchService.GetAllShipementDispatchAsync()))
            {
                palletSearchModel.AvailableShipmentDispatch.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                });
            }
            palletSearchModel.SetGridPageSize();
            return palletSearchModel;
        }

        /// <summary>
        /// Prepare pallet product search model
        /// </summary>
        /// <param name="palletProductSearchModel">Pallet Product search model</param>
        /// <param name="pallet">Pallet</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the pallet product search model
        /// </returns>
        protected virtual PalletProductSearchModel PreparePalletProductSearchModelAsync(PalletProductSearchModel palletProductSearchModel, Pallet pallet)
        {
            ArgumentNullException.ThrowIfNull(palletProductSearchModel);

            ArgumentNullException.ThrowIfNull(pallet);

            palletProductSearchModel.PalletId = pallet.Id;


            palletProductSearchModel.SetGridPageSize();


            return palletProductSearchModel;
        }

        /// <summary>
        /// Prepare paged pallet list model
        /// </summary>
        /// <param name="searchModel">Pallet search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the pallet list model
        /// </returns>
        public async Task<PalletListModel> PreparePalletListModelAsync(PalletSearchModel searchModel)
        {
            var pallets = await _palletService.GetAllPalletPagedAsync(
                showHidden: true,
                name: searchModel.SearchPalletName,
                shipmentDispatchId: searchModel.SearchShipmentDispatchId > 0 ? searchModel.SearchShipmentDispatchId : null,
                visible: searchModel.SearchVisible == 0 ? null : (bool?)(searchModel.SearchVisible == 1),
                productId: searchModel.SearchProductId,
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize);

            var model = await new PalletListModel().PrepareToGridAsync(searchModel, pallets, () =>
            {
                return pallets.SelectAwait(async p => new PalletModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    ShipmentDispatchId = p.ShipmentDispatchId,
                    DisplayOrder = p.DisplayOrder,
                    IsVisible = p.IsVisible,
                    Description = p.Description,
                    ShipmentDispatchName = (await _shipmentDispatchService.GetShipmentDispatchByIdAsync(p.ShipmentDispatchId))?.Name ?? string.Empty,
                    SequenceOrder = p.SequenceOrder,

                });
            });

            return model;
        }

        /// <summary>
        /// Prepare pallet model
        /// </summary>
        /// <param name="model">Pallet model</param>
        /// <param name="pallet">Pallet</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the pallet model
        /// </returns>
        public async Task<PalletModel> PreparePalletModelAsync(PalletModel model, Pallet pallet)
        {
            if (pallet != null)
            {
                model ??= new PalletModel();

                model.Id = pallet.Id;
                model.Name = pallet.Name;
                model.ShipmentDispatchId = pallet.ShipmentDispatchId;
                model.DisplayOrder = pallet.DisplayOrder;
                model.IsVisible = pallet.IsVisible;
                model.Description = pallet.Description;
                model.SequenceOrder = pallet.SequenceOrder;

                //prepare nested search model
                PreparePalletProductSearchModelAsync(model.PalletProductSearchModel, pallet);
            }

            model.AvailableShipmentDispatch = (await _shipmentDispatchService.GetAllShipementDispatchAsync()).Select(x => new SelectListItem
            {
                Value = (x.Id).ToString(),
                Text = x.Name
            }).ToList();

            return model;
        }

        #region Pallet Product

        /// <summary>
        /// Prepare paged pallet product Child list model
        /// </summary>
        /// <param name="palletProductSearchModel">Pallet Product search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the pallet product child list model
        /// </returns>
        public async Task<PalletProductListModel> PreparePalletProductListModelAsync(PalletProductSearchModel palletProductSearchModel)
        {
            var palletProduct = await _palletService.GetAllPalletProductPageAsync(
                palletId: palletProductSearchModel.PalletId,
                pageIndex: palletProductSearchModel.Page - 1,
                pageSize: palletProductSearchModel.PageSize);

            var palletProductModel = await new PalletProductListModel().PrepareToGridAsync(palletProductSearchModel, palletProduct, () =>
            {
                return palletProduct.SelectAwait(async palletProduct =>
                {
                    var model = new PalletProductModel();
                    model.Id = palletProduct.Id;
                    model.PalletId = palletProduct.PalletId;
                    model.ProductId = palletProduct.ProductId;
                    model.ProductName = (await _productService.GetProductByIdAsync(palletProduct.ProductId))?.Name;

                    var productCategories = await _categoryService.GetProductCategoriesByProductIdAsync(palletProduct.ProductId);
                    var category = productCategories.FirstOrDefault();

                    string categoryName = null;

                    if (category != null)
                    {
                        var cat = await _categoryService.GetCategoryByIdAsync(category.CategoryId);
                        categoryName = cat?.Name;
                    }
                    model.CategoryName = categoryName;

                    var specs = await _specificationAttributeService.GetProductSpecificationAttributesAsync(model.ProductId);

                    var specList = new List<string>();

                    foreach (var spec in specs)
                    {
                        var option = await _specificationAttributeService
                            .GetSpecificationAttributeOptionByIdAsync(spec.SpecificationAttributeOptionId);

                        if (option != null)
                        {
                            var attribute = await _specificationAttributeService
                                .GetSpecificationAttributeByIdAsync(option.SpecificationAttributeId);

                            specList.Add($"{attribute?.Name}: {option.Name}");
                        }
                    }

                    model.Specification = string.Join(", ", specList);

                    return model;
                });
            });
            return palletProductModel;
        }
        #endregion

        #endregion
    }
}