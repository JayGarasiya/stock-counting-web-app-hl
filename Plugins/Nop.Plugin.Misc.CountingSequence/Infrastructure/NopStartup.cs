using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Misc.CountingSequence.ActionFilter;
using Nop.Plugin.Misc.CountingSequence.Controllers;
using Nop.Plugin.Misc.CountingSequence.Events;
using Nop.Plugin.Misc.CountingSequence.Factories;
using Nop.Plugin.Misc.CountingSequence.Services;
using Nop.Services.Events;
using Nop.Services.Security;
using Nop.Web.Controllers;
using Nop.Web.Framework.Events;

namespace Nop.Plugin.Misc.CountingSequence.Infrastructure;

/// <summary>
/// Represents object for the configuring services on application startup
/// </summary>
public class NopStartup : INopStartup
{
    #region Methods

    /// <summary>
    /// Add and configure any of the middleware
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configuration">Configuration of the application</param>
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<CustomerController, OverrideLoginController>();

        //Pallet
        services.AddScoped<IPalletService, PalletService>();
        services.AddScoped<IPalletModelFactory, PalletModelFactory>();
        services.AddScoped<IConsumer<AdminMenuCreatedEvent>, AdminMenuEventConsumer>();

        //Stock
        services.AddScoped<IStockModelFactory, StockModelFactory>();
        services.AddScoped<IStockService, StockService>();

        services.AddScoped<IPermissionConfigManager, CountingSequencePermissionConfigManager>();

        //Rack
        services.AddScoped<IRackService, RackService>();
        services.AddScoped<IRackModelFactory, RackModelFactory>();

        //Counting Sequence
        services.AddScoped<ICountingSequenceService, CountingSequenceService>();
        services.AddScoped<ICountingSequenceModelFactory, CountingSequenceModelFactory>();

        //History
        services.AddScoped<IHistoryService, HistoryService>();
        services.AddScoped<IHistoryModelFactory, HistoryModelFactory>();

        //Shipment Dispatch
        services.AddScoped<IShipmentDispatchService, ShipmentDispatchService>();
        services.AddScoped<IShipmentDispatchModelFactory, ShipmentDispatchModelFactory>();

        //Shipment Transit
        services.AddScoped<IShipmentTransitService, ShipmentTransitService>();
        services.AddScoped<IShipmentTransitModelFactory, ShipmentTransitModelFactory>();
        services.AddScoped<IShipmentTransitItemService, ShipmentTransitItemService>();

        //Channel
        services.AddScoped<IChannelService, ChannelService>();
        services.AddScoped<IChannelModelFactory, ChannelModelFactory>();

        //BackOrders 
        services.AddScoped<IBackOrdersService, BackOrdersService>();
        services.AddScoped<IBackOrdersModelFactory, BackOrdersModelFactory>();

        services.AddScoped<AdminAutoRedirectActionFilter>();

        services.Configure<MvcOptions>(options =>
        {
            options.Filters.AddService<AdminAutoRedirectActionFilter>();
        });
        services.Configure<RazorViewEngineOptions>(options =>
        {
            options.ViewLocationExpanders.Add(new ViewLocationExpander());
        });
    }

    /// <summary>
    /// Configure the using of added middleware
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public void Configure(IApplicationBuilder application)
    {
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets order of this startup configuration implementation
    /// </summary>
    public int Order => 3000;

    #endregion
}