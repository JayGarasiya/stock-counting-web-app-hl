using Nop.Plugin.Misc.CountingSequence.Domain;
using Nop.Plugin.Misc.CountingSequence.Models.BackOrders;

namespace Nop.Plugin.Misc.CountingSequence.Factories
{
    /// <summary>
    /// Back orders model factory interface
    /// </summary>
    public interface IBackOrdersModelFactory
    {
        /// <summary>
        /// Prepare back orders search model
        /// </summary>
        /// <param name="backOrdersSearchModel">Back orders search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the back orders search model
        /// </returns>

        Task<BackOrdersSearchModel> PrepareBackOrdersSearchModelAsync(BackOrdersSearchModel backOrdersSearchModel);

        /// <summary>
        /// Prepare paged back orders list model
        /// </summary>
        /// <param name="backOrdersSearchModel">Back orders search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the back orders list model
        /// </returns>
        Task<BackOrdersListModel> PrepareBackOrdersListModelAsync(BackOrdersSearchModel backOrdersSearchModel);

        /// <summary>
        /// Prepare back orders model
        /// </summary>
        /// <param name="backOrdersModel">Back orders model</param>
        /// <param name="backOrders">Back orders</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the back order model
        /// </returns>
        Task<BackOrdersModel> PrepareBackOrdersModelAsync(BackOrdersModel backOrdersModel, BackOrders backOrders);
    }
}
