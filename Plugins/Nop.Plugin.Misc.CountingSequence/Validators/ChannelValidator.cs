using FluentValidation;
using Nop.Plugin.Misc.CountingSequence.Domain;
using Nop.Plugin.Misc.CountingSequence.Models.Channel;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Misc.CountingSequence.Validators
{
    /// <summary>
    /// Represents channel model validator
    /// </summary>
    public class ChannelValidator : BaseNopValidator<ChannelModel>
    {
        public ChannelValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Plugins.Misc.CountingSequence.Fields.Name.Required"));
            SetDatabaseValidationRules<Channel>();
        }
    }
}
