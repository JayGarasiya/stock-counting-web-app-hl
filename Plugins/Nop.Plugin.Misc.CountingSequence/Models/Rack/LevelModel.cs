using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.CountingSequence.Models.Rack
{
    /// <summary>
    /// Represents a level model
    /// </summary>
    public record LevelModel : BaseNopEntityModel
    {
        #region Properties

        public int RackId { get; set; }

        public int LevelId { get; set; }

        public string Name { get; set; }

        #endregion
    }
}
