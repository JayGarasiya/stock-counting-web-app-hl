using FluentMigrator;
using Nop.Core.Domain.Security;
using Nop.Data;
using Nop.Data.Migrations;
using Nop.Services.Localization;
using Nop.Services.Security;

namespace Nop.Plugin.Misc.CountingSequence.Data
{
    [NopMigration("2026/05/03 08:08:09:1102530", "Misc.CountingSequence localization", MigrationProcessType.Update)]
    public class DataMigration : Migration
    {
        #region Fields

        protected readonly ILocalizationService _localizationService;
        protected readonly IPermissionService _permissionService;
        protected readonly INopDataProvider _nopDataProvider;

        #endregion

        #region Ctor

        public DataMigration(ILocalizationService localizationService, IPermissionService permissionService, INopDataProvider nopDataProvider)
        {
            _localizationService = localizationService;
            _permissionService = permissionService;
            _nopDataProvider = nopDataProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Collect the UP migration expressions
        /// </summary>
        public override async void Up()
        {
            var permissions = await _permissionService.GetAllPermissionRecordsAsync();

            var existingPermission = permissions.FirstOrDefault(p => p.SystemName == CountingSequenceDefaults.CountingSequnceTabList);
            if (existingPermission != null)
            {
                existingPermission.SystemName = CountingSequenceDefaults.CountingSequnceTabList;
                existingPermission.Name = CountingSequenceDefaults.CountingSequncePermissionName;
                existingPermission.Category = CountingSequenceDefaults.CountingSequence;
                await _permissionService.UpdatePermissionRecordAsync(existingPermission);
            }
            else if (existingPermission == null)
            {
                var permission = new PermissionRecord
                {
                    Name = CountingSequenceDefaults.CountingSequncePermissionName,
                    SystemName = CountingSequenceDefaults.CountingSequnceTabList,
                    Category = CountingSequenceDefaults.CountingSequence
                };

                await _permissionService.InsertPermissionRecordAsync(permission);
            }

            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Misc.CountingSequence.Settings"] = "Settings",
                ["Plugins.Misc.CountingSequence.Fields.Enabled"] = "Enable",
                ["Plugins.Misc.CountingSequence.Fields.Enabled.Hint"] = "Check to enable counting sequence functionality",

                ["Plugins.Misc.CountingSequence.Fields.PalletCountingOrder"] = "Pallet Counting Order",
                ["Plugins.Misc.CountingSequence.Fields.PalletCountingOrder.Hint"] = "Check to pallet counting order sequence functionality",

                ["Plugins.Misc.CountingSequence.Fields.SaleTypeOrder"] = "Sale Type Order",
                ["Plugins.Misc.CountingSequence.Fields.SaleTypeOrder.Hint"] = "Check to sale order sequence functionality",

                ["Plugins.Misc.CountingSequence.Fields.Level"] = "Sequence Level",
                ["Plugins.Misc.CountingSequence.Fields.Level.Hint"] = "Select sequence level to display sequence top to bottom and bottom to top",
                ["Plugins.Misc.CountingSequence.Fields.Position"] = "Sequence Position",
                ["Plugins.Misc.CountingSequence.Fields.Position.Hint"] = "Select sequence position to display sequence left to right and right to left",

                //count search
                ["Plugins.Misc.CountingSequence.StockCountSearch.SearchName"] = "Name",
                ["Plugins.Misc.CountingSequence.StockCountSearch.SearchName.Hint"] = "Seacrh by count name",
                ["Plugins.Misc.CountingSequence.StockCountSearch.SearchWarehouseId"] = "Location",
                ["Plugins.Misc.CountingSequence.StockCountSearch.SearchWarehouseId.Hint"] = "Search by location",
                ["Plugins.Misc.CountingSequence.StockCountSearch.SearchCountTypeId"] = "Count Type",
                ["Plugins.Misc.CountingSequence.StockCountSearch.SearchCountTypeId.Hint"] = "Search by count type",
                ["Plugins.Misc.CountingSequence.StockCountSearch.SearchProgressStatusTypeId"] = "Progress Status",
                ["Plugins.Misc.CountingSequence.StockCountSearch.SearchProgressStatusTypeId.Hint"] = "Search by progress status",
                ["Plugins.Misc.CountingSequence.StockCountSearch.SearchFromDate"] = "Created From",
                ["Plugins.Misc.CountingSequence.StockCountSearch.SearchFromDate.Hint"] = "Search by created from range",
                ["Plugins.Misc.CountingSequence.StockCountSearch.SearchToDate"] = "Created To",
                ["Plugins.Misc.CountingSequence.StockCountSearch.SearchToDate.Hint"] = "Search by created to range",

                ["Enum.Plugins.Misc.CountingSequence.Level.TopToBottom"] = "Top To Bottom",
                ["Enum.Plugins.Misc.CountingSequence.Level.BottomToTop"] = "Bottom To Top",

                ["Enum.Plugins.Misc.CountingSequence.Position.LeftToRight"] = "Left To Right",
                ["Enum.Plugins.Misc.CountingSequence.Position.RightToLeft"] = "Right To Left",

                ["Enum.Plugins.Misc.CountingSequence.ShipmentRouteType.USShipment"] = "US Shipment",
                ["Enum.Plugins.Misc.CountingSequence.ShipmentRouteType.HLShipment"] = "HL Shipment",

                //Pallet  
                ["Plugins.Misc.CountingSequence.Fields.Pallet.Name"] = "Pallet Name",
                ["Plugins.Misc.CountingSequence.Fields.Pallet.Name.Hint"] = "Enter Pallet Name",

                ["Plugins.Misc.CountingSequence.Fields.Pallet.ShipmentDispatchId"] = "Shipment Dispatch",
                ["Plugins.Misc.CountingSequence.Fields.Pallet.ShipmentDispatchId.Hint"] = "Select shipment dispatch type",

                ["Plugins.Misc.CountingSequence.Fields.Pallet.SequenceOrder"] = "SequenceOrder",
                ["Plugins.Misc.CountingSequence.Fields.Pallet.SequenceOrder.Hint"] = "Define display order",

                ["Plugins.Misc.CountingSequence.Fields.Pallet.ShipmentDispatchId.Hint"] = "Define display order",

                ["Plugins.Misc.CountingSequence.Fields.Pallet.DisplayOrder"] = "Display Order",
                ["Plugins.Misc.CountingSequence.Fields.Pallet.DisplayOrder.Hint"] = "Define display order (lower number appears first)",

                ["Plugins.Misc.CountingSequence.Fields.Pallet.IsVisible"] = "Visible",
                ["Plugins.Misc.CountingSequence.Fields.Pallet.IsVisible.Hint"] = "Check to make this item visible",

                ["Plugins.Misc.CountingSequence.Fields.Pallet.Description"] = "Description",
                ["Plugins.Misc.CountingSequence.Fields.Pallet.Description.Hint"] = "Enter pallet description",
                ["Plugins.Misc.CountingSequence.Pallet.ProductId"] = "Product",
                ["Plugins.Misc.CountingSequence.Pallet.PalletId"] = "Pallet",
                ["Plugins.Misc.CountingSequence.Pallet"] = "Pallet",
                ["Plugins.Misc.CountingSequence.Pallets"] = "Pallets",
                ["Plugins.Misc.CountingSequence.CreatePallet"] = "Create Pallet",
                ["Plugins.Misc.CountingSequence.EditPallet"] = "Edit Pallet",

                ["Plugins.Misc.CountingSequence.Menu"] = "Counting Sequence",
                ["Plugins.Misc.CountingSequence.NewCount"] = "New Count",
                ["Plugins.Misc.CountingSequence.StockCountNumber"] = "Stock count number",
                ["Plugins.Misc.CountingSequence.CountCreated"] = "Count created",
                ["Plugins.Misc.CountingSequence.CountCompleted"] = "Count completed",
                ["Plugins.Misc.CountingSequence.EmptyCount"] = "Your count is empty",
                ["Plugins.Misc.CountingSequence.StartNewCount"] = "Start a new count",

                ["Plugins.Misc.CountingSequence.Create"] = "Start",
                ["Plugins.Misc.CountingSequence.NoWarehouseFound"] = "No warehouse found",

                ["Plugins.Misc.CountingSequence.Pallet.CreateMessage"] = "The new pallet has been added successfully.",
                ["Plugins.Misc.CountingSequence.Pallet.UpdateMessage"] = "The pallet has been updated  successfully.",
                ["Plugins.Misc.CountingSequence.Pallet.DeleteMessage"] = "The pallet has been deleted  successfully.",

                ["Plugins.Misc.CountingSequence.Fields.Pallet.PalletId"] = "Pallet",
                ["Plugins.Misc.CountingSequence.Fields.Pallet.ProductId"] = "Product",
                ["Plugins.Misc.CountingSequence.Fields.Pallet.CategoryName"] = "Category Name",
                ["Plugins.Misc.CountingSequence.Fields.Pallet.Specification"] = "Specification",

                ["Plugins.Misc.CountingSequence.Fields.PageSize"] = "Page Size",


                //Rack
                ["Plugins.Misc.CountingSequence.Racks"] = "Racks",
                ["Plugins.Misc.CountingSequence.Rack"] = "Rack",
                ["Plugins.Misc.CountingSequence.Fields.Rack.Name"] = "Rack",
                ["Plugins.Misc.CountingSequence.Fields.Rack.Name.Hint"] = "Enter rack name",

                ["Plugins.Misc.CountingSequence.CreateRack"] = "Create Rack",

                ["Plugins.Misc.CountingSequence.Fields.Rack.Description"] = "Description",
                ["Plugins.Misc.CountingSequence.Fields.Rack.Description.Hint"] = "Enter rack description",

                ["Plugins.Misc.CountingSequence.Fields.Rack.SequenceOrder"] = "Sequence Order",
                ["Plugins.Misc.CountingSequence.Fields.Rack.SequenceOrder.Hint"] = "Define display order",

                ["Plugins.Misc.CountingSequence.Fields.Rack.DisplayOrder"] = "Display Order",
                ["Plugins.Misc.CountingSequence.Fields.Rack.DisplayOrder.Hint"] = "Define display order (lower number appears first)",

                ["Plugins.Misc.CountingSequence.Fields.Rack.IsVisible"] = "Visible",
                ["Plugins.Misc.CountingSequence.Fields.Rack.IsVisible.Hint"] = "Check to make this item visible",

                ["Plugins.Misc.CountingSequence.Fields.Rack.LevelTypeId"] = "Level Type",
                ["Plugins.Misc.CountingSequence.Fields.Rack.LevelTypeId.Hint"] = "Select level type",

                ["Plugins.Misc.CountingSequence.Fields.Actions"] = "Actions",
                ["Plugins.Misc.CountingSequence.Fields.CountComplete"] = "The stock quantity has been added by the rack counting complete #{0}",

                //History
                ["Plugins.Misc.Countingsequence.History"] = "History",
                ["Plugins.Misc.Countingsequence.History.Fields.Product"] = "Product",
                ["Plugins.Misc.Countingsequence.History.Fields.Month"] = "Month",
                ["Plugins.Misc.Countingsequence.History.Fields.Year"] = "Year",
                ["Plugins.Misc.Countingsequence.History.Fields.Type"] = "Type",
                ["Plugins.Misc.Countingsequence.History.Fields.OfUnits"] = "# Of Units",
                ["Plugins.Misc.Countingsequence.History.Fields.Us"] = "Us#",
                ["Plugins.Misc.Countingsequence.History.Fields.HL"] = "H&L#",

                //Start Count
                ["Plugins.Misc.CountingSequence.StartCounting"] = "Start Counting",
                ["Plugins.Misc.CountingSequence.Stock.Fields.Name"] = "Name",
                ["Plugins.Misc.CountingSequence.Stock.Fields.CustomerName"] = "Created By",
                ["Plugins.Misc.CountingSequence.Stock.Fields.WarehouseName"] = "Location",
                ["Plugins.Misc.CountingSequence.Stock.Fields.CountType"] = "Type",
                ["Plugins.Misc.CountingSequence.Stock.Fields.CreatedOnUtc"] = "Created On",
                ["Plugins.Misc.CountingSequence.Stock.Fields.ProgressStatusName"] = "Status",
                ["Plugins.Misc.CountingSequence.Records.Exist"] = "Rack & Pallet record for this warehouse already exist.",
                ["Plugins.Misc.CountingSequence.EmptyRecord.Exist"] = "Count record for this warehouse already exist without any counting.",

                ["Plugins.Misc.CountingSequence.Product"] = "Product",
                ["Plugins.Misc.CountingSequence.Quantity"] = "Quantity",

                //Stock
                ["Plugins.Misc.CountingSequence.Stock"] = "Stock",
                ["Plugins.Misc.CountingSequence.Stock.Product"] = "Product",
                ["Plugins.Misc.CountingSequence.Stock.Rack"] = "Rack",
                ["Plugins.Misc.CountingSequence.Stock.Pallet"] = "Pallet",
                ["Plugins.Misc.CountingSequence.Stock.RackLevel"] = "Rack-Level",
                ["Plugins.Misc.CountingSequence.Stock.NoOfBegUnit"] = "No Of Beg.Unit",
                ["Plugins.Misc.CountingSequence.Stock.NoOfPacks"] = "No Of Packs",
                ["Plugins.Misc.CountingSequence.Stock.NoOfUnits"] = "No Of Units",

                //Shipment Dispatch
                ["Plugins.Misc.CountingSequence.ShipmentDispatch"] = "Shipment Dispatch",
                ["Plugins.Misc.CountingSequence.CreateShipmentDispatch"] = "Create Shipment Dispatch",
                ["Plugins.Misc.CountingSequence.ShipmentDispatch.Fields.Name"] = "Name",
                ["Plugins.Misc.CountingSequence.ShipmentDispatch.Fields.DisplayOrder"] = "Display order",
                ["Plugins.Misc.CountingSequence.ShipmentDispatch.Fields.DispatchType"] = "Dispatch type",
                ["Plugins.Misc.CountingSequence.ShipmentDispatch.Fields.ShippedMonth"] = "Shipped month",
                ["Plugins.Misc.CountingSequence.ShipmentDispatch.Fields.ShippedYear"] = "Shipped year",
                ["Plugins.Misc.CountingSequence.ShipmentDispatch.Fields.Visible"] = "Visible",
                ["Plugins.Misc.CountingSequence.ShipmentDispatch.CreateMessage"] = "The new shipment dispatch has been added successfully.",
                ["Plugins.Misc.CountingSequence.ShipmentDispatch.UpdateMessage"] = "The shipment dispatch has been updated  successfully.",
                ["Plugins.Misc.CountingSequence.ShipmentDispatch.DeleteMessage"] = "The shipment dispatch has been deleted  successfully.",

                ["Plugins.Misc.CountingSequence.SelectShipmentDispatch"] = "Which shipments were delivered since the last count ?",
                ["Plugins.Misc.CountingSequence.ShipmentDispatch.Required"] = "Please select a shipment dispatch.",
                ["Plugins.Misc.CountingSequence.Create.ShipmentDispatch"] = "Accept",

                //Shipment Transit
                ["Plugins.Misc.CountingSequence.ShipmentTransit"] = "Shipment Transit",
                ["Plugins.Misc.CountingSequence.CreateShipmentTransit"] = "Create Shipment Transit",
                ["Plugins.Misc.CountingSequence.Fields.FromDispatchId"] = "US#",
                ["Plugins.Misc.CountingSequence.Fields.FromDispatchId.Hint"] = "Select US dispatch",

                ["Plugins.Misc.CountingSequence.Fields.ToDispatchId"] = "H&L#",
                ["Plugins.Misc.CountingSequence.Fields.ToDispatchId.Hint"] = "Select H&L dispatch",

                ["Plugins.Misc.CountingSequence.Fields.ST.Status"] = "Status",
                ["Plugins.Misc.CountingSequence.Fields.ST.Status.Hint"] = "Select status",

                ["Plugins.Misc.CountingSequence.ShipmentTransit.CreateMessage"] = "The new shipment transit has been added successfully.",
                ["Plugins.Misc.CountingSequence.ShipmentTransit.UpdateMessage"] = "The shipment transit has been updated  successfully.",
                ["Plugins.Misc.CountingSequence.ShipmentTransit.DeleteMessage"] = "The shipment transit has been deleted  successfully.",

                ["Plugins.Misc.CountingSequence.Fields.ST.PalletCount"] = "Pallet Count",
                ["Plugins.Misc.CountingSequence.Fields.ST.PalletCount.Hint"] = "Enter pallet number",

                ["Plugins.Misc.CountingSequence.ShipmentTransitItem.Product"] = "Product",
                ["Plugins.Misc.CountingSequence.ShipmentTransitItem.Quantity"] = "Quantity",
                ["Plugins.Misc.CountingSequence.ShipmentTransitItem.NoOfUnits"] = "No Of Units",
                ["Plugins.Misc.CountingSequence.ShipmentTransitItem.NoOfPacks"] = "No Of Packs",
                ["Plugins.Misc.CountingSequence.ShipmentTransitItem.HAndL"] = "H&L#",
                ["Plugins.Misc.CountingSequence.ShipmentTransitItem.Specification"] = "Specification",
                ["Plugins.Misc.CountingSequence.ShipmentTransitItem.CategoryName"] = "Category name",

                ["Plugins.Misc.CountingSequence.ShipmentTransit.Validtion.Create"] = "This shipment already exists.",
                ["Plugins.Misc.CountingSequence.ShipmentTransit.Validtion.Edit"] = "This shipment cannot be updated because it already exists.",

                //Channel
                ["Plugins.Misc.CountingSequence.Channel"] = "Channel",
                ["Plugins.Misc.CountingSequence.CreateChannel"] = "Create Channel",
                ["Plugins.Misc.CountingSequence.EditChannel"] = "Edit Channel",
                ["Plugins.Misc.CountingSequence.Channel.Fields.ChannelId"] = "Channel Type",
                ["Plugins.Misc.CountingSequence.Channel.Fields.Name"] = "Channel Name",
                ["Plugins.Misc.CountingSequence.Channel.Fields.Description"] = "Description",
                ["Plugins.Misc.CountingSequence.Channel.Fields.DisplayOrder"] = "Display Order",
                ["Plugins.Misc.CountingSequence.Channel.Fields.Visible"] = "Visible",
                ["Plugins.Misc.CountingSequence.Channel.CreateMessage"] = "The new channel has been added successfully.",
                ["Plugins.Misc.CountingSequence.Channel.UpdateMessage"] = "The channel has been updated  successfully.",
                ["Plugins.Misc.CountingSequence.Channel.DeleteMessage"] = "The channel has been deleted  successfully.",

                //Validation
                ["Plugins.Misc.CountingSequence.Fields.Name.Required"] = "Name is required",
                ["Plugins.Misc.CountingSequence.Fields.Name.Unique"] = "This name already exists. Please enter a different name.",
                ["Plugins.Misc.CountingSequence.Fields.FromDispatchId.Required"] = "US# is required",
                ["Plugins.Misc.CountingSequence.Fields.ToDispatchId.Required"] = "H&L# is required",
                ["Plugins.Misc.CountingSequence.Fields.ShippedMonth.Required"] = "To month is required",
                ["Plugins.Misc.CountingSequence.Fields.ShippedYear.Required"] = "To year is required",

                ["Plugins.Misc.CountingSequence.Fields.ShippedMonth.Range"] = "Month must be between 1 and 12",

                //BackOrders
                ["Plugins.Misc.CountingSequence.BackOrders"] = "Back Order",
                ["Plugins.Misc.CountingSequence.BackOrder"] = "Back Order",
                ["Plugins.Misc.CountingSequence.BackOrder.CreateBackOrder"] = "Create Back Order",
                ["Plugins.Misc.CountingSequence.BackOrder.EditBackOrder"] = "Edit Back Order",
                ["Plugins.Misc.CountingSequence.BackOrder.Fields.ReferenceNo"] = "Reference No",
                ["Plugins.Misc.CountingSequence.BackOrder.Fields.ChannelId"] = "Channel",
                ["Plugins.Misc.CountingSequence.BackOrder.Fields.ProductId"] = "Product",
                ["Plugins.Misc.CountingSequence.BackOrder.Fields.Quantity"] = "Quantity",
                ["Plugins.Misc.CountingSequence.BackOrder.Fields.Status"] = "Status",
                ["Plugins.Misc.CountingSequence.BackOrder.Fields.Actions"] = "Actions",
                ["Plugins.Misc.CountingSequence.BackOrder.Fields.OrderDate"] = "Order Date",
                ["Plugins.Misc.CountingSequence.BackOrder.CreateMessage"] = "The new back order has been added successfully.",
                ["Plugins.Misc.CountingSequence.BackOrder.UpdateMessage"] = "The back order has been updated  successfully.",
                ["Plugins.Misc.CountingSequence.BackOrder.DeleteMessage"] = "The back order has been deleted  successfully.",
                ["Plugins.Misc.CountingSequence.Fields.BackOrder.PlaceOrder"] = "The stock quantity has been reduced by placing the back order #{0}",
                ["Plugins.Misc.CountingSequence.Fields.BackOrder.CancelOrder"] = "The stock quantity has been added by cancelling the back order #{0}",

                ["Enum.Plugins.Misc.CountingSequence.ChannelId.Online"] = "Online",
                ["Enum.Plugins.Misc.CountingSequence.ChannelId.Offline"] = "Offline",
                ["Enum.Plugins.Misc.CountingSequence.ChannelId.MarketPlace"] = "MarketPlace",

                ["Enum.Plugins.Misc.CountingSequence.ShipmentTransitStatus.InTransit"] = "InTransit",
                ["Enum.Plugins.Misc.CountingSequence.ShipmentTransitStatus.Partial"] = "Partial",
                ["Enum.Plugins.Misc.CountingSequence.ShipmentTransitStatus.Received"] = "Received",

                ["Plugins.Misc.CountingSequence.ShipmebtTransitItemTitle"] = "Add Shipment Transit Item",
                ["Plugins.Misc.CountingSequence.AddMore"] = "Add More",

                //ChannelSearchModel
                ["Plugins.Misc.CountingSequence.Fields.SearchChannelName"] = "Channel Name",
                ["Plugins.Misc.CountingSequence.Fields.SearchChannelName.Hint"] = "Search by channel name",

                ["Plugins.Misc.CountingSequence.Fields.SearchVisibleId"] = "Visibility",
                ["Plugins.Misc.CountingSequence.Fields.SearchVisibleId.Hint"] = "Filter by visibility status",

                ["Plugins.Misc.CountingSequence.Fields.SearchChannelTypeId"] = "Channel Type",
                ["Plugins.Misc.CountingSequence.Fields.SearchChannelTypeId.Hint"] = "Select channel type",

                ["Plugins.Misc.CountingSequence.Search.All"] = "All",
                ["Plugins.Misc.CountingSequence.Search.VisibleOnly"] = "Visible Only",
                ["Plugins.Misc.CountingSequence.Search.UnvisibleOnly"] = "Hidden Only",


                //StockSearchModel
                ["Plugins.Misc.CountingSequence.Fields.SearchProductName"] = "Product Name",
                ["Plugins.Misc.CountingSequence.Fields.SearchProductName.Hint"] = "Search by product name",

                ["Plugins.Misc.CountingSequence.Fields.SearchRackName"] = "Rack",
                ["Plugins.Misc.CountingSequence.Fields.SearchRackName.Hint"] = "Search by rack name",

                ["Plugins.Misc.CountingSequence.Fields.SearchPalletName"] = "Pallet",
                ["Plugins.Misc.CountingSequence.Fields.SearchPalletName.Hint"] = "Search by pallet name",

                ["Plugins.Misc.CountingSequence.Fields.NoOfUnitMin"] = "No. of Units (Min)",
                ["Plugins.Misc.CountingSequence.Fields.NoOfUnitMin.Hint"] = "Enter minimum number of units",
                ["Plugins.Misc.CountingSequence.Fields.NoOfUnitMax"] = "No. of Units (Max)",
                ["Plugins.Misc.CountingSequence.Fields.NoOfUnitMax.Hint"] = "Enter maximum number of units",

                ["Plugins.Misc.CountingSequence.Fields.NoOfPackMin"] = "No. of Packs (Min)",
                ["Plugins.Misc.CountingSequence.Fields.NoOfPackMin.Hint"] = "Enter minimum number of packs",
                ["Plugins.Misc.CountingSequence.Fields.NoOfPackMax"] = "No. of Packs (Max)",
                ["Plugins.Misc.CountingSequence.Fields.NoOfPackMax.Hint"] = "Enter maximum number of packs",

                //PalletSearchModel
                ["Plugins.Misc.CountingSequence.Fields.SearchPalletName"] = "Pallet Name",
                ["Plugins.Misc.CountingSequence.Fields.SearchPalletName.Hint"] = "Search by pallet name",

                ["Plugins.Misc.CountingSequence.Fields.SearchShipmentDispatchId"] = "Shipment Dispatch",
                ["Plugins.Misc.CountingSequence.Fields.SearchShipmentDispatchId.Hint"] = "Filter by shipment dispatch type",

                ["Plugins.Misc.CountingSequence.Fields.SearchVisible"] = "Visibility",
                ["Plugins.Misc.CountingSequence.Fields.SearchVisible.Hint"] = "Filter by visibility status",

                ["Plugins.Misc.CountingSequence.Fields.SearchProductName"] = "Product Name",
                ["Plugins.Misc.CountingSequence.Fields.SearchProductName.Hint"] = "Search by product name",

                ["Plugins.Misc.CountingSequence.SearchVisible.All"] = "All",
                ["Plugins.Misc.CountingSequence.SearchVisible.VisibleOnly"] = "Visible Only",
                ["Plugins.Misc.CountingSequence.SearchVisible.UnvisibleOnly"] = "Invisible Only",

                ["Plugins.Misc.CountingSequence.Pallet.EditPallet"] = "Edit Pallet",
                ["Plugins.Misc.CountingSequence.Pallet.AddNewPallet"] = "Add New Pallet",
                ["Plugins.Misc.CountingSequence.Pallet.CreatePallet"] = "Create Pallet",
                ["Plugins.Misc.CountingSequence.Pallet.BackToList"] = "Back to list",

                ["Plugins.Misc.CountingSequence.Pallet.SavePalletBeforeProduct"] = "Save pallet before adding products",

                //RackSearchModel
                ["Plugins.Misc.CountingSequence.Fields.SearchRackName"] = "Rack Name",
                ["Plugins.Misc.CountingSequence.Fields.SearchRackName.Hint"] = "Search by rack name",

                ["Plugins.Misc.CountingSequence.Fields.SearchProductId"] = "Product",
                ["Plugins.Misc.CountingSequence.Fields.SearchProductId.Hint"] = "Search by product name",

                ["Plugins.Misc.CountingSequence.Fields.SearchLevelTypeId"] = "Level Type",
                ["Plugins.Misc.CountingSequence.Fields.SearchLevelTypeId.Hint"] = "Select level type",

                ["Plugins.Misc.CountingSequence.Fields.SearchFunctionTypeId"] = "Function Type",
                ["Plugins.Misc.CountingSequence.Fields.SearchFunctionTypeId.Hint"] = "Select Function type",

                ["Plugins.Misc.CountingSequence.Fields.SearchVisible"] = "Visibility",
                ["Plugins.Misc.CountingSequence.Fields.SearchVisible.Hint"] = "Filter by visibility status",

                ["Plugins.Misc.CountingSequence.SearchVisible.All"] = "All",
                ["Plugins.Misc.CountingSequence.SearchVisible.VisibleOnly"] = "Visible Only",
                ["Plugins.Misc.CountingSequence.SearchVisible.UnvisibleOnly"] = "Invisible Only",

                ["Plugins.Misc.CountingSequence.InUsePallet"] = "Which pallets have been moved in full to the racks ?",
                ["Plugins.Misc.CountingSequence.Create.Pallet"] = "Start count",

                ["Plugins.Misc.CountingSequence.Rack.EditRack"] = "Edit Rack",
                ["Plugins.Misc.CountingSequence.Rack.AddNewRack"] = "Add New Rack",
                ["Plugins.Misc.CountingSequence.Rack.CreateRack"] = "Create Rack",
                ["Plugins.Misc.CountingSequence.Rack.BackToList"] = "Back to list",
                ["Plugins.Misc.CountingSequence.Rack.Actions"] = "Actions",
                ["Plugins.Misc.CountingSequence.Rack.SaveRackBeforeProduct"] = "Save rack before adding products",

                //BackSearchModel
                ["Plugins.Misc.CountingSequence.Fields.SearchRefrenceNumber"] = "Reference Number",
                ["Plugins.Misc.CountingSequence.Fields.SearchRefrenceNumber.Hint"] = "Search by reference number",

                ["Plugins.Misc.CountingSequence.Fields.SearchChennel"] = "Channel",
                ["Plugins.Misc.CountingSequence.Fields.SearchChennel.Hint"] = "Search by channel name",

                ["Plugins.Misc.CountingSequence.Fields.Channel.SearchProductId"] = "Product",
                ["Plugins.Misc.CountingSequence.Fields.Channel.SearchProductId.Hint"] = "Search by product name",

                ["Plugins.Misc.CountingSequence.Fields.SearchSatus"] = "Status",
                ["Plugins.Misc.CountingSequence.Fields.SearchSatus.Hint"] = "Select status",

                ["Plugins.Misc.CountingSequence.Fields.MinQuantity"] = "Min Quantity",
                ["Plugins.Misc.CountingSequence.Fields.MinQuantity.Hint"] = "Enter minimum quantity",

                ["Plugins.Misc.CountingSequence.Fields.MaxQuantity"] = "Max Quantity",
                ["Plugins.Misc.CountingSequence.Fields.MaxQuantity.Hint"] = "Enter maximum quantity",

                //ShipmentDispatchSearchModel
                ["Plugins.Misc.CountingSequence.Fields.SearchName"] = "Name",
                ["Plugins.Misc.CountingSequence.Fields.SearchName.Hint"] = "Search by dispatch name",

                ["Plugins.Misc.CountingSequence.Fields.SearchDispatchTypeId"] = "Dispatch Type",
                ["Plugins.Misc.CountingSequence.Fields.SearchDispatchTypeId.Hint"] = "Select dispatch type",

                ["Plugins.Misc.CountingSequence.Fields.SearchMinMonth"] = "Min Month",
                ["Plugins.Misc.CountingSequence.Fields.SearchMinMonth.Hint"] = "Enter minimum month",

                ["Plugins.Misc.CountingSequence.Fields.SearchMaxMonth"] = "Max Month",
                ["Plugins.Misc.CountingSequence.Fields.SearchMaxMonth.Hint"] = "Enter maximum month",

                ["Plugins.Misc.CountingSequence.Fields.SearchMinYear"] = "Min Year",
                ["Plugins.Misc.CountingSequence.Fields.SearchMinYear.Hint"] = "Enter minimum year",

                ["Plugins.Misc.CountingSequence.Fields.SearchMaxYear"] = "Max Year",
                ["Plugins.Misc.CountingSequence.Fields.SearchMaxYear.Hint"] = "Enter maximum year",

                ["Plugins.Misc.CountingSequence.Fields.SearchVisible"] = "Visibility",
                ["Plugins.Misc.CountingSequence.Fields.SearchVisible.Hint"] = "Filter by visibility status",

                //HistorySearchModel

                ["Plugins.Misc.CountingSequence.Fields.HistorySearch.SearchProductId"] = "Search Product",
                ["Plugins.Misc.CountingSequence.Fields.HistorySearch.SearchProductId.Hint"] = "Select a product",

                ["Plugins.Misc.CountingSequence.Fields.HistorySearch.SearchMonth"] = "Search Month",
                ["Plugins.Misc.CountingSequence.Fields.HistorySearch.SearchMonth.Hint"] = "Enter month (e.g., 1 to 12)",

                ["Plugins.Misc.CountingSequence.Fields.HistorySearch.SearchYear"] = "Search Year",
                ["Plugins.Misc.CountingSequence.Fields.HistorySearch.SearchYear.Hint"] = "Enter year (e.g., 2026)",

                ["Plugins.Misc.CountingSequence.Fields.HistorySearch.SearchOfUnit"] = "Search #of Units",
                ["Plugins.Misc.CountingSequence.Fields.HistorySearch.SearchOfUnit.Hint"] = "Enter unit count",

                ["Plugins.Misc.CountingSequence.SearchHistory.UsOnly"] = "Us Only",
                ["Plugins.Misc.CountingSequence.SearchHistory.H&LOnly"] = "H&L Only",

                ["Plugins.Misc.CountingSequence.Fields.HistorySearch.SearchShipment"] = "Shipment",
                ["Plugins.Misc.CountingSequence.Fields.HistorySearch.SearchShipment.Hint"] = "Filter by shimpment status",


                //ShipmentTransitModel
                ["Plugins.Misc.CountingSequence.Fields.SearchFromDisptchId"] = "From Dispatch",
                ["Plugins.Misc.CountingSequence.Fields.SearchFromDisptchId.Hint"] = "Select source dispatch",

                ["Plugins.Misc.CountingSequence.Fields.SearchToDisptchId"] = "To Dispatch",
                ["Plugins.Misc.CountingSequence.Fields.SearchToDisptchId.Hint"] = "Select destination dispatch",

                ["Plugins.Misc.CountingSequence.Fields.ShipmentTransitSearchModel.SearchPalletName"] = "Status",
                ["Plugins.Misc.CountingSequence.Fields.ShipmentTransitSearchModel.SearchPalletName.Hint"] = "Select status",



                //Shipment Transit
                ["Plugins.Misc.CountingSequence.ShipmentTransit.CreateShipmentTransit"] = "Create Shipment Transit",
                ["Plugins.Misc.CountingSequence.ShipmentTransit.EditShipmentTransit"] = "Edit Shipment Transit",
                ["Plugins.Misc.CountingSequence.ShipmentTransit.AddNewShipmentTransit"] = "Add New Shipment Transit",
                ["Plugins.Misc.CountingSequence.ShipmentTransit.BackToList"] = "Back to list",
                ["Plugins.Misc.CountingSequence.ShipmentTransit.Actions"] = "Actions",
                ["Plugins.Misc.CountingSequence.ShipmentTransit.Pallets"] = "Pallets",
                ["Plugins.Misc.CountingSequence.ShipmentTransit.Pallet"] = "Pallet",
                ["Plugins.Misc.CountingSequence.ShipmentTransit.TotalQuantity"] = "Total Quantity",
                ["Plugins.Misc.CountingSequence.ShipmentTransit.Product"] = "Product",
                ["Plugins.Misc.CountingSequence.ShipmentTransit.Quantity"] = "Quantity",
                ["Plugins.Misc.CountingSequence.ShipmentTransit.AddItem"] = "Add Item",
                ["Plugins.Misc.CountingSequence.Fields.ShipmentTransitSearchModel.MaxQuantity.Hint"] = "Enter maximum quantity",

                //Rack
                ["Plugins.Misc.CountingSequence.Rack.Fields.Name"] = "Rack",
                ["Plugins.Misc.CountingSequence.Rack.Fields.Name.Hint"] = "Enter rack name",
                ["Plugins.Misc.CountingSequence.CreateRack"] = "Create Rack",
                ["Plugins.Misc.CountingSequence.Rack.Fields.Description"] = "Description",
                ["Plugins.Misc.CountingSequence.Rack.Fields.Description.Hint"] = "Enter rack description",
                ["Plugins.Misc.CountingSequence.Rack.Fields.SequenceOrder"] = "Sequence Order",
                ["Plugins.Misc.CountingSequence.Rack.Fields.SequenceOrder.Hint"] = "Define display order",
                ["Plugins.Misc.CountingSequence.Rack.Fields.DisplayOrder"] = "Display Order",
                ["Plugins.Misc.CountingSequence.Rack.Fields.DisplayOrder.Hint"] = "Define display order (lower number appears first)",
                ["Plugins.Misc.CountingSequence.Rack.Fields.IsVisible"] = "Visible",
                ["Plugins.Misc.CountingSequence.Rack.Fields.IsVisible.Hint"] = "Check to make this item visible",
                ["Plugins.Misc.CountingSequence.Rack.Fields.FunctionTypeId"] = "Function Type",
                ["Plugins.Misc.CountingSequence.Rack.Fields.FunctionTypeId.Hint"] = "Select function type",
                ["Plugins.Misc.CountingSequence.Rack.Fields.LevelTypeId"] = "Level Type",
                ["Plugins.Misc.CountingSequence.Rack.Fields.LevelTypeId.Hint"] = "Select level type",
                ["Plugins.Misc.CountingSequence.Rack.Fields.ProductId"] = "Product",
                ["Plugins.Misc.CountingSequence.Rack.Fields.CategoryName"] = "Category Name",
                ["Plugins.Misc.CountingSequence.Rack.Fields.Specification"] = "Specification",
                ["Plugins.Misc.CountingSequence.Rack.Fields.ProductPositionId"] = "Product Position",

                ["Plugins.Misc.CountingSequence.Rack.Fields.RackId"] = "Rack",
                ["Plugins.Misc.CountingSequence.Rack.EditRack"] = "Edit rack details",
                ["Plugins.Misc.CountingSequence.Rack.AddNewRack"] = "Add a new rack",
                ["Plugins.Misc.CountingSequence.Rack.CreateRack"] = "Create Rack",
                ["Plugins.Misc.CountingSequence.Rack.BackToList"] = "Back to list",
                ["Plugins.Misc.CountingSequence.Rack.Actions"] = "Actions",
                ["Plugins.Misc.CountingSequence.Rack.SaveRackBeforeProduct"] = "Save rack before adding products",
                ["Plugins.Misc.CountingSequence.Rack.CreateMessage"] = "The new rack has been added successfully.",
                ["Plugins.Misc.CountingSequence.Rack.UpdateMessage"] = "The rack has been updated  successfully.",
                ["Plugins.Misc.CountingSequence.Rack.DeleteMessage"] = "The rack has been deleted  successfully.",

                ["Plugins.Misc.CountingSequence.InUsePallet.Required"] = "At least one pallet is required.",
                ["Plugins.Misc.CountingSequence.InValidStockCount"] = "Invalid stock count. Please refresh the page.",
                ["Plugins.Misc.CountingSequence.Warehouse.Required"] = "Select at least one warehouse to proceed.",
                ["Plugins.Misc.CountingSequence.Warehouse.NotFound"] = "No Records",
                ["Plugins.Misc.CountingSequence.StockCount.ProductInformation"] = "Product Information",
                ["Plugins.Misc.CountingSequence.StockCount.ProductQRCodeImage"] = "QR Code",
                ["Plugins.Misc.CountingSequence.StockCount.ProductOnOtherLocations"] = "Other Locations",
                ["Plugins.Misc.CountingSequence.StockCount.ProductCountSheet"] = "Count Sheet",
                ["Plugins.Misc.CountingSequence.StockCount.BreadcrumbSequence"] = "Breadcrumb (Sequence)",
                ["Plugins.Misc.CountingSequence.StockCount.RecentShipments"] = "Most Recent 6 Shipments",
                ["Plugins.Misc.CountingSequence.StockCount.IncomingShipments"] = "Incoming Shipments",
                ["Plugins.Misc.CountingSequence.StockCount.SaleAndCountHistory"] = "Sale And Count History",
                ["Plugins.Misc.CountingSequence.StockCount.Product.Brand"] = "Brand",
                ["Plugins.Misc.CountingSequence.StockCount.Product.Name"] = "Name",
                ["Plugins.Misc.CountingSequence.StockCount.Product.Quantity"] = "Qty Per Case",
                ["Plugins.Misc.CountingSequence.StockCount.Count"] = "Count",
                ["Plugins.Misc.CountingSequence.StockCount.Cases"] = "Cases",
                ["Plugins.Misc.CountingSequence.StockCount.Units"] = "Units",
                ["Plugins.Misc.CountingSequence.StockCount.TotalAtThisLocation"] = "Total at this location",
                ["Plugins.Misc.CountingSequence.StockCount.Current"] = "Current",
                ["Plugins.Misc.CountingSequence.StockCount.LastMonth"] = "Last Month",
                ["Plugins.Misc.CountingSequence.StockCount.TotalProductCount"] = "Beg. product count",
                ["Plugins.Misc.CountingSequence.StockCount.UnitSales"] = "Unit Sales",
                ["Plugins.Misc.CountingSequence.StockCount.UnitSalesThisMonth"] = "This Month",
                ["Plugins.Misc.CountingSequence.StockCount.UnitSalesLast6Month"] = "Last 6 Month",
                ["Plugins.Misc.CountingSequence.StockCount.BackButton"] = "Back",
                ["Plugins.Misc.CountingSequence.StockCount.SaveAndNext"] = "Save & Continue",
                ["Plugins.Misc.CountingSequence.StockCount.Summary.Complete"] = "Complete",
                ["Plugins.Misc.CountingSequence.StockCount.AddRow"] = "Add Row",
                ["Plugins.Misc.CountingSequence.StockCount.Summary.Product"] = "Product",
                ["Plugins.Misc.CountingSequence.StockCount.Summary.ProductQuantity"] = "Quantity",
                ["Plugins.Misc.CountingSequence.StockCount.Summary.NewProductQuantity"] = "New Quantity",
                ["Plugins.Misc.CountingSequence.StockCount.Summary.RackTitle"] = "Rack Count Completion Report",
                ["Plugins.Misc.CountingSequence.StockCount.Summary.PalletTitle"] = "Pallet Count Completion Report",
                ["Plugins.Misc.CountingSequence.StockCount.Calculator"] = "Calculator",

                ["Plugins.Misc.CountingSequence.ShipmentDispatch.NoRecords"] = "No Record Found",
                ["Plugins.Misc.CountingSequence.Product.QRCode"] = "Generate QR Code",
                ["Admin.Catalog.Products.Fields.OrderMaximumQuantity"] = "No of unit per case",
                ["Plugins.Misc.CountingSequence.Product.QRCodeNotification"] = "QR code generated successfully.",
                ["Plugins.Misc.CountingSequence.Pallet.Complete.Warning"] = "Count will be locked to update. Do you want to continue?",
                ["Plugins.Misc.CountingSequence.Rack.Complete.Warning"] = "Product stock will be reset to current pallet-rack combine stock. Do you want to continue?",
                ["Plugins.Misc.CountingSequence.Count.Delete.Warning"] = "Are you sure you want to delete this counting?",
                ["Plugins.Misc.CountingSequence.StockCount.DeleteMessage"] = "Counting deleted successfully",
                ["Plugins.Misc.CountingSequence.Pallet.NoRecords"] = "Pallet not found to count.",

                //RackLevelType
                ["Plugins.Misc.CountingSequence.RackLevelType"] = "Rack Level",
                ["Plugins.Misc.CountingSequence.Fields.RackLevelType.Name"] = "Name",
                ["Plugins.Misc.CountingSequence.Fields.RackLevelType.FunctionTypeId"] = "Function Type",
                ["Plugins.Misc.CountingSequence.Fields.RackLevelType.DisplayOrder"] = "Display Order",
                ["Plugins.Misc.CountingSequence.Fields.RackLevelType.IsVisible"] = "Visible",

                ["Plugins.Misc.CountingSequence.Fields.SearchRackLevelName"] = "Rack Level Name",
                ["Plugins.Misc.CountingSequence.Fields.SearchRackLevelName.Hint"] = "Search Rack Level Name",
                ["Plugins.Misc.CountingSequence.Fields.SearchFunctionTypeId"] = "Function Type",
                ["Plugins.Misc.CountingSequence.Fields.SearchFunctionTypeId.Hint"] = "Select Function type",
                ["Plugins.Misc.CountingSequence.Fields.RackLevelType.Actions"] = "Actions",

                ["Plugins.Misc.CountingSequence.RackLevelType.CreateMessage"] = "The new rack level has been added successfully.",
                ["Plugins.Misc.CountingSequence.RackLevelType.UpdateMessage"] = "The back order has been updated  successfully.",
                ["Plugins.Misc.CountingSequence.RackLevelType.DeleteMessage"] = "The back order has been deleted  successfully.",
                ["Plugins.Misc.CountingSequence.RackLevelType.CreateRackLevel"] = "Create Rack Level",
                ["Plugins.Misc.CountingSequence.RackLevelType.UpdateRackLevel"] = "Edit Rack Level",
                ["Plugins.Misc.CountingSequence.Fields.RackLevelType.RackLevelTypeId"] = "Function Type",
                ["Plugins.Misc.CountingSequence.Rack.Fields.RackProductPositionId"] = "Position",
            });


