using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Plugin.Misc.CountingSequence.Domain;

namespace Nop.Plugin.Misc.CountingSequence.Services
{
    /// <summary>
    /// Pallet service 
    /// </summary>
    public class PalletService : IPalletService
    {
        #region Fields

        protected readonly IRepository<Pallet> _palletRepository;
        protected readonly IRepository<PalletProduct> _palletProductRepository;
        protected readonly IRepository<Product> _productRepository;
        protected readonly IRepository<ShipmentTransitItems> _shipmentTransitItemsRepository;
        protected readonly IRepository<ShipmentTransit> _shipmentTransitRepository;

        #endregion

        #region Ctor

        public PalletService(IRepository<Pallet> palletRepository,
            IRepository<PalletProduct> palletProductRepository,
            IRepository<Product> productRepository,
            IRepository<ShipmentTransitItems> shipmentTransitItemsRepository,
            IRepository<ShipmentTransit> shipmentTransitRepository)
        {
            _palletRepository = palletRepository;
            _palletProductRepository = palletProductRepository;
            _productRepository = productRepository;
            _shipmentTransitItemsRepository = shipmentTransitItemsRepository;
            _shipmentTransitRepository = shipmentTransitRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a pallet by identifier
        /// </summary>
        /// <param name="id">Pallet identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the pallet
        /// </returns>
        public async Task<Pallet> GetPalletByIdAsync(int id)
        {
            return await _palletRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Get Pallet by identifiers
        /// </summary>
        /// <param name="palletsIds">Pallets identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the products
        /// </returns>
        public virtual async Task<IList<Pallet>> GetPalletsByIdsAsync(int[] palletsIds)
        {
            return await _palletRepository.GetByIdsAsync(palletsIds, cache => default, false);
        }

        /// <summary>
        /// Inserts a pallet
        /// </summary>
        /// <param name="pallet">Pallet</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task InsertPalletAsync(Pallet pallet)
        {
            await _palletRepository.InsertAsync(pallet);
        }

        /// <summary>
        /// Updates the pallet
        /// </summary>
        /// <param name="pallet">Pallet</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task UpdatePalletAsync(Pallet pallet)
        {
            await _palletRepository.UpdateAsync(pallet);
        }

        /// <summary>
        /// Deletes the pallet
        /// </summary>
        /// <param name="pallet">Pallet</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task DeletePalletAsync(Pallet pallet)
        {
            await _palletRepository.DeleteAsync(pallet);
        }

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
        public async Task<IPagedList<Pallet>> GetAllPalletPagedAsync( bool showHidden = false,
            string name = null,
            int? shipmentDispatchId = null,
            bool? visible = null,
            int productId = 0,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _palletRepository.Table;

            if (!showHidden)
            {
                query = query.Where(m => m.IsVisible);
            }
            else if (visible.HasValue)
            {
                query = query.Where(m => m.IsVisible == visible.Value);
            }

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => x.Name.Contains(name));

            if (shipmentDispatchId.HasValue && shipmentDispatchId.Value > 0)
                query = query.Where(x => x.ShipmentDispatchId == shipmentDispatchId.Value);

            //Product filter 
            if (productId > 0)
            {
                query = from x in query
                        join pp in _palletProductRepository.Table
                            on x.Id equals pp.PalletId
                        where pp.ProductId == productId
                        select x;
            }

            return await query.OrderBy(x => x.DisplayOrder).ToPagedListAsync(pageIndex, pageSize);
        }

        /// <summary>
        /// Get Pallet by identifiers
        /// </summary>
        /// <param name="palletId">Pallets Product identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the products
        /// </returns>
        public async Task<IList<PalletProduct>> GetProductByPalletIdAsync(int palletId)
        {
            var query = from pp in _palletProductRepository.Table
                        where pp.PalletId == palletId
                        select pp;

            return await query.ToListAsync();
        }

        /// <summary>
        /// Get Pallet by shipment dispatch Ids
        /// </summary>
        /// <param name="shipmentDispatchIds">Pallets shipment dispatch Ids</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the products
        /// </returns>
        public async Task<IList<Pallet>> GetPalletByShipmentDispatchIdsAsync(int[] shipmentDispatchIds)
        {
            var query =
                from p in _palletRepository.Table
                where p.IsVisible &&
                      (
                          shipmentDispatchIds.Contains(p.ShipmentDispatchId)

                          ||

                          (
                              from sti in _shipmentTransitItemsRepository.Table
                              join st in _shipmentTransitRepository.Table
                                  on sti.ShipmentTransitId equals st.Id
                              where sti.PalletId == p.Id &&
                                    st.Status == (int)ShipmentTransitStatus.Received
                              select sti.Id
                          ).Any()
                      )
                select p;

            return await query.ToListAsync();
        }

        #region Pallet Product

        /// <summary>
        /// Gets a Pallet Product by identifier
        /// </summary>
        /// <param name="id">pallet Product identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the pallet product
        /// </returns>
        public async Task<PalletProduct> GetPalletProductByIdAsync(int id)
        {
            return await _palletProductRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Inserts a pallet product
        /// </summary>
        /// <param name="palletProduct">Pallet Prodcut</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task InsertPalletProductAsync(PalletProduct palletProduct)
        {
            await _palletProductRepository.InsertAsync(palletProduct);
        }

        /// <summary>
        /// Deletes the pallet product
        /// </summary>
        /// <param name="palletProduct">Pallet</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task DeletePalletProductAsync(PalletProduct palletProduct)
        {
            await _palletProductRepository.DeleteAsync(palletProduct);
        }

        /// <summary>
        /// Get Pallet Product by identifiers
        /// </summary>
        /// <param name="palletProductId">Pallets Product identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the products
        /// </returns>
        public async Task<IList<PalletProduct>> GetPalletProductsByIdAsync(int palletProductId)
        {
            var query = from r in _palletProductRepository.Table
                        where r.PalletId == palletProductId
                        select r;
            return await query.ToListAsync();
        }

        /// <summary>
        /// Get Pallet By Product identifiers
        /// </summary>
        /// <param name="productId">Product identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the pallets
        /// </returns>
        public async Task<IList<Pallet>> GetPalletByProductIdAsync(int productId)
        {
            var query = from rp in _palletProductRepository.Table
                        join r in _palletRepository.Table on rp.PalletId equals r.Id
                        where rp.ProductId == productId && r.IsVisible
                        select r;

            return await query.ToListAsync();
        }

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
        public async Task<IPagedList<PalletProduct>> GetAllPalletProductPageAsync(int palletId = 0, int productId = 0, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _palletProductRepository.Table;

            if (palletId > 0)
            {
                query = query.Where(x => x.PalletId == palletId);
            }

            if (productId > 0)
            {
                query = query.Where(x => x.ProductId == productId);
            }

            return await query.ToPagedListAsync(pageIndex, pageSize);
        }

        #endregion

        #endregion
    }
}
