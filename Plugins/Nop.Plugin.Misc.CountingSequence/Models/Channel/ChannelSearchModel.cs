using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.CountingSequence.Models.Channel
{
    /// <summary>
    /// Represents a channel search model
    /// </summary>
    public record ChannelSearchModel : BaseSearchModel
    {
        #region Ctor

        public ChannelSearchModel()
        {
            AvailableVisibleOptions = new List<SelectListItem>();
            AvailableChannelTypes = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchChannelName")]
        public string SearchChannelName { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchVisibleId")]
        public int SearchVisibleId { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Fields.SearchChannelTypeId")]
        public int SearchChannelTypeId { get; set; }

        public IList<SelectListItem> AvailableVisibleOptions { get; set; }

        public IList<SelectListItem> AvailableChannelTypes { get; set; }

        #endregion
    }
}
