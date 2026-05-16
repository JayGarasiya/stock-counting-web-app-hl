using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.CountingSequence.Models.Pallet
{
    /// <summary>
    /// Represents a pallet product search model
    /// </summary>
    public partial record PalletProductSearchModel : BaseSearchModel
    {
        public int PalletId { get; set; }
    }
}
