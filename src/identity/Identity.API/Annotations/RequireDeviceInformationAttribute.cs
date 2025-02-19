using Identity.Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Identity.API.Annotations
{
    /// <summary>
    /// Use this attribute to validate the device information in the request.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class RequireDeviceInformationAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var (isValid, errors) = DeviceManager.VerifyRequestHasRequiredProperties(context.HttpContext.Request);
            if (!isValid)
            {
                context.Result = new BadRequestObjectResult(errors); // we expect at least one error if not somthing wrong with verify process to lead here
                return;
            }
        }
    }
}
