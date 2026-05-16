using Nop.Plugin.Misc.CountingSequence.Domain;
using Nop.Plugin.Misc.CountingSequence.Models.Pallet;

namespace Nop.Plugin.Misc.CountingSequence.Factories
{
    /// <summary>
    /// Pallet model factory interface
    /// </summary>
    public interface IPalletModelFactory
    {

        /// <summary>
        /// Prepare pallet search model
        /// </summary>
        /// <param name="palletSearchModel">Pallet search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the pallet search model
        /// </returns>
        Task<PalletSearchModel> PreparePalletSearchModelAsync(PalletSearchModel palletSearchModel);

        /// <summary>
        /// Prepare paged pallet list model
        /// </summary>
        /// <param name="searchModel">Pallet search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the pallet list model
        /// </returns>
        Task<PalletListModel> PreparePalletListModelAsync(PalletSearchModel searchModel);

        /// <summary>
        /// Prepare pallet model
        /// </summary>
        /// <param name="model">Pallet model</param>
        /// <param name="pallet">Pallet </param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the pallet model
        /// </returns>
        Task<PalletModel> PreparePalletModelAsync(PalletModel model, Pallet pallet);


        //Pallete Product

        /// <summary>
        /// Prepare paged pallet product child list model
        /// </summary>
        /// <param name="palletProductSearchModel">Pallet product search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the pallet product child list model
        /// </returns>
        Task<PalletProductListModel> PreparePalletProductListModelAsync(PalletProductSearchModel palletProductSearchModel);
    }
}
