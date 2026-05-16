using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Forums;
using Nop.Core.Domain.Gdpr;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Tax;
using Nop.Core.Events;
using Nop.Core.Http;
using Nop.Core.Http.Extensions;
using Nop.Services.Attributes;
using Nop.Services.Authentication;
using Nop.Services.Authentication.External;
using Nop.Services.Authentication.MultiFactor;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.ExportImport;
using Nop.Services.Gdpr;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Tax;
using Nop.Web.Controllers;
using Nop.Web.Factories;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Models.Customer;
using System.Text.Encodings.Web;

namespace Nop.Plugin.Misc.CountingSequence.Controllers
{
    public class OverrideLoginController : CustomerController
    {

        #region Ctor

        public OverrideLoginController(AddressSettings addressSettings,
            CaptchaSettings captchaSettings,
            CustomerSettings customerSettings,
            DateTimeSettings dateTimeSettings,
            ForumSettings forumSettings,
            GdprSettings gdprSettings,
            HtmlEncoder htmlEncoder,
            IAddressModelFactory addressModelFactory,
            IAddressService addressService,
            IAttributeParser<AddressAttribute, AddressAttributeValue> addressAttributeParser,
            IAttributeParser<CustomerAttribute, CustomerAttributeValue> customerAttributeParser,
            IAttributeService<CustomerAttribute, CustomerAttributeValue> customerAttributeService,
            IAuthenticationService authenticationService,
            ICountryService countryService,
            ICurrencyService currencyService,
            ICustomerActivityService customerActivityService,
            ICustomerModelFactory customerModelFactory,
            ICustomerRegistrationService customerRegistrationService,
            ICustomerService customerService,
            IDownloadService downloadService,
            IEventPublisher eventPublisher,
            IExportManager exportManager,
            IExternalAuthenticationService externalAuthenticationService,
            IGdprService gdprService,
            IGenericAttributeService genericAttributeService,
            IGiftCardService giftCardService,
            ILocalizationService localizationService,
            ILogger logger,
            IMultiFactorAuthenticationPluginManager multiFactorAuthenticationPluginManager,
            INewsLetterSubscriptionService newsLetterSubscriptionService,
            INotificationService notificationService,
            IOrderService orderService,
            IPermissionService permissionService,
            IPictureService pictureService,
            IPriceFormatter priceFormatter,
            IProductService productService,
            IStateProvinceService stateProvinceService,
            IStoreContext storeContext,
            ITaxService taxService,
            IWorkContext workContext,
            IWorkflowMessageService workflowMessageService,
            LocalizationSettings localizationSettings,
            MediaSettings mediaSettings,
            MultiFactorAuthenticationSettings multiFactorAuthenticationSettings,
            StoreInformationSettings storeInformationSettings,
            TaxSettings taxSettings) 
            : base(addressSettings, captchaSettings, customerSettings, dateTimeSettings, forumSettings, gdprSettings, htmlEncoder, addressModelFactory, addressService, addressAttributeParser, customerAttributeParser, customerAttributeService, authenticationService, countryService, currencyService, customerActivityService, customerModelFactory, customerRegistrationService, customerService, downloadService, eventPublisher, exportManager, externalAuthenticationService, gdprService, genericAttributeService, giftCardService, localizationService, logger, multiFactorAuthenticationPluginManager, newsLetterSubscriptionService, notificationService, orderService, permissionService, pictureService, priceFormatter, productService, stateProvinceService, storeContext, taxService, workContext, workflowMessageService, localizationSettings, mediaSettings, multiFactorAuthenticationSettings, storeInformationSettings, taxSettings)
        {

        }

        #endregion

        #region Methods

        [HttpPost]
        [ValidateCaptcha]
        //available even when a store is closed
        [CheckAccessClosedStore(ignore: true)]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(ignore: true)]
        public override async Task<IActionResult> Login(LoginModel model, string returnUrl, bool captchaValid)
        {
            // CAPTCHA validation
            if (_captchaSettings.Enabled && _captchaSettings.ShowOnLoginPage && !captchaValid)
            {
                ModelState.AddModelError("", await _localizationService.GetResourceAsync("Common.WrongCaptchaMessage"));
            }

            if (ModelState.IsValid)
            {
                var customerUserName = model.Username;
                var customerEmail = model.Email;
                var userNameOrEmail = _customerSettings.UsernamesEnabled ? customerUserName : customerEmail;

                var loginResult = await _customerRegistrationService.ValidateCustomerAsync(userNameOrEmail, model.Password);

                switch (loginResult)
                {
                    case CustomerLoginResults.Successful:
                        {
                            var customer = _customerSettings.UsernamesEnabled
                                ? await _customerService.GetCustomerByUsernameAsync(customerUserName)
                                : await _customerService.GetCustomerByEmailAsync(customerEmail);

                            await _customerRegistrationService.SignInCustomerAsync(customer, null, model.RememberMe);

                            return Redirect("/Admin/CountingSequence/List");
                        }

                    case CustomerLoginResults.MultiFactorAuthenticationRequired:
                        {
                            var customerMultiFactorAuthenticationInfo = new CustomerMultiFactorAuthenticationInfo
                            {
                                UserName = userNameOrEmail,
                                RememberMe = model.RememberMe,
                                ReturnUrl = returnUrl
                            };

                            await HttpContext.Session.SetAsync(
                                NopCustomerDefaults.CustomerMultiFactorAuthenticationInfo,
                                customerMultiFactorAuthenticationInfo);

                            return RedirectToRoute(NopRouteNames.Standard.MULTIFACTOR_VERIFICATION);
                        }

                    case CustomerLoginResults.CustomerNotExist:
                        ModelState.AddModelError("", await _localizationService.GetResourceAsync("Account.Login.WrongCredentials.CustomerNotExist"));
                        break;

                    case CustomerLoginResults.Deleted:
                        ModelState.AddModelError("", await _localizationService.GetResourceAsync("Account.Login.WrongCredentials.Deleted"));
                        break;

                    case CustomerLoginResults.NotActive:
                        ModelState.AddModelError("", await _localizationService.GetResourceAsync("Account.Login.WrongCredentials.NotActive"));
                        break;

                    case CustomerLoginResults.NotRegistered:
                        ModelState.AddModelError("", await _localizationService.GetResourceAsync("Account.Login.WrongCredentials.NotRegistered"));
                        break;

                    case CustomerLoginResults.LockedOut:
                        ModelState.AddModelError("", await _localizationService.GetResourceAsync("Account.Login.WrongCredentials.LockedOut"));
                        break;

                    case CustomerLoginResults.WrongPassword:
                    default:
                        ModelState.AddModelError("", await _localizationService.GetResourceAsync("Account.Login.WrongCredentials"));
                        break;
                }

                if (loginResult == CustomerLoginResults.WrongPassword && _customerSettings.NotifyFailedLoginAttempt)
                {
                    var customer = _customerSettings.UsernamesEnabled
                        ? await _customerService.GetCustomerByUsernameAsync(customerUserName)
                        : await _customerService.GetCustomerByEmailAsync(customerEmail);

                    await _workflowMessageService.SendCustomerFailedLoginAttemptNotificationAsync(customer, customer.LanguageId ?? 0);
                }

                await _customerActivityService.InsertActivityAsync("PublicStore.FailedLogin",
                    string.Format(await _localizationService.GetResourceAsync("ActivityLog.PublicStore.Login.Fail"),
                    _customerSettings.UsernamesEnabled ? customerUserName : customerEmail));
            }

            model = await _customerModelFactory.PrepareLoginModelAsync(model.CheckoutAsGuest);
            return View(model);
        }

        #endregion
    }
}
