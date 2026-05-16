using Nop.Web.Framework.Models;
using Nop.Web.Models.Catalog;

namespace Nop.Plugin.Misc.CountingSequence.Models.CountingSequence
{
    public partial record CountingSequencePalletProductDetail : BaseNopEntityModel
    {
        public CountingSequencePalletProductDetail()
        {
            ProductDetailsModel = new ProductDetailsModel();
            CountSheetRows = new List<PalletCountSheetModel>();
            AvailableProductPallets = new List<Domain.Pallet>();
            AvailablePallets = new List<Domain.Pallet>();
            SummaryItems = new List<StockCountItemModel>();
            RecentShipments = new List<ProductShipmentModel>();
            IncomingShipments = new List<ProductShipmentModel>();
            StockCountModel = new StockCountModel();
        }

        public ProductDetailsModel ProductDetailsModel { get; set; }
        public string CategoryName { get; set; }
        public int QuantityPerPack { get; set; }
        public int StockCountId { get; set; }
        public int CurrentIndex { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPalletId { get; set; }
        public int PalletIndex { get; set; }
        public int TotalPallets { get; set; }
        public int TotalCountingQuantity { get; set; }
        public int TotalCountingQuantityLastMonth { get; set; }
        public int TotalProductQuantity { get; set; }
        public int TotalProductQuantityLastMonth { get; set; }
        public int TotalSaleThisMonth { get; set; }
        public int TotalSaleThisLastSixMonth { get; set; }

        public bool IsSummaryMode { get; set; }
        public string PalletCountingOrder { get; set; }
        public StockCountModel StockCountModel { get; set; }
        public IList<StockCountItemModel> SummaryItems { get; set; }

        public IList<Domain.Pallet> AvailableProductPallets { get; set; }
        public IList<Domain.Pallet> AvailablePallets { get; set; }
        public IList<PalletCountSheetModel> CountSheetRows { get; set; }

        public IList<ProductShipmentModel> RecentShipments { get; set; }
        public IList<ProductShipmentModel> IncomingShipments { get; set; }
    }
    public class PalletCountSheetModel
    {
        public int Cases { get; set; }
        public int Units { get; set; }
    }

}
