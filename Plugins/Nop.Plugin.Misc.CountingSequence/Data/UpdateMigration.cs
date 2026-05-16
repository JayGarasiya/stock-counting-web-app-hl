using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Mapping;
using Nop.Data.Migrations;
using Nop.Plugin.Misc.CountingSequence.Domain;
namespace Nop.Plugin.Misc.CountingSequence.Data
{
    [NopMigration("2026-05-06 08:09:09", "Misc.CountingSequence Table", MigrationProcessType.Update)]
    public class UpdateMigration : Migration
    {
        #region Methods

        /// <summary>
        /// Collect the UP migration expressions
        /// </summary>
        public override void Up()
        {

            // Remove ToDispatchId from ShipmentTransit
            var shipmentTransitTable = NameCompatibilityManager.GetTableName(typeof(ShipmentTransit));

            if (Schema.Table(shipmentTransitTable).Exists())
            {
                if (Schema.Table(shipmentTransitTable).Column("ToDispatchId").Exists())
                {
                    Delete.Column("ToDispatchId")
                        .FromTable(shipmentTransitTable);
                }
            }

            var shipmentTransitItems = NameCompatibilityManager.GetTableName(typeof(ShipmentTransitItems));

            if (Schema.Table(shipmentTransitItems).Exists())
            {
                if (Schema.Table(shipmentTransitItems).Column("HAndL").Exists())
                {
                    Rename.Column("HAndL")
                        .OnTable(shipmentTransitItems)
                        .To("ToDispatchId");
                }
            }

            // Add Units, Cases, HAndL to ShipmentTransitItems
            var shipmentTransitItemsTable = NameCompatibilityManager.GetTableName(typeof(ShipmentTransitItems));

            if (Schema.Table(shipmentTransitItemsTable).Exists())
            {
                if (!Schema.Table(shipmentTransitItemsTable).Column(nameof(ShipmentTransitItems.ToDispatchId)).Exists())
                {
                    Alter.Table(shipmentTransitItemsTable)
                        .AddColumn(nameof(ShipmentTransitItems.ToDispatchId))
                        .AsInt32()
                        .NotNullable()
                        .WithDefaultValue(0);
                }

                if (Schema.Table(shipmentTransitItems).Column("Units").Exists())
                {
                    Rename.Column("Units")
                        .OnTable(shipmentTransitItemsTable)
                        .To(nameof(ShipmentTransitItems.NoOfUnits));

                }
                if (Schema.Table(shipmentTransitItems).Column("Cases").Exists())
                {
                    Rename.Column("Cases")
                        .OnTable(shipmentTransitItemsTable)
                        .To(nameof(ShipmentTransitItems.NoOfPacks));

                }
            }
            // Check if table already exists
            if (Schema.Table(nameof(Pallet)).Exists())
            {
                if (Schema.Table(nameof(Pallet)).Column("ShipmentRouteTypeId").Exists())
                {
                    Rename.Column("ShipmentRouteTypeId")
                        .OnTable(nameof(Pallet))
                        .To("ShipmentDispatchId");
                }

                // Add Description column
                if (!Schema.Table(nameof(Pallet)).Column("Description").Exists())
                {
                    Alter.Table(nameof(Pallet))
                        .AddColumn("Description")
                        .AsString(int.MaxValue)
                        .Nullable();
                }

                if (!Schema.Table(nameof(Pallet)).Column(nameof(Pallet.SequenceOrder)).Exists())
                {
                    Alter.Table(nameof(Pallet))
                        .AddColumn(nameof(Pallet.SequenceOrder))
                        .AsInt32()
                        .NotNullable().WithDefaultValue(0);
                }
            }

            if (Schema.Table(NameCompatibilityManager.GetTableName(typeof(Rack))).Exists())
            {
                if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(Rack))).Column(nameof(Rack.FunctionTypeId)).Exists())
                {
                    Alter.Table(NameCompatibilityManager.GetTableName(typeof(Rack)))
                        .AddColumn(nameof(Rack.FunctionTypeId))
                        .AsInt32()
                        .NotNullable().WithDefaultValue(5);
                }
            }

            if (Schema.Table(nameof(Channel)).Exists())
            {
                if (Schema.Table(nameof(Channel)).Column("ChannelType").Exists())
                {
                    Rename.Column("ChannelType")
                        .OnTable(nameof(Channel))
                        .To("ChannelId");
                }
            }

            if (Schema.Table(NameCompatibilityManager.GetTableName(typeof(RackProduct))).Exists())
            {
                //// Add LevelId column if not exists
                //if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(RackProduct))).Column(nameof(RackProduct.RackLevelId)).Exists())
                //{
                //    Alter.Table(NameCompatibilityManager.GetTableName(typeof(RackProduct)))
                //        .AddColumn(nameof(RackProduct.RackLevelId))
                //        .AsInt32()
                //        .NotNullable()
                //        .WithDefaultValue(0);
                //}

                if (Schema.Table(NameCompatibilityManager.GetTableName(typeof(RackProduct))).Column("LevelId").Exists())
                {
                    Rename.Column("LevelId")
                        .OnTable(NameCompatibilityManager.GetTableName(typeof(RackProduct)))
                        .To(nameof(RackProduct.RackLevelId));
                }

                //Add RackProductPositionId column
                if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(RackProduct))).Column(nameof(RackProduct.ProductPositionId)).Exists())
                {
                    Alter.Table(NameCompatibilityManager.GetTableName(typeof(RackProduct)))
                        .AddColumn(nameof(RackProduct.ProductPositionId))
                        .AsInt32()
                        .NotNullable()
                        .WithDefaultValue(0);
                }

            }

            if (Schema.Table(NameCompatibilityManager.GetTableName(typeof(StockCount))).Exists())
            {
                if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(StockCount))).Column(nameof(StockCount.DiscontinuePalletIds)).Exists())
                {
                    Alter.Table(NameCompatibilityManager.GetTableName(typeof(StockCount)))
                        .AddColumn(nameof(StockCount.DiscontinuePalletIds))
                        .AsString()
                        .Nullable();
                }

                if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(StockCount))).Column(nameof(StockCount.DeliveredShipmentIds)).Exists())
                {
                    Alter.Table(NameCompatibilityManager.GetTableName(typeof(StockCount)))
                        .AddColumn(nameof(StockCount.DeliveredShipmentIds))
                        .AsString()
                        .Nullable();
                }
            }


            if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(InventoryAppProductMapping))).Exists())
            {
                Create.TableFor<InventoryAppProductMapping>();
            }

            if (Schema.Table(NameCompatibilityManager.GetTableName(typeof(InventoryAppProductMapping))).Exists())
            {
                if (Schema.Table(NameCompatibilityManager.GetTableName(typeof(InventoryAppProductMapping))).Column("InventoryProductId").Exists())
                    Rename.Column("InventoryProductId")
                            .OnTable(NameCompatibilityManager.GetTableName(typeof(InventoryAppProductMapping)))
                            .To(nameof(InventoryAppProductMapping.InventoryProductModelId));

                if (Schema.Table(NameCompatibilityManager.GetTableName(typeof(InventoryAppProductMapping))).Column("ProductId").Exists())
                    Rename.Column("ProductId")
                            .OnTable(NameCompatibilityManager.GetTableName(typeof(InventoryAppProductMapping)))
                            .To(nameof(InventoryAppProductMapping.CountProductId));
            }


            //if (Schema.Table(NameCompatibilityManager.GetTableName(typeof(RackStock))).Exists())
            //{
            //    if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(RackStock))).Column(nameof(RackStock.ShipmentTransitId)).Exists())
            //    {
            //        Alter.Table(NameCompatibilityManager.GetTableName(typeof(RackStock)))
            //           .AddColumn(nameof(RackStock.ShipmentTransitId))
            //           .AsInt32()
            //           .NotNullable()
            //           .WithDefaultValue(0);
            //    }
            //}

            if (Schema.Table(NameCompatibilityManager.GetTableName(typeof(PalletStock))).Exists())
            {
                if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(PalletStock))).Column(nameof(PalletStock.ShipmentTransitId)).Exists())
                {
                    Alter.Table(NameCompatibilityManager.GetTableName(typeof(PalletStock)))
                       .AddColumn(nameof(PalletStock.ShipmentTransitId))
                       .AsInt32()
                       .NotNullable()
                       .WithDefaultValue(0);
                }


                if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(PalletStock))).Column(nameof(PalletStock.ToDispatchId)).Exists())
                {
                    Alter.Table(NameCompatibilityManager.GetTableName(typeof(PalletStock)))
                        .AddColumn(nameof(PalletStock.ToDispatchId))
                        .AsInt32()
                        .NotNullable()
                        .WithDefaultValue(0);
                }
            }

            // Rename tables
            if (Schema.Table("hl_BackOrders").Exists())
            {
                Rename.Table("hl_BackOrders").To(NameCompatibilityManager.GetTableName(typeof(BackOrders)));
            }

            if (Schema.Table("hl_Channel").Exists())
            {
                Rename.Table("hl_Channel").To(NameCompatibilityManager.GetTableName(typeof(Channel)));
            }

            if (Schema.Table("hl_Pallet").Exists())
            {
                Rename.Table("hl_Pallet").To(NameCompatibilityManager.GetTableName(typeof(Pallet)));
            }

            if (Schema.Table("hl_PalletProduct").Exists())
            {
                Rename.Table("hl_PalletProduct").To(NameCompatibilityManager.GetTableName(typeof(PalletProduct)));
            }

            if (Schema.Table("hl_PalletStock").Exists())
            {
                Rename.Table("hl_PalletStock").To(NameCompatibilityManager.GetTableName(typeof(PalletStock)));
            }

            if (Schema.Table("hl_Rack").Exists())
            {
                Rename.Table("hl_Rack").To(NameCompatibilityManager.GetTableName(typeof(Rack)));
            }

            if (Schema.Table("hl_RackLevel").Exists())
            {
                Rename.Table("hl_RackLevel").To(NameCompatibilityManager.GetTableName(typeof(RackLevel)));
            }

            if (Schema.Table("hl_RackProduct").Exists())
            {
                Rename.Table("hl_RackProduct").To(NameCompatibilityManager.GetTableName(typeof(RackProduct)));
            }

            if (Schema.Table("hl_RackStock").Exists())
            {
                Rename.Table("hl_RackStock").To(NameCompatibilityManager.GetTableName(typeof(RackStock)));
            }

            if (Schema.Table("hl_ShipmentDispatches").Exists())
            {
                Rename.Table("hl_ShipmentDispatches").To(NameCompatibilityManager.GetTableName(typeof(ShipmentDispatches)));
            }

            if (Schema.Table("hl_ShipmentTransit").Exists())
            {
                Rename.Table("hl_ShipmentTransit").To(NameCompatibilityManager.GetTableName(typeof(ShipmentTransit)));
            }

            if (Schema.Table("hl_ShipmentTransitItems").Exists())
            {
                Rename.Table("hl_ShipmentTransitItems").To(NameCompatibilityManager.GetTableName(typeof(ShipmentTransitItems)));
            }

            if (Schema.Table("hl_StockCount").Exists())
            {
                Rename.Table("hl_StockCount").To(NameCompatibilityManager.GetTableName(typeof(StockCount)));
            }

            if (Schema.Table("hl_StockCountItem").Exists())
            {
                Rename.Table("hl_StockCountItem").To(NameCompatibilityManager.GetTableName(typeof(StockCountItem)));
            }

            if (Schema.Table("hl_WarehouseStockHistory").Exists())
            {
                Rename.Table("hl_WarehouseStockHistory").To(NameCompatibilityManager.GetTableName(typeof(WarehouseStockHistory)));
            }

            if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(ContainerShipmentDispatchMapping))).Exists())
            {
                Create.TableFor<ContainerShipmentDispatchMapping>();
            }

            if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(RackLevelType))).Exists())
            {
                Create.TableFor<RackLevelType>();
            }

            if (Schema.Table(NameCompatibilityManager.GetTableName(typeof(RackStock))).Exists())
            {
                //if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(RackStock))).Column(nameof(RackStock.StockCountId)).Exists())
                //{
                //    Alter.Table(NameCompatibilityManager.GetTableName(typeof(RackStock)))
                //       .AddColumn(nameof(RackStock.StockCountId)).AsInt32().NotNullable().WithDefaultValue(0);
                //}

                if (Schema.Table(NameCompatibilityManager.GetTableName(typeof(RackStock))).Column("StockDate").Exists())
                {
                    Delete.Column("StockDate").FromTable(NameCompatibilityManager.GetTableName(typeof(RackStock)));
                }

                if (Schema.Table(NameCompatibilityManager.GetTableName(typeof(RackStock))).Column("StockCountId").Exists())
                {
                    Delete.Column("StockCountId").FromTable(NameCompatibilityManager.GetTableName(typeof(RackStock)));
                }

                if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(RackStock))).Column(nameof(RackStock.ProductPositionId)).Exists())
                {
                    Alter.Table(NameCompatibilityManager.GetTableName(typeof(RackStock)))
                       .AddColumn(nameof(RackStock.ProductPositionId)).AsInt32().NotNullable().WithDefaultValue(0);
                }
            }

            if (Schema.Table(NameCompatibilityManager.GetTableName(typeof(PalletStock))).Exists())
            {
                //if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(PalletStock))).Column(nameof(PalletStock.StockCountId)).Exists())
                //{
                //    Alter.Table(NameCompatibilityManager.GetTableName(typeof(PalletStock)))
                //       .AddColumn(nameof(PalletStock.StockCountId)).AsInt32().NotNullable().WithDefaultValue(0);
                //}

                if (Schema.Table(NameCompatibilityManager.GetTableName(typeof(PalletStock))).Column("StockDate").Exists())
                {
                    Delete.Column("StockDate").FromTable(NameCompatibilityManager.GetTableName(typeof(PalletStock)));
                }

                if (Schema.Table(NameCompatibilityManager.GetTableName(typeof(PalletStock))).Column("StockCountId").Exists())
                {
                    Delete.Column("StockCountId").FromTable(NameCompatibilityManager.GetTableName(typeof(PalletStock)));
                }

                if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(PalletStock))).Column(nameof(PalletStock.AvailableQuantity)).Exists())
                {
                    Alter.Table(NameCompatibilityManager.GetTableName(typeof(PalletStock)))
                       .AddColumn(nameof(PalletStock.AvailableQuantity)).AsInt32().NotNullable().WithDefaultValue(0);
                }
            }

            if (Schema.Table(NameCompatibilityManager.GetTableName(typeof(WarehouseStockHistory))).Exists())
            {
                if (Schema.Table(NameCompatibilityManager.GetTableName(typeof(WarehouseStockHistory))).Column("RackStockId").Exists())
                {
                    Delete.Column("RackStockId").FromTable(NameCompatibilityManager.GetTableName(typeof(WarehouseStockHistory)));
                }
                if (Schema.Table(NameCompatibilityManager.GetTableName(typeof(WarehouseStockHistory))).Column("PalletStockId").Exists())
                {
                    Delete.Column("PalletStockId").FromTable(NameCompatibilityManager.GetTableName(typeof(WarehouseStockHistory)));
                }

                if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(WarehouseStockHistory))).Column(nameof(WarehouseStockHistory.PalletId)).Exists())
                {
                    Alter.Table(NameCompatibilityManager.GetTableName(typeof(WarehouseStockHistory)))
                       .AddColumn(nameof(WarehouseStockHistory.PalletId)).AsInt32().NotNullable().WithDefaultValue(0);
                }
                if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(WarehouseStockHistory))).Column(nameof(WarehouseStockHistory.RackId)).Exists())
                {
                    Alter.Table(NameCompatibilityManager.GetTableName(typeof(WarehouseStockHistory)))
                       .AddColumn(nameof(WarehouseStockHistory.RackId)).AsInt32().NotNullable().WithDefaultValue(0);
                }
                if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(WarehouseStockHistory))).Column(nameof(WarehouseStockHistory.StockCountId)).Exists())
                {
                    Alter.Table(NameCompatibilityManager.GetTableName(typeof(WarehouseStockHistory)))
                       .AddColumn(nameof(WarehouseStockHistory.StockCountId)).AsInt32().NotNullable().WithDefaultValue(0);
                }
            }
        }

        public override void Down()
        {

        }
        #endregion
    }
}
