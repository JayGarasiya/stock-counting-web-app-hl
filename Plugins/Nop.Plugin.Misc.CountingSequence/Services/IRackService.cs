using Nop.Core;
using Nop.Plugin.Misc.CountingSequence.Domain;

namespace Nop.Plugin.Misc.CountingSequence.Services
{
    /// <summary>
    /// Rack service interface
    /// </summary>
    public interface IRackService
    {
        #region Rack

        /// <summary>
        /// Gets a rack by identifier
        /// </summary>
        /// <param name="id">Rack identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rack
        /// </returns>
        Task<Rack> GetRackByIdAsync(int id);
        Task<RackLevel> GetRackLevelByIdAsync(int id);

        /// <summary>
        /// Gets a rack level by identifier
        /// </summary>
        /// <param name="rackId">Rack Level identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rack level
        /// </returns>
        Task<RackLevel> GetRackLevelByRackIdAsync(int rackId);

        /// <summary>
        /// Inserts a rack
        /// </summary>
        /// <param name="rack">Rack</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertRackAsync(Rack rack);

        /// <summary>
        /// Inserts a rack level
        /// </summary>
        /// <param name="rackLevel">Rack Level</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertRackLevelAsync(RackLevel rackLevel);

        /// <summary>
        /// Updates the rack
        /// </summary>
        /// <param name="rack">Rack</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateRackAsync(Rack rack);

        /// <summary>
        /// Deletes the rack
        /// </summary>
        /// <param name="rack">Rack</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteRackAsync(Rack rack);

        /// <summary>
        /// Deletes the rack level
        /// </summary>
        /// <param name="rackLevel">Rack Level</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteRackLevelAsync(RackLevel rackLevel);

        /// <summary>
        /// Get all rack entries
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">Show Hidden</param>
        /// <param name="name">Rack Name</param>
        /// <param name="levelTypeId">Level type identifier</param>
        /// <param name="visible">Visible</param>
        /// <param name="productId">Product</param>
        /// <param name="functionTypeId">Function type identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the paged list of entity entries
        /// </returns>
        Task<IPagedList<Rack>> GetAllRackPagedAsync(bool showHidden = false,
            string name = null,
            int? levelTypeId = null,
            bool? visible = null,
            int productId = 0,
            int? functionTypeId = null,
            int pageIndex = 0,
            int pageSize = int.MaxValue);

        /// <summary>
        /// Get Rack by identifiers
        /// </summary>
        /// <param name="Ids">Rack identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the products
        /// </returns>
        Task<IList<Rack>> GetRacksByIdsAsync(int[] ids);

        /// <summary>
        /// Get Rack Product by identifiers
        /// </summary>
        /// <param name="productId">Rack Product identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the products
        /// </returns>
        Task<IList<Rack>> GetRackByProductIdAsync(int productId);

        #endregion

        #region Rack Product

        /// <summary>
        /// Gets a rack product by identifier
        /// </summary>
        /// <param name="id">Rack Product identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rack product
        /// </returns>
        Task<RackProduct> GetRackProductByIdAsync(int id);

        /// <summary>
        /// Inserts a rack product
        /// </summary>
        /// <param name="rackProduct">Rack Product</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertRackProductAsync(RackProduct rackProduct);

        /// <summary>
        /// Deletes the rack product
        /// </summary>
        /// <param name="rackProduct">Rack Product</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteRackProductAsync(RackProduct rackProduct);

        /// <summary>
        /// Get all rack product entries
        /// </summary>
        /// <param name="rackId">Rack indentifier</param>
        /// <param name="productId">Product indentifier</param>
        /// <param name="levelId">Level indentifier</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the paged list of entity entries
        /// </returns>
        Task<IPagedList<RackProduct>> GetAllRackProductPageAsync(
            int rackId = 0,
            int productId = 0,
            int levelId = 0,
            int pageIndex = 0,
            int pageSize = int.MaxValue);

        /// <summary>
        /// Gets a rack product by identifier
        /// </summary>
        /// <param name="rackId">Rack Product identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rack product
        /// </returns>
        Task<IList<RackProduct>> GetProductByRackIdAsync(int rackId);

        /// <summary>
        /// Get rack level by rack identifiers
        /// </summary>
        /// <param name="rackId">Rack identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the levels
        /// </returns>
        Task<IList<RackLevel>> GetRackLevelsByRackIdAsync(int rackId);

        #endregion

        #region Rack Level Type

        /// <summary>
        /// Gets a Rack level type by identifier
        /// </summary>
        /// <param name="rackLevelTypeId">Rack level type identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rack level type
        /// </returns>
        Task<RackLevelType> GetRackLevelTypeByIdAsync(int rackLevelTypeId);

        /// <summary>
        /// Inserts a Rack level type
        /// </summary>
        /// <param name="rackLevelType">Rack level type</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertRackLevelTypeAsync(RackLevelType rackLevelType);

        /// <summary>
        /// Updates the Rack level type
        /// </summary>
        /// <param name="rackLevelType">Rack level type</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateRackLevelTypeAsync(RackLevelType rackLevelType);

        /// <summary>
        /// Deletes the Rack Level Type
        /// </summary>
        /// <param name="rackLevelType">Rack Level Type</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteRackLevelTypeAsync(RackLevelType rackLevelType);


        /// <summary>
        /// Get all rack level type entries
        /// </summary>
        /// <param name="showHidden">Show Hidden</param>
        /// <param name="name">rack level name</param>
        /// <param name="rackLevelTypeId">Rack Level type identifier</param>
        /// <param name="visible">Visible</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the paged list of entity entries
        /// </returns>
        Task<IPagedList<RackLevelType>> GetAllRackLevelTypePagedAsync(
            bool showHidden = false,
            string name = null,
            int? functionTypeId = null,
            int pageIndex = 0,
            int pageSize = int.MaxValue);


        /// <summary>
        /// Get rack level type by rack identifiers
        /// </summary>
        /// <param name="rackLevelTypeId">Rack level type identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the levels
        /// </returns>
        Task<IList<RackLevelType>> GetRackLevelTypeByRackIdAsync(int rackLevelTypeId);

        #endregion

    }
}
