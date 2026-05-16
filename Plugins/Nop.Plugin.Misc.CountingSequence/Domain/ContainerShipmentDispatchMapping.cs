using Nop.Core;

namespace Nop.Plugin.Misc.CountingSequence.Domain
{
    public class ContainerShipmentDispatchMapping : BaseEntity
    {
        public int InventoryContainerId { get; set; }
        public int ShipmentDispatchId { get; set; }
    }
}
