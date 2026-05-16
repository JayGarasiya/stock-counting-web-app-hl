using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.CountingSequence.Models
{
    /// <summary>
    /// Represents a configuration model
    /// </summary>
    public class ConfigurationModel
    {
        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.Enabled")]
        public bool Enabled { get; set; }
        public bool Enabled_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.PalletCountingOrder")]
        public int PalletCountingOrder { get; set; }
        public bool PalletCountingOrder_OverrideForStore { get; set; }
        public IList<SelectListItem> AvailablePalletCountingOrder { get; set; } = new List<SelectListItem>();

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SaleTypeOrder")]
        public int SaleTypeOrder { get; set; }
        public bool SaleTypeOrder_OverrideForStore { get; set; }
        public IList<SelectListItem> AvailableSaleOrder { get; set; } = new List<SelectListItem>();

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.Level")]
        public int Level { get; set; }
        public bool Level_OverrideForStore { get; set; }
        public IList<SelectListItem> AvailableLevels { get; set; } = new List<SelectListItem>();

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.Position")]
        public int Position { get; set; }
        public bool Position_OverrideForStore { get; set; }
        public IList<SelectListItem> AvailablePositions { get; set; } = new List<SelectListItem>();

        #endregion
    }
}
