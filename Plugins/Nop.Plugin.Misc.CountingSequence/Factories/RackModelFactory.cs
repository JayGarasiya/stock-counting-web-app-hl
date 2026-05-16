using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Misc.CountingSequence.Domain;
using Nop.Plugin.Misc.CountingSequence.Models.Rack;
using Nop.Plugin.Misc.CountingSequence.Services;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Misc.CountingSequence.Factories
{
    /// <summary>
    /// Rack model factory 
    /// </summary>
    public class RackModelFactory : IRackModelFactory
    {
        #region Fields

        protected readonly IRackService _rackService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IProductService _productService;
        protected readonly ICategoryService _categoryService;
        protected readonly ISpecificationAttributeService _specificationAttributeService;

        #endregion

        #region Ctor

        public RackModelFactory(IRackService rackService,
            ILocalizationService localizationService,
            IProductService productService,
            ICategoryService categoryService,
            ISpecificationAttributeService specificationAttributeService)
        {
            _rackService = rackService;
            _localizationService = localizationService;
            _productService = productService;
            _categoryService = categoryService;
            _specificationAttributeService = specificationAttributeService;
        }

        #endregion

        #region Methods

        #region Rack

        /// <summary>
        /// Prepare rack search model
        /// </summary>
        /// <param name="searchModel">Rack search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rack search model
        /// </returns>
        public async Task<RackSearchModel> PrepareRackSearchModelAsync(RackSearchModel searchModel)
        {
            //Visible
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

            //Function type
            searchModel.AvailableFunctionType.Add(new SelectListItem
            {
                Text = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Search.All"),
                Value = "0"
            });

            foreach (var item in Enum.GetValues(typeof(RackFunctionType)))
            {
                searchModel.AvailableFunctionType.Add(new SelectListItem
                {
                    Text = item.ToString(),
                    Value = ((int)item).ToString()
                });
            }

            // Level Type
            searchModel.AvailableLevelTypes.Add(new SelectListItem
            {
                Value = "0",
                Text = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Search.All")
            });

            var rackLevelTypes = await _rackService.GetAllRackLevelTypePagedAsync(
                showHidden: false);

            foreach (var item in rackLevelTypes)
            {
                searchModel.AvailableLevelTypes.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                });
            }

            searchModel.SetGridPageSize();
            return searchModel;
        }

        /// <summary>
        /// Prepare rack product search model
        /// </summary>
        /// <param name="rackProductSearchModel">Rack Product search model</param>
        /// <param name="rack">Rack model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rack product search model
        /// </returns>
        protected virtual RackProductSearchModel PrepareRackProductSearchModelAsync(RackProductSearchModel rackProductSearchModel, Rack rack)
        {
            ArgumentNullException.ThrowIfNull(rackProductSearchModel);

            ArgumentNullException.ThrowIfNull(rack);

            rackProductSearchModel.RackId = rack.Id;

            rackProductSearchModel.SetGridPageSize();

            return rackProductSearchModel;
        }

        /// <summary>
        /// Prepare paged rack list model
        /// </summary>
        /// <param name="searchModel">Rack search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rack list model
        /// </returns>
        public async Task<RackListModel> PrepareRackListModelAsync(RackSearchModel searchModel)
        {
            var racks = await _rackService.GetAllRackPagedAsync(
                showHidden: true,
                name: searchModel.SearchRackName,
                levelTypeId: searchModel.SearchLevelTypeId > 0 ? searchModel.SearchLevelTypeId : null,
                visible: searchModel.SearchVisible == 0 ? null : (bool?)(searchModel.SearchVisible == 1),
                productId: searchModel.SearchProductId,
                functionTypeId: searchModel.SearchFunctionTypeId,
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize);

            var model = await new RackListModel().PrepareToGridAsync(searchModel, racks, () =>
            {
                return racks.SelectAwait(async rack =>
                {
                    var levels = await _rackService.GetRackLevelsByRackIdAsync(rack.Id);

                    var levelNames = new List<string>();

                    foreach (var level in levels)
                    {
                        var rackLevelType = await _rackService.GetRackLevelTypeByIdAsync(level.LevelId);

                        if (rackLevelType != null)
                            levelNames.Add(rackLevelType.Name);
                    }

                    return new RackModel
                    {
                        Id = rack.Id,
                        Name = rack.Name,
                        Description = rack.Description,
                        SequenceOrder = rack.SequenceOrder,
                        DisplayOrder = rack.DisplayOrder,
                        IsVisible = rack.IsVisible,
                        FunctionTypeName = await _localizationService.GetLocalizedEnumAsync((RackFunctionType)rack.FunctionTypeId),
                        LevelTypeName = string.Join(", ", levelNames),
                    };
                });
            });

            return model;
        }


        /// <summary>
        /// Prepare rack model
        /// </summary>
        /// <param name="model">Rack model</param>
        /// <param name="rack">Rack entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the Rack model
        /// </returns>

        public async Task<RackModel> PrepareRackModelAsync(RackModel model, Rack rack)
        {
            if (model == null)
                model = new RackModel();

            if (rack != null)
            {
                model.Id = rack.Id;
                model.Name = rack.Name;
                model.Description = rack.Description;
                model.SequenceOrder = rack.SequenceOrder;
                model.DisplayOrder = rack.DisplayOrder;
                model.IsVisible = rack.IsVisible;
                model.FunctionTypeId = rack.FunctionTypeId;
                model.FunctionTypeName = await _localizationService.GetLocalizedEnumAsync((RackFunctionType)rack.FunctionTypeId);

                var levels = await _rackService.GetRackLevelsByRackIdAsync(rack.Id);
                model.LevelTypeIds = levels.Select(x => x.LevelId).ToList();

                //prepare nested search model
                PrepareRackProductSearchModelAsync(model.RackProductSearchModel, rack);
            }

            model.AvailableFunctionType = await Enum.GetValues(typeof(RackFunctionType))
               .Cast<RackFunctionType>()
               .SelectAwait(async x => new SelectListItem
               {
                   Value = ((int)x).ToString(),
                   Text = await _localizationService.GetLocalizedEnumAsync(x)
               }).ToListAsync();

            // Rack Level Type From DB
            var rackLevelTypes = await _rackService.GetAllRackLevelTypePagedAsync(
                showHidden: false,
                functionTypeId: model.FunctionTypeId > 0 ? model.FunctionTypeId : null);

            model.AvailableLevelType = rackLevelTypes
                
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList();


            return model;
        }

        #endregion

        #region Rack Product

        /// <summary>
        /// Prepare child paged rack product child list model
        /// </summary>
        /// <param name="rackProductSearchModel">Rack Product search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rack product child list model
        /// </returns>
        public async Task<RackProductListModel> PrepareRackProductListModelAsync(RackProductSearchModel rackProductSearchModel)
        {
            var rackProduct = await _rackService.GetAllRackProductPageAsync(
               rackId: rackProductSearchModel.RackId,
               levelId: rackProductSearchModel.LevelId,
               pageIndex: rackProductSearchModel.Page - 1,
               pageSize: rackProductSearchModel.PageSize);

            var rackProductModel = await new RackProductListModel().PrepareToGridAsync(rackProductSearchModel, rackProduct, () =>
            {
                return rackProduct.SelectAwait(async rackProduct =>
                {
                    var model = new RackProductModel();
                    model.Id = rackProduct.Id;
                    model.RackId = rackProduct.RackId;
                    model.ProductId = rackProduct.ProductId;
                    model.ProductPositionId = rackProduct.ProductPositionId;
                    model.ProductPositionName = await _localizationService.GetLocalizedEnumAsync((RackProductPosition)rackProduct.ProductPositionId);
                    model.ProductName = (await _productService.GetProductByIdAsync(rackProduct.ProductId))?.Name;

                    var productCategories = await _categoryService.GetProductCategoriesByProductIdAsync(rackProduct.ProductId);
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

                            specList.Add($"{attribute.Name} : {option.Name}");
                        }
                    }

                    model.Specification = string.Join(", ", specList);

                    return model;
                });
            });
            return rackProductModel;
        }

        #endregion

        #region Rack Level Type

        /// <summary>
        /// Prepare rack model
        /// </summary>
        /// <param name="rackLevelTypeModel">Rack Level Type model</param>
        /// <param name="rackLevelType">Rack Level Type entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the Rack level type model
        /// </returns>
        public async Task<RackLevelTypeModel> PrepareRackLevelTypeModelAsync(RackLevelTypeModel rackLevelTypeModel, RackLevelType rackLevelType)
        {
            if (rackLevelTypeModel == null)
                rackLevelTypeModel = new RackLevelTypeModel();

            if (rackLevelType != null)
            {
                rackLevelTypeModel.Id = rackLevelType.Id;
                rackLevelTypeModel.Name = rackLevelType.Name;
                rackLevelTypeModel.FunctionTypeId = rackLevelType.FunctionTypeId;
                rackLevelTypeModel.FunctionTypeName = await _localizationService.GetLocalizedEnumAsync((RackFunctionType)rackLevelType.FunctionTypeId);
                rackLevelTypeModel.DisplayOrder = rackLevelType.DisplayOrder;
                rackLevelTypeModel.IsVisible = rackLevelType.IsVisible;
            }

            rackLevelTypeModel.AvailableFunctionType = await Enum.GetValues(typeof(RackFunctionType))
                .Cast<RackFunctionType>()
                .SelectAwait(async x => new SelectListItem()
                {
                    Value = ((int)x).ToString(),
                    Text = await _localizationService.GetLocalizedEnumAsync(x)
                }).ToListAsync();

            return rackLevelTypeModel;
        }

        /// <summary>
        /// Prepare rack search model
        /// </summary>
        /// <param name="rackLevelTypeSearch">Rack search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rack search model
        /// </returns>
        public async Task<RackLevelTypeSearchModel> PrepareRackLevelTypeSearchModelAsync(RackLevelTypeSearchModel rackLevelTypeSearch)
        {
            rackLevelTypeSearch.AvailableFunctionType.Add(new SelectListItem
            {
                Value = "0",
                Text = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Search.All")
            });

            foreach (var item in Enum.GetValues(typeof(RackFunctionType)))
            {
                rackLevelTypeSearch.AvailableFunctionType.Add(new SelectListItem
                {
                    Text = await _localizationService.GetLocalizedEnumAsync((RackFunctionType)item),
                    Value = ((int)item).ToString()
                });
            }

            rackLevelTypeSearch.SetGridPageSize();
            return rackLevelTypeSearch;
        }


        /// <summary>
        /// Prepare paged rack level type list model
        /// </summary>
        /// <param name="searchRackLevelModel">Rack level type search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rack list model
        /// </returns>
        public async Task<RackLevelTypeListModel> PrepareRackLevelTypeListModelAsync(RackLevelTypeSearchModel searchRackLevelModel)
        {
            if (searchRackLevelModel == null)
                throw new ArgumentNullException(nameof(searchRackLevelModel));

            // Get paged data
            var rackLevelTypes = await _rackService.GetAllRackLevelTypePagedAsync(
                showHidden: true,
                name: searchRackLevelModel.SearchRackLevelName,
                functionTypeId: searchRackLevelModel.SearchFunctionTypeId > 0 ? searchRackLevelModel.SearchFunctionTypeId : null,
                pageIndex: searchRackLevelModel.Page - 1,
                pageSize: searchRackLevelModel.PageSize);

            // Prepare list model
            var model = await new RackLevelTypeListModel().PrepareToGridAsync(searchRackLevelModel, rackLevelTypes, () =>
            {
                return rackLevelTypes.SelectAwait(async x =>
                {
                    var rackLevelTypeModel = new RackLevelTypeModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        FunctionTypeId = x.FunctionTypeId,
                        FunctionTypeName = await _localizationService.GetLocalizedEnumAsync((RackFunctionType)x.FunctionTypeId),
                        DisplayOrder = x.DisplayOrder,
                        IsVisible = x.IsVisible
                    };

                    return rackLevelTypeModel;
                });
            });

            return model;
        }

        #endregion

        #endregion
    }
}
