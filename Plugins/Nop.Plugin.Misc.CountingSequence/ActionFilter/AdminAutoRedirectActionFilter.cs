using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Nop.Core;
using Nop.Data;
using Nop.Services.Customers;

namespace Nop.Plugin.Misc.CountingSequence.ActionFilter
{
    /// <summary>
    /// Admin auto redirect action filter
    /// </summary>
    public class AdminAutoRedirectActionFilter : IAsyncActionFilter
    {
        #region Fields

        protected readonly IWebHelper _webHelper;
        protected readonly ICustomerService _customerService;
        protected readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public AdminAutoRedirectActionFilter(
            IWebHelper webHelper,
            ICustomerService customerService,
            IWorkContext workContext)
        {
            _webHelper = webHelper;
            _customerService = customerService;
            _workContext = workContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called after the action executes, before the action result.
        /// </summary>
        /// <param name="context">A context for action filters</param>
        /// <param name="next">A delegate for action filters</param>
        /// <returns>A task that represents the operation</returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (!DataSettingsManager.IsDatabaseInstalled())
            {
                await next();
                return;
            }

            var request = context.HttpContext.Request;

            if (_webHelper.IsAjaxRequest(request))
            {
                await next();
                return;
            }

            if (!request.Method.Equals("GET", StringComparison.InvariantCultureIgnoreCase))
            {
                await next();
                return;
            }

            var routeName = context.ActionDescriptor.AttributeRouteInfo?.Name;

            var returnUrl = string.Empty;

            if (request.Query.TryGetValue("returnUrl", out StringValues url))
                returnUrl = url.ToString();

            var customer = await _workContext.GetCurrentCustomerAsync();
            var isRegistered = await _customerService.IsRegisteredAsync(customer);

            // If already logged in and trying to access login page
            if (routeName != null && routeName.Equals("Login", StringComparison.InvariantCultureIgnoreCase) && isRegistered)
            {
                context.Result = new RedirectResult("/Admin/CountingSequence/List");
                return;
            }

            // If not logged in → redirect to login
            if (!isRegistered && !request.Path.StartsWithSegments("/login") && !request.Path.StartsWithSegments("/logout"))
            {
                context.Result = new RedirectToRouteResult("Login", new RouteValueDictionary(new { returnUrl = "/Admin/CountingSequence/List" }));
                return;
            }

            if (isRegistered &&
                !request.Path.StartsWithSegments("/admin") &&
                !request.Path.StartsWithSegments("/logout") &&
                !request.Path.StartsWithSegments("/login"))
            {
                context.Result = new RedirectResult("/Admin/CountingSequence/List");
                return;
            }

            await next();
        }

        #endregion
    }
}
