using FluentValidation;
using Nop.Data;
using Nop.Plugin.Misc.CountingSequence.Domain;
using Nop.Plugin.Misc.CountingSequence.Models.Pallet;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Misc.CountingSequence.Validators
{
    /// <summary>
    /// Represents pallet model validator
    /// </summary>
    public class PalletValidator : BaseNopValidator<PalletModel>
    {
        public PalletValidator(ILocalizationService localizationService, IRepository<Pallet> palletRepository)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Fields.Name.Required"));
            SetDatabaseValidationRules<Pallet>();

            // Name unique
            RuleFor(x => x.Name)
                .Must((model, name) =>
                {
                    if (string.IsNullOrWhiteSpace(name))
                        return true;

                    var normalizedName = name.Trim().ToLower();

                    return !palletRepository.Table.Any(r =>
                        r.Id != model.Id &&
                        r.Name.Trim().ToLower() == normalizedName);
                })
                .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Fields.Name.Unique"));
        }
    }
}