            await _nopDataProvider.ExecuteNonQueryAsync(@"
EXEC('
CREATE OR ALTER PROCEDURE [dbo].[FinalizeContainerData]
(
    @ContainerId INT,
    @Action INT
)
AS
BEGIN
    SET NOCOUNT ON;

    IF @Action NOT IN (10, 20, 30)
    BEGIN
        RAISERROR(''Invalid Action. Use 10, 20, 30'', 16, 1);
        RETURN;
    END

    DECLARE @ShipmentId INT;
    DECLARE @TransitId INT;
    DECLARE @ContainerName NVARCHAR(200);

    SELECT @ContainerName = c.Containers
    FROM HL_QR_Temp.dbo.Container c
    WHERE c.Id = @ContainerId;

    IF @ContainerName IS NULL
    BEGIN
        RAISERROR(''Container not found'', 16, 1);
        RETURN;
    END

    ------------------------------------------------------------
    -- CLEANUP
    ------------------------------------------------------------
    IF @Action IN (20, 30)
    BEGIN
        SELECT @ShipmentId = ShipmentDispatchId
        FROM HL_Stock_Counting.dbo.HL_ContainerShipmentDispatchMapping
        WHERE InventoryContainerId = @ContainerId;

        IF @ShipmentId IS NOT NULL
        BEGIN
            DELETE FROM HL_Stock_Counting.dbo.Hl_ShipmentTransitItems
            WHERE ShipmentTransitId IN (
                SELECT Id FROM HL_Stock_Counting.dbo.Hl_ShipmentTransit
                WHERE FromDispatchId = @ShipmentId
            );

            DELETE FROM HL_Stock_Counting.dbo.Hl_ShipmentTransit
            WHERE FromDispatchId = @ShipmentId;

            DELETE FROM HL_Stock_Counting.dbo.Hl_PalletProduct
            WHERE PalletId IN (
                SELECT Id FROM HL_Stock_Counting.dbo.Hl_Pallet
                WHERE ShipmentDispatchId = @ShipmentId
            );

            DELETE FROM HL_Stock_Counting.dbo.Hl_Pallet
            WHERE ShipmentDispatchId = @ShipmentId;

            DELETE FROM HL_Stock_Counting.dbo.Hl_ShipmentDispatches
            WHERE Id = @ShipmentId;
        END

        DELETE FROM HL_Stock_Counting.dbo.HL_ContainerShipmentDispatchMapping
        WHERE InventoryContainerId = @ContainerId;
    END

    IF @Action = 30
        RETURN;

    ------------------------------------------------------------
    -- SOURCE DATA
    ------------------------------------------------------------
    IF OBJECT_ID(''tempdb..#SourceData'') IS NOT NULL DROP TABLE #SourceData;

    SELECT 
        pc.Id AS PalletContainerId,
        pc.PalletId,
        pc.CartonId,
        pc.HashOfCartons,
        carton.ProductModelId,
        carton.Quantity,
        carton.PartialCarton,
        map.CountProductId AS CountingProductId
    INTO #SourceData
    FROM HL_QR_Temp.dbo.PalletContainer pc
    INNER JOIN HL_QR_Temp.dbo.ProductQRGenerated carton 
        ON pc.CartonId = carton.Id
    LEFT JOIN HL_Stock_Counting.dbo.Hl_InventoryAppProductMapping map
        ON map.InventoryProductModelId = carton.ProductModelId
    WHERE TRY_CAST(carton.Container AS INT) = @ContainerId;

    ------------------------------------------------------------
    -- SHIPMENT DISPATCH
    ------------------------------------------------------------
    DECLARE @ShipmentOrder INT;

    SELECT @ShipmentOrder = ISNULL(MAX(DisplayOrder), 0)
    FROM HL_Stock_Counting.dbo.Hl_ShipmentDispatches;

    INSERT INTO HL_Stock_Counting.dbo.Hl_ShipmentDispatches
    (
        Name, DispatchType, ShippedMonth, ShippedYear, DisplayOrder, Visible
    )
    VALUES
    (
        @ContainerName,
        10,
        MONTH(GETDATE()),
        YEAR(GETDATE()),
        @ShipmentOrder + 1,
        1
    );

    SET @ShipmentId = SCOPE_IDENTITY();

    INSERT INTO HL_Stock_Counting.dbo.HL_ContainerShipmentDispatchMapping
    (
        InventoryContainerId,
        ShipmentDispatchId
    )
    VALUES (@ContainerId, @ShipmentId);

    ------------------------------------------------------------
    -- TRANSIT
    ------------------------------------------------------------
    INSERT INTO HL_Stock_Counting.dbo.Hl_ShipmentTransit
    (
        FromDispatchId, Status
    )
    VALUES (@ShipmentId, 10);

    SET @TransitId = SCOPE_IDENTITY();

    ------------------------------------------------------------
    -- PALLETS
    ------------------------------------------------------------
    IF OBJECT_ID(''tempdb..#PalletSource'') IS NOT NULL DROP TABLE #PalletSource;

    DECLARE @BaseOrder INT;

    SELECT @BaseOrder = ISNULL(MAX(DisplayOrder), 0)
    FROM HL_Stock_Counting.dbo.Hl_Pallet;

    SELECT 
        PalletId,
        ROW_NUMBER() OVER (ORDER BY PalletId) + @BaseOrder AS rn
    INTO #PalletSource
    FROM (SELECT DISTINCT PalletId FROM #SourceData) x;

    INSERT INTO HL_Stock_Counting.dbo.Hl_Pallet
    (
        Name, ShipmentDispatchId, DisplayOrder, IsVisible, Description, SequenceOrder
    )
    SELECT 
        @ContainerName + ''_'' + CAST(rn AS NVARCHAR),
        @ShipmentId,
        rn,
        1,
        NULL,
        rn
    FROM #PalletSource;

    ------------------------------------------------------------
    -- PALLET MAP
    ------------------------------------------------------------
    CREATE TABLE #PalletMap
    (
        SourcePalletId INT,
        NewPalletId INT
    );

    INSERT INTO #PalletMap
    SELECT s.PalletId, p.Id
    FROM #PalletSource s
    INNER JOIN HL_Stock_Counting.dbo.Hl_Pallet p
        ON p.ShipmentDispatchId = @ShipmentId
       AND p.SequenceOrder = s.rn;

    ------------------------------------------------------------
    -- PALLET PRODUCT
    ------------------------------------------------------------
    INSERT INTO HL_Stock_Counting.dbo.Hl_PalletProduct
    (
        PalletId, ProductId
    )
    SELECT DISTINCT
        pm.NewPalletId,
        s.CountingProductId
    FROM #SourceData s
    INNER JOIN #PalletMap pm
        ON pm.SourcePalletId = s.PalletId
    WHERE s.CountingProductId IS NOT NULL;

    ------------------------------------------------------------
    -- TRANSIT ITEMS
    ------------------------------------------------------------
    INSERT INTO HL_Stock_Counting.dbo.Hl_ShipmentTransitItems
    (
        ShipmentTransitId, ProductId, PalletId, Quantity, NoOfUnits, NoOfPacks, ToDispatchId
    )
    SELECT 
        @TransitId,
        s.CountingProductId,
        pm.NewPalletId,
        CASE 
            WHEN s.PartialCarton = 1 THEN ISNULL(s.Quantity, 0)
            ELSE ISNULL(s.HashOfCartons, 0) * ISNULL(s.Quantity, 0)
        END,
        0,
        CASE 
            WHEN s.PartialCarton = 1 THEN 1
            ELSE ISNULL(s.HashOfCartons, 0)
        END,
        0
    FROM #SourceData s
    INNER JOIN #PalletMap pm
        ON pm.SourcePalletId = s.PalletId
    WHERE s.CountingProductId IS NOT NULL;

    ------------------------------------------------------------
    -- OUTPUT
    ------------------------------------------------------------
    SELECT 
        @ShipmentId AS ShipmentId,
        @TransitId AS TransitId;

END
');
");
        }
        public override void Down()
        {

        }
        #endregion
    }
}
