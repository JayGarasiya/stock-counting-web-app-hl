using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.CountingSequence.Models.CountingSequence
{
    public partial record ProductShipmentModel : BaseNopEntityModel
    {
        public string ShipmentName { get; set; }
        public string ShipmentDispatchesDate { get; set; }
        public int ShipmentTransitItemsQuantity { get; set; }
    }
}
