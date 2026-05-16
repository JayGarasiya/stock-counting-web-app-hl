using FluentValidation;
using Nop.Plugin.Misc.CountingSequence.Models.ShipmentTransit;
using Nop.Plugin.Misc.CountingSequence.Services;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Misc.CountingSequence.Validators
{
    /// <summary>
    /// Represents shipment transit model validator
    /// </summary>
    public class ShipmentTransitValidator : BaseNopValidator<ShipmentTransitModel>
    {
        public ShipmentTransitValidator(ILocalizationService localizationService, IShipmentDispatchService shipmentDispatchService, IShipmentTransitService shipmentTransitService)
        {
            RuleFor(x => x.FromDispatchId)
                .GreaterThan(0)
                .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Fields.FromDispatchId.Required"));

        }
    }
}
