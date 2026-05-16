using Nop.Core;
using Nop.Data;
using Nop.Plugin.Misc.CountingSequence.Domain;

namespace Nop.Plugin.Misc.CountingSequence.Services
{
    /// <summary>
    /// Rack service 
    /// </summary>
    public class RackService : IRackService
    {
        #region Fields

        protected readonly IRepository<Rack> _rackRepository;
        protected readonly IRepository<RackLevel> _rackLevelRepository;
        protected readonly IRepository<RackProduct> _rackProductRepository;
        protected readonly IRepository<RackLevelType> _rackLevelTypeRepository;

        #endregion

        #region Ctor

        public RackService(IRepository<Rack> rackRepository,
            IRepository<RackLevel> rackLevelRepository,
            IRepository<RackProduct> rackProductRepository,
            IRepository<RackLevelType> rackLevelTypeRepository)
        {
            _rackRepository = rackRepository;
            _rackLevelRepository = rackLevelRepository;
            _rackProductRepository = rackProductRepository;
            _rackLevelTypeRepository = rackLevelTypeRepository;
        }

        #endregion

        #region Methods

        #region Rack

        /// <summary>
        /// Gets a Rack by identifier
        /// </summary>
        /// <param name="id">Rack identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rack
        /// </returns>
        public async Task<Rack> GetRackByIdAsync(int id)
        {
            return await _rackRepository.GetByIdAsync(id);
        }

