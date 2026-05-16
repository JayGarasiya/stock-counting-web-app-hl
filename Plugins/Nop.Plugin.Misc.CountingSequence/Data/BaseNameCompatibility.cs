using Nop.Data.Mapping;
using Nop.Plugin.Misc.CountingSequence.Domain;

namespace Nop.Plugin.Misc.CountingSequence.Data
{
    /// <summary>
    /// Base instance of backward compatibility of table naming
    /// </summary>
    public partial class BaseNameCompatibility : INameCompatibility
    {
        public Dictionary<Type, string> TableNames => new()
        {
            { typeof(BackOrders), "Hl_BackOrders" },
            { typeof(Channel), "Hl_Channel" },
            { typeof(Pallet), "Hl_Pallet" },
            { typeof(PalletProduct), "Hl_PalletProduct" },
            { typeof(PalletStock), "Hl_PalletStock" },
            { typeof(Rack), "Hl_Rack" },
            { typeof(RackLevel), "Hl_RackLevel" },
            { typeof(RackProduct), "Hl_RackProduct" },
            { typeof(RackStock), "Hl_RackStock" },
            { typeof(ShipmentDispatches), "Hl_ShipmentDispatches" },
            { typeof(ShipmentTransit), "Hl_ShipmentTransit" },
            { typeof(ShipmentTransitItems), "Hl_ShipmentTransitItems" },
            { typeof(StockCount), "Hl_StockCount" },
            { typeof(StockCountItem), "Hl_StockCountItem" },
            { typeof(WarehouseStockHistory), "Hl_WarehouseStockHistory" },
            { typeof(InventoryAppProductMapping), "Hl_InventoryAppProductMapping" },
            { typeof(ContainerShipmentDispatchMapping), "Hl_ContainerShipmentDispatchMapping" },
            { typeof(RackLevelType), "Hl_RackLevelType" },
        };

        public Dictionary<(Type, string), string> ColumnName => new()
        {
        };
    }
}
