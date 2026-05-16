using FluentValidation;
using Nop.Data;
using Nop.Plugin.Misc.CountingSequence.Domain;
using Nop.Plugin.Misc.CountingSequence.Models.Rack;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Misc.CountingSequence.Validators
{
    /// <summary>
    /// Represents rack model validator
    /// </summary>
    public class RackValidator : BaseNopValidator<RackModel>
    {
        public RackValidator(ILocalizationService localizationService, IRepository<Rack> rackRepository)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Fields.Name.Required"));
            SetDatabaseValidationRules<Rack>();

            // Name unique
            RuleFor(x => x.Name)
                .Must((model, name) =>
                {
                    if (string.IsNullOrWhiteSpace(name))
                        return true;

                    var normalizedName = name.Trim().ToLower();

                    return !rackRepository.Table.Any(r =>
                        r.Id != model.Id &&
                        r.Name.Trim().ToLower() == normalizedName);
                })
                .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Fields.Name.Unique"));
        }
    }
}
