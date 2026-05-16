using Nop.Plugin.Misc.CountingSequence.Domain;
using Nop.Plugin.Misc.CountingSequence.Models.Rack;

namespace Nop.Plugin.Misc.CountingSequence.Factories
{
    /// <summary>
    /// Rack model factory interface
    /// </summary>
    public interface IRackModelFactory
    {
        #region Rack

        /// <summary>
        /// Prepare rack search model
        /// </summary>
        /// <param name="searchModel">Rack search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rack search model
        /// </returns>
        Task<RackSearchModel> PrepareRackSearchModelAsync(RackSearchModel searchModel);

        /// <summary>
        /// Prepare paged rack list model
        /// </summary>
        /// <param name="searchModel">Rack search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the Rack list model
        /// </returns>
        Task<RackListModel> PrepareRackListModelAsync(RackSearchModel searchModel);

        /// <summary>
        /// Prepare Rack model
        /// </summary>
        /// <param name="model">Rack model</param>
        /// <param name="rack">Rack entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the Rack model
        /// </returns>
        Task<RackModel> PrepareRackModelAsync(RackModel model, Rack rack);

        #endregion

        #region Rack Product 

        /// <summary>
        /// Prepare child paged rack list model
        /// </summary>
        /// <param name="searchModel">Rack Product search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the Rack product list model
        /// </returns>
        Task<RackProductListModel> PrepareRackProductListModelAsync(RackProductSearchModel searchModel);

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
        Task<RackLevelTypeModel> PrepareRackLevelTypeModelAsync(RackLevelTypeModel rackLevelTypeModel, RackLevelType rackLevelType);

        /// <summary>
        /// Prepare rack search model
        /// </summary>
        /// <param name="rackLevelTypeSearch">Rack search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rack search model
        /// </returns>
        Task<RackLevelTypeSearchModel> PrepareRackLevelTypeSearchModelAsync(RackLevelTypeSearchModel rackLevelTypeSearch);

        /// <summary>
        /// Prepare paged rack level type list model
        /// </summary>
        /// <param name="searchRackLevelModel">Rack level type search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rack list model
        /// </returns>
        Task<RackLevelTypeListModel> PrepareRackLevelTypeListModelAsync(RackLevelTypeSearchModel searchRackLevelModel);

        #endregion
    }
}
