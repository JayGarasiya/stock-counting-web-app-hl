using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Misc.CountingSequence.Events
{
    /// <summary>
    /// Represents plugin event consumer
    /// </summary>
    public class AdminMenuEventConsumer : IConsumer<AdminMenuCreatedEvent>
    {
        #region Fields

        protected readonly IAdminMenu _adminMenu;
        protected readonly ILocalizationService _localizationService;

        #endregion

        #region Ctor

        public AdminMenuEventConsumer(IAdminMenu adminMenu, ILocalizationService localizationService)
        {
            _adminMenu = adminMenu;
            _localizationService = localizationService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handle admin menu created event
        /// </summary>
        /// <param name="eventMessage">Event message</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task HandleEventAsync(AdminMenuCreatedEvent eventMessage)
        {
            var rootNode = eventMessage.RootMenuItem;

            var helpMenu = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Help");

            if (helpMenu != null)
            {
                // remove completely
                rootNode.ChildNodes.Remove(helpMenu);
            }

            var dashboardMenu = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Dashboard");

            if (dashboardMenu != null)
            {
                // remove completely
                rootNode.ChildNodes.Remove(dashboardMenu);
            }

            if (!rootNode.ChildNodes.Any(x => x.SystemName == CountingSequenceDefaults.CountingSequence))
            {
                rootNode.ChildNodes.Add(new AdminMenuItem
                {
                    SystemName = CountingSequenceDefaults.CountingSequence,
                    Title = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Menu"),
                    Url = _adminMenu.GetMenuItemUrl("CountingSequence", "List"),
                    IconClass = "fas fa-calculator",
                    Visible = true,
                    PermissionNames = new List<string> { CountingSequenceDefaults.CountingSequnceTabList }
                });
            }

            if (!rootNode.ChildNodes.Any(x => x.SystemName == CountingSequenceDefaults.CountingSequencePallet))
            {
                rootNode.ChildNodes.Add(new AdminMenuItem
                {
                    SystemName = CountingSequenceDefaults.CountingSequencePallet,
                    Title = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Pallet"),
                    Url = _adminMenu.GetMenuItemUrl("Pallet", "List"),
                    IconClass = "fas fa-pallet",
                    Visible = true,
                    PermissionNames = new List<string> { CountingSequenceDefaults.CountingSequnceTabList }
                });
            }

            if (!rootNode.ChildNodes.Any(x => x.SystemName == CountingSequenceDefaults.CountingSequenceRack))
            {
                rootNode.ChildNodes.Add(new AdminMenuItem
                {
                    SystemName = CountingSequenceDefaults.CountingSequenceRack,
                    Title = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Rack"),
                    Url = _adminMenu.GetMenuItemUrl("Rack", "List"),
                    IconClass = "fas fa-layer-group",
                    Visible = true,
                    PermissionNames = new List<string> { CountingSequenceDefaults.CountingSequnceTabList }
                });
            }

            if (!rootNode.ChildNodes.Any(x => x.SystemName == CountingSequenceDefaults.CountingSequenceRackLevelType))
            {
                rootNode.ChildNodes.Add(new AdminMenuItem
                {
                    SystemName = CountingSequenceDefaults.CountingSequenceRackLevelType,
                    Title = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.RackLevelType"),
                    Url = _adminMenu.GetMenuItemUrl("Rack", "LevelList"),
                    IconClass = "fa-solid fa-list",
                    Visible = true,
                    PermissionNames = new List<string> { CountingSequenceDefaults.CountingSequnceTabList }
                });
            }

            if (!rootNode.ChildNodes.Any(x => x.SystemName == CountingSequenceDefaults.CountingSequenceStock))
            {
                rootNode.ChildNodes.Add(new AdminMenuItem
                {
                    SystemName = CountingSequenceDefaults.CountingSequenceStock,
                    Title = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Stock"),
                    Url = _adminMenu.GetMenuItemUrl("Stock", "List"),
                    IconClass = "fas fa-warehouse",
                    Visible = true,
                    PermissionNames = new List<string> { CountingSequenceDefaults.CountingSequnceTabList }
                });
            }

            if (!rootNode.ChildNodes.Any(x => x.SystemName == CountingSequenceDefaults.CountingSequenceHistory))
            {
                rootNode.ChildNodes.Add(new AdminMenuItem
                {
                    SystemName = CountingSequenceDefaults.CountingSequenceHistory,
                    Title = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.History"),
                    Url = _adminMenu.GetMenuItemUrl("History", "List"),
                    IconClass = "fas fa-history",
                    Visible = true,
                    PermissionNames = new List<string> { CountingSequenceDefaults.CountingSequnceTabList }
                });
            }

            if (!rootNode.ChildNodes.Any(x => x.SystemName == CountingSequenceDefaults.CountingSequenceShipmentDispatch))
            {
                rootNode.ChildNodes.Add(new AdminMenuItem
                {
                    SystemName = CountingSequenceDefaults.CountingSequenceShipmentDispatch,
                    Title = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.ShipmentDispatch"),
                    Url = _adminMenu.GetMenuItemUrl("ShipmentDispatch", "List"),
                    IconClass = "fas fa-truck-loading",
                    Visible = true,
                    PermissionNames = new List<string> { CountingSequenceDefaults.CountingSequnceTabList }
                });
            }

            if (!rootNode.ChildNodes.Any(x => x.SystemName == CountingSequenceDefaults.CountingSequenceShipmentTransit))
            {
                rootNode.ChildNodes.Add(new AdminMenuItem
                {
                    SystemName = CountingSequenceDefaults.CountingSequenceShipmentTransit,
                    Title = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.ShipmentTransit"),
                    Url = _adminMenu.GetMenuItemUrl("ShipmentTransit", "List"),
                    IconClass = "fas fa-shipping-fast",
                    Visible = true,
                    PermissionNames = new List<string> { CountingSequenceDefaults.CountingSequnceTabList }
                });
            }

            if (!rootNode.ChildNodes.Any(x => x.SystemName == CountingSequenceDefaults.CountingSequenceChannel))
            {
                rootNode.ChildNodes.Add(new AdminMenuItem
                {
                    SystemName = CountingSequenceDefaults.CountingSequenceChannel,
                    Title = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Channel"),
                    Url = _adminMenu.GetMenuItemUrl("Channel", "List"),
                    IconClass = "fas fa-store",
                    Visible = true,
                    PermissionNames = new List<string> { CountingSequenceDefaults.CountingSequnceTabList }
                });
            }

            if (!rootNode.ChildNodes.Any(x => x.SystemName == CountingSequenceDefaults.CountingSequnceBackOrders))
            {
                rootNode.ChildNodes.Add(new AdminMenuItem
                {
                    SystemName = CountingSequenceDefaults.CountingSequnceBackOrders,
                    Title = await _localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.BackOrders"),
                    Url = _adminMenu.GetMenuItemUrl("BackOrders", "List"),
                    IconClass = "fas fa-network-wired",
                    Visible = true,
                    PermissionNames = new List<string> { CountingSequenceDefaults.CountingSequnceTabList }
                });
            }
        }

        #endregion
    }
}
