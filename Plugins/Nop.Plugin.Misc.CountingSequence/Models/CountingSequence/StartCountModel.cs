namespace Nop.Plugin.Misc.CountingSequence.Models.CountingSequence
{
    public class StartCountModel
    {
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public DateTime CountDate { get; set; }
        public string CountStatus { get; set; }
        public int StockCountId { get; set; }
        public bool IsRackAvailable { get; set; }
        public bool IsPalletAvailable { get; set; }
    }
}
