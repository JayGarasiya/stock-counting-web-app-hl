using FluentValidation;
using Nop.Plugin.Misc.CountingSequence.Domain;
using Nop.Plugin.Misc.CountingSequence.Models.ShipmentDispatch;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Misc.CountingSequence.Validators
{
    /// <summary>
    /// Represents shipment dispatch model validator
    /// </summary>
    public class ShipmentDispatchValidator : BaseNopValidator<ShipmentDispatchModel>
    {
        public ShipmentDispatchValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Fields.Name.Required"));
            RuleFor(x => x.ShippedMonth).GreaterThan(0).WithMessageAwait(localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Fields.ShippedMonth.Required"));
            RuleFor(x => x.ShippedMonth).InclusiveBetween(1, 12).WithMessageAwait(localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Fields.ShippedMonth.Range"));
            RuleFor(x => x.ShippedYear).GreaterThan(0).WithMessageAwait(localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Fields.ShippedYear.Required"));

            SetDatabaseValidationRules<ShipmentDispatches>();
        }
    }
}
