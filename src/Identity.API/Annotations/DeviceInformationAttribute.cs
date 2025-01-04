using Identity.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Identity.API.Annotations
{
    [AttributeUsage(AttributeTargets.Method)]
    public class DeviceInformationAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var services = context.HttpContext.RequestServices;

            var deviceService = services.GetService<DeviceManager>();
            if (deviceService is null)
            {
                context.Result = new ObjectResult("DeviceManager service not found")
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
                return;
            }

            var (isValid, errors) = deviceService.VerifyRequestHasRequiredProperties(context.HttpContext.Request);
            if (!isValid)
            {
                context.Result = new BadRequestObjectResult(new
                {
                    Message = "Device request validation failed.",
                    Errors = errors
                });
                return;
            }
        }
    }
}
