using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Misc.CountingSequence.Domain;

namespace Nop.Plugin.Misc.CountingSequence.Data
{
    [NopMigration("2026/04/18 10:30:00:0000001", "Misc.CountingSequence base schema", MigrationProcessType.Installation)]
    public class SchemaMigration : AutoReversingMigration
    {
        #region Methods

        /// <summary>
        /// Collect the UP migration expressions
        /// </summary>
        public override void Up()
        {
            Create.TableFor<Pallet>();

            Create.TableFor<Rack>();

            Create.TableFor<RackLevel>();

            Create.TableFor<RackLevelType>();

            Create.TableFor<ShipmentDispatches>();

            Create.TableFor<RackProduct>();

            Create.TableFor<PalletProduct>();

            Create.TableFor<StockCount>();

            Create.TableFor<StockCountItem>();

            Create.TableFor<ShipmentTransit>();

            Create.TableFor<ShipmentTransitItems>();

            Create.TableFor<Channel>();

            Create.TableFor<BackOrders>();

            Create.TableFor<PalletStock>();

            Create.TableFor<RackStock>();

            Create.TableFor<WarehouseStockHistory>();

            Create.TableFor<InventoryAppProductMapping>();

            Create.TableFor<ContainerShipmentDispatchMapping>();
        }
        #endregion
    }
}
