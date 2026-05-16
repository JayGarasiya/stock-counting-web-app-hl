using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.CountingSequence.Models.Rack
{
    /// <summary>
    /// Represents a rack product search model
    /// </summary>
    public partial record RackProductSearchModel : BaseSearchModel
    {
        #region Properties

        public int RackId { get; set; }

        public int LevelId { get; set; }

        #endregion
    }
}
