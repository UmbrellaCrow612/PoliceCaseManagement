using Identity.Application.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Identity.API.Annotations
{
    /// <summary>
    /// Use this attribute to validate the device information in the request needed for <see cref="Core.ValueObjects.DeviceInfo"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequireDeviceInformationAttribute : Attribute, IResourceFilter
    {
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var httpContext = context.HttpContext;
            var request = httpContext.Request;
            var errors = new List<string>();

            string? ipAddress;
            if (request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
            {
                ipAddress = forwardedFor.ToString().Split(',').FirstOrDefault()?.Trim();
            }
            else
            {
                ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();
            }

            if (string.IsNullOrWhiteSpace(ipAddress) || ipAddress == IPAddress.None.ToString())
            {
                errors.Add("Missing IP address.");
            }

            if (!request.Headers.TryGetValue(CustomHeaderOptions.XDeviceFingerprint, out var deviceFingerprint) ||
                string.IsNullOrWhiteSpace(deviceFingerprint))
            {
                errors.Add($"Missing {CustomHeaderOptions.XDeviceFingerprint} header.");
            }

            if (!request.Headers.TryGetValue("User-Agent", out var userAgent) ||
                string.IsNullOrWhiteSpace(userAgent))
            {
                errors.Add("Missing User-Agent header.");
            }

            if (errors.Count != 0)
            {
                context.Result = new BadRequestObjectResult(new
                {
                    Error = "Missing required request information.",
                    Details = errors
                });
            }
        }
    }
}
