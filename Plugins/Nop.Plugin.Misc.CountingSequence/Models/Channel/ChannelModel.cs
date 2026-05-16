using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.CountingSequence.Models.Channel
{
    /// <summary>
    /// Represents a channel model
    /// </summary>
    public record ChannelModel : BaseNopEntityModel
    {
        #region Ctor

        public ChannelModel()
        {
            AvailableChannelTypes = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Channel.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Channel.Fields.ChannelId")]
        public int ChannelId { get; set; }

        public string ChannelTypeName { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Channel.Fields.Description")]
        public string Description { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Channel.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CountingSequence.Channel.Fields.Visible")]
        public bool Visible { get; set; }

        public IList<SelectListItem> AvailableChannelTypes { get; set; }

        #endregion
    }
}
