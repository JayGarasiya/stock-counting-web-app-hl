using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.CountingSequence.Models.Rack
{
    /// <summary>
    /// Represents a level search model
    /// </summary>
    public record LevelSearchModel : BaseSearchModel
    {
        #region Properties

        public int RackId { get; set; }

        #endregion
    }
}
