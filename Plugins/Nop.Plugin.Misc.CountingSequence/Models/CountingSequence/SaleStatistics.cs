namespace Nop.Plugin.Misc.CountingSequence.Models.CountingSequence
{
    public partial record SaleStatistics
    {
        public string Month { get; set; }
        public int Count { get; set; }
        public int Sales { get; set; }
    }
}
