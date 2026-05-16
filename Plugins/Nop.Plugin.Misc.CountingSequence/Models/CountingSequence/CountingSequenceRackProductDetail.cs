using Nop.Web.Framework.Models;
using Nop.Web.Models.Catalog;

namespace Nop.Plugin.Misc.CountingSequence.Models.CountingSequence
{
    public partial record CountingSequenceRackProductDetail : BaseNopEntityModel
    {
        public CountingSequenceRackProductDetail()
        {
            ProductDetailsModel = new ProductDetailsModel();
            CountSheetRows = new List<RackCountSheetModel>();
            AvailableProductRacks = new List<Domain.Rack>();
            AvailableRacks = new List<Domain.Rack>();
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
        public int CurrentRackId { get; set; }
        public int RackIndex { get; set; }
        public int TotalRacks { get; set; }
        public int TotalCountingQuantity { get; set; }
        public int TotalCountingQuantityLastMonth { get; set; }
        public int TotalProductQuantity { get; set; }
        public int TotalProductQuantityLastMonth { get; set; }
        public int TotalSaleThisMonth { get; set; }
        public int TotalSaleThisLastSixMonth { get; set; }

        public bool IsSummaryMode { get; set; }
        public string ProductPositionLevel { get; set; }
        public string SequenceLevelType { get; set; }
        public string SequencePositionType { get; set; }
        public StockCountModel StockCountModel { get; set; }
        public IList<StockCountItemModel> SummaryItems { get; set; }

        public IList<Domain.Rack> AvailableProductRacks { get; set; }
        public IList<Domain.Rack> AvailableRacks { get; set; }
        public IList<RackCountSheetModel> CountSheetRows { get; set; }

        public IList<ProductShipmentModel> RecentShipments { get; set; }
        public IList<ProductShipmentModel> IncomingShipments { get; set; }
    }

    public class RackCountSheetModel
    {
        public int Cases { get; set; }
        public int Units { get; set; }
    }
}
