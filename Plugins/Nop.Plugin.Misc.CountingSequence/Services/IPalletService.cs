using Nop.Core;
using Nop.Plugin.Misc.CountingSequence.Domain;

namespace Nop.Plugin.Misc.CountingSequence.Services
{
    /// <summary>
    /// Pallet service interface
    /// </summary>
    public interface IPalletService
    {
        #region Pallet

        /// <summary>
        /// Gets a pallet by identifier
        /// </summary>
        /// <param name="id">Pallet identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the pallet
        /// </returns>
        Task<Pallet> GetPalletByIdAsync(int id);

        /// <summary>
        /// Inserts a pallet
        /// </summary>
        /// <param name="pallet">Pallet</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertPalletAsync(Pallet pallet);

        /// <summary>
        /// Updates the pallet
        /// </summary>
        /// <param name="pallet">Pallet</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdatePalletAsync(Pallet pallet);

        /// <summary>
        /// Deletes the pallet
        /// </summary>
        /// <param name="pallet">Pallet</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeletePalletAsync(Pallet pallet);

        /// <summary>
        /// Get all pallet entries
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">Show Hidden</param>
        /// <param name="name">Pallet Name</param>
        /// <param name="shipmentDispatchId">Shipment Dispatch identifier</param>
        /// <param name="visible">Visible</param>
        /// <param name="productId">Product</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the paged list of entity entries
        /// </returns>
        Task<IPagedList<Pallet>> GetAllPalletPagedAsync(
            bool showHidden = false,
            string name = null,
            int? shipmentDispatchId = null,
            bool? visible = null,
            int productId = 0,
            int pageIndex = 0,
            int pageSize = int.MaxValue);

        /// <summary>
        /// Get Pallet by identifiers
        /// </summary>
        /// <param name="ids">Pallets identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the products
        /// </returns>
        Task<IList<Pallet>> GetPalletsByIdsAsync(int[] ids);

        /// <summary>
        /// Get Pallet By Product identifiers
        /// </summary>
        /// <param name="productId">Product identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the pallets
        /// </returns>
        Task<IList<Pallet>> GetPalletByProductIdAsync(int productId);

        /// <summary>
        /// Gets a pallet product by identifier
        /// </summary>
        /// <param name="palletId">Pallet identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the pallet product
        /// </returns>
        Task<IList<PalletProduct>> GetProductByPalletIdAsync(int palletId);

        /// <summary>
        /// Get Pallet by shipment dispatch Ids
        /// </summary>
        /// <param name="shipmentDispatchIds">Pallets shipment dispatch Ids</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the products
        /// </returns>
        Task<IList<Pallet>> GetPalletByShipmentDispatchIdsAsync(int[] shipmentDispatchIds);


        #endregion

        #region Pallet Product

        /// <summary>
        /// Gets a pallet product by identifier
        /// </summary>
        /// <param name="id">Pallet Product identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the pallet product
        /// </returns>
        Task<PalletProduct> GetPalletProductByIdAsync(int id);

        /// <summary>
        /// Inserts a pallet product
        /// </summary>
        /// <param name="palletProduct">Pallet Product</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertPalletProductAsync(PalletProduct palletProduct);

        /// <summary>
        /// Deletes the pallet product
        /// </summary>
        /// <param name="palletProduct">Pallet Product</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeletePalletProductAsync(PalletProduct palletProduct);

        /// <summary>
        /// Gets a pallet product by identifier
        /// </summary>
        /// <param name="palletProductId">Pallet identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the pallet product list
        /// </returns>
        Task<IList<PalletProduct>> GetPalletProductsByIdAsync(int palletProductId);

        /// <summary>
        /// Get all pallet product entries
        /// </summary>
        /// <param name="palletId">Pallet indentifier</param>
        /// <param name="productId">Product indentifier</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the paged list of entity entries
        /// </returns>
        Task<IPagedList<PalletProduct>> GetAllPalletProductPageAsync(int palletId = 0, int productId = 0, int pageIndex = 0, int pageSize = int.MaxValue);

        #endregion
    }
}
