using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Misc.CountingSequence.Infrastructure;

/// <summary>
/// Represents plugin route provider
/// </summary>
public class RouteProvider : IRouteProvider
{
    #region Methods

    /// <summary>
    /// Register routes
    /// </summary>
    /// <param name="endpointRouteBuilder">Route builder</param>
    public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapControllerRoute(
            name: "Plugin.Misc.CountingSequence.Configure", 
            pattern: "Admin/CountingSequence/Configure",
            defaults: new { controller = "CountingSequence", action = "Configure" });

        endpointRouteBuilder.MapControllerRoute(
            name: "Plugin.CountingSequence.Login",
            pattern: "login",
            defaults: new { controller = "OverrideLogin", action = "Login" });

        endpointRouteBuilder.MapControllerRoute(
            name: "Plugin.CountingSequenceQr.Generate",
            pattern: "Admin/CountingSequenceQr/Generate",
            defaults: new { controller = "CountingSequenceQr", action = "Generate" });
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets a priority of route provider
    /// </summary>
    public int Priority => int.MinValue;

    #endregion
}