        public async Task<RackLevel> GetRackLevelByIdAsync(int id)
        {
            return await _rackLevelRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Gets a Rack Level by identifier
        /// </summary>
        /// <param name="rackId">Rack Level identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rack level
        /// </returns>
        public async Task<RackLevel> GetRackLevelByRackIdAsync(int rackId)
        {
            var query = from r in _rackLevelRepository.Table
                        where r.RackId == rackId
                        select r;
            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Inserts a Rack
        /// </summary>
        /// <param name="rack">Rack</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task InsertRackAsync(Rack rack)
        {
            await _rackRepository.InsertAsync(rack);
        }

        /// <summary>
        /// Inserts a Rack Level
        /// </summary>
        /// <param name="rackLevel">Rack Level</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task InsertRackLevelAsync(RackLevel rackLevel)
        {
            await _rackLevelRepository.InsertAsync(rackLevel);
        }

        /// <summary>
        /// Updates the Rack
        /// </summary>
        /// <param name="rack">Rack</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task UpdateRackAsync(Rack rack)
        {
            await _rackRepository.UpdateAsync(rack);
        }

        /// <summary>
        /// Deletes the Rack
        /// </summary>
        /// <param name="rack">Rack</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task DeleteRackAsync(Rack rack)
        {
            await _rackRepository.DeleteAsync(rack);
        }

        /// <summary>
        /// Deletes the Rack Level
        /// </summary>
        /// <param name="rackLevel">Rack Level</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task DeleteRackLevelAsync(RackLevel rackLevel)
        {
            await _rackLevelRepository.DeleteAsync(rackLevel);
        }

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
        public async Task<IPagedList<Rack>> GetAllRackPagedAsync(
            bool showHidden = false,
            string name = null,
            int? levelTypeId = null,
            bool? visible = null,
            int productId = 0,
            int? functionTypeId = null,
            int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            var query = _rackRepository.Table;
            if (!showHidden)
                query = query.Where(x => x.IsVisible);
            else if (visible.HasValue)
                query = query.Where(x => x.IsVisible == visible.Value);

            // Name filter
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => x.Name.Contains(name));

            // Level filter 
            if (levelTypeId.HasValue && levelTypeId.Value > 0)
            {
                var rackIds = _rackLevelRepository.Table
                    .Where(l => l.LevelId == levelTypeId)
                    .Select(l => l.RackId);

                query = query.Where(x => rackIds.Contains(x.Id));
            }

            // Function type filter
            if (functionTypeId.HasValue && functionTypeId.Value > 0)
            {
                query = query.Where(x => x.FunctionTypeId == functionTypeId.Value);
            }

            // Product filter
            if (productId > 0)
            {
                var rackIds = _rackProductRepository.Table
                    .Where(rp => rp.ProductId == productId)
                    .Select(rp => rp.RackId);

                query = query.Where(x => rackIds.Contains(x.Id));
            }
            return await query.OrderBy(x => x.DisplayOrder).ToPagedListAsync(pageIndex, pageSize);
        }

        /// <summary>
        /// Get Rack by identifiers
        /// </summary>
        /// <param name="ids">Rack identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the products
        /// </returns>
        public async Task<IList<Rack>> GetRacksByIdsAsync(int[] ids)
        {
            return await _rackRepository.Table
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }

        /// <summary>
        /// Get rack product by identifiers
        /// </summary>
        /// <param name="productId">Rack Product identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the products
        /// </returns>
        public async Task<IList<Rack>> GetRackByProductIdAsync(int productId)
        {
            var query = from rp in _rackProductRepository.Table
                        join r in _rackRepository.Table on rp.RackId equals r.Id
                        where rp.ProductId == productId && r.IsVisible
                        select r;

            return await query.ToListAsync();
        }

        /// <summary>
        /// Get rack level by rack identifiers
        /// </summary>
        /// <param name="rackId">Rack identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the levels
        /// </returns>
        public async Task<IList<RackLevel>> GetRackLevelsByRackIdAsync(int rackId)
        {
            return await _rackLevelRepository.Table
                .Where(x => x.RackId == rackId)
                .ToListAsync();
        }

        #endregion

        #region Rack Product

        /// <summary>
        /// Gets a Rack Product by identifier
        /// </summary>
        /// <param name="id">Rack Product identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rack product
        /// </returns>
        public async Task<RackProduct> GetRackProductByIdAsync(int id)
        {
            return await _rackProductRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Inserts a rack product
        /// </summary>
        /// <param name="rackProduct">Rack Prodcut</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task InsertRackProductAsync(RackProduct rackProduct)
        {
            await _rackProductRepository.InsertAsync(rackProduct);
        }

        /// <summary>
        /// Deletes the rack product
        /// </summary>
        /// <param name="rackProduct">Rack Product</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task DeleteRackProductAsync(RackProduct rackProduct)
        {
            await _rackProductRepository.DeleteAsync(rackProduct);
        }

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
        public async Task<IPagedList<RackProduct>> GetAllRackProductPageAsync(
            int rackId = 0,
            int productId = 0,
            int levelId = 0,
            int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            var query = _rackProductRepository.Table;

            if (rackId > 0)
            {
                query = query.Where(x => x.RackId == rackId);
            }

            if (productId > 0)
            {
                query = query.Where(x => x.ProductId == productId);
            }

            if (levelId > 0)
            {
                query = query.Where(x => x.RackLevelId == levelId);
            }

            return await query.ToPagedListAsync(pageIndex, pageSize);
        }

        /// <summary>
        /// Get Rack Product by identifiers
        /// </summary>
        /// <param name="rackId">Racks Product identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the products
        /// </returns>
        public async Task<IList<RackProduct>> GetProductByRackIdAsync(int rackId)
        {
            var query = from rp in _rackProductRepository.Table
                        where rp.RackId == rackId
                        select rp;

            return await query.ToListAsync();
        }

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
        public async Task<RackLevelType> GetRackLevelTypeByIdAsync(int rackLevelTypeId)
        {
            return await _rackLevelTypeRepository.GetByIdAsync(rackLevelTypeId);
        }

        /// <summary>
        /// Inserts a Rack level type
        /// </summary>
        /// <param name="rackLevelType">Rack level type</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task InsertRackLevelTypeAsync(RackLevelType rackLevelType)
        {
            await _rackLevelTypeRepository.InsertAsync(rackLevelType);
        }

        /// <summary>
        /// Updates the Rack level type
        /// </summary>
        /// <param name="rackLevelType">Rack level type</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task UpdateRackLevelTypeAsync(RackLevelType rackLevelType)
        {
            await _rackLevelTypeRepository.UpdateAsync(rackLevelType);
        }

        /// <summary>
        /// Deletes the Rack Level Type
        /// </summary>
        /// <param name="rackLevelType">Rack Level Type</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task DeleteRackLevelTypeAsync(RackLevelType rackLevelType)
        {
            await _rackLevelTypeRepository.DeleteAsync(rackLevelType);
        }


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
        public async Task<IPagedList<RackLevelType>> GetAllRackLevelTypePagedAsync(
            bool showHidden = false,
            string name = null,
            int? functionTypeId = null,
            int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            var query = _rackLevelTypeRepository.Table;

            // Search by name
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(x => x.Name.Contains(name));
            }

            // Search by level type
            if (functionTypeId.HasValue)
            {
                query = query.Where(x => x.FunctionTypeId == functionTypeId.Value);
            }

            // Hide invisible records if showHidden = false
            if (!showHidden)
            {
                query = query.Where(x => x.IsVisible);
            }

            query = query.OrderBy(x => x.DisplayOrder);

            return await query.ToPagedListAsync(pageIndex, pageSize);
        }

        /// <summary>
        /// Get rack level type by rack identifiers
        /// </summary>
        /// <param name="rackLevelTypeId">Rack level type identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the levels
        /// </returns>
        public async Task<IList<RackLevelType>> GetRackLevelTypeByRackIdAsync(int rackLevelTypeId)
        {
            return await _rackLevelTypeRepository.Table
                .Where(x => x.Id == rackLevelTypeId)
                .ToListAsync();
        }

        #endregion

        #endregion
    }
}
