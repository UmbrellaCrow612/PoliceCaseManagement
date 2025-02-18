using Authorization.Core;
using Identity.API.Services.Interfaces;
using Identity.API.Settings;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Extensions
{
    public static class ControllerExtensions
    {
        /// <summary>
        /// Custom method that sets the JWT bearer and refresh tokens in the response cookies
        /// </summary>
        public static void SetAuthCookies(this ControllerBase controller, Tokens tokens, JwtBearerOptions options)
        {
            controller.Response.Cookies.Append(CookieNamesConstant.JWT, tokens.JwtBearerToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(options.ExpiresInMinutes)
            });

            controller.Response.Cookies.Append(CookieNamesConstant.REFRESH_TOKEN, tokens.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(options.RefreshTokenExpiriesInMinutes)
            });
        }

        /// <summary>
        /// Return the current requesting device information to a <see cref="DeviceInfo"/>
        /// </summary>
        public static DeviceInfo ComposeDeviceInfo(this ControllerBase controller)
        {
            /// we assume that <see cref="Annotations.RequireDeviceInformationAttribute"/> has been run on the endpoint
            return new DeviceInfo
            {
                DeviceFingerPrint = controller.Request.Headers[CustomHeaderOptions.XDeviceFingerprint].FirstOrDefault()!, 
                IpAddress = controller.Request.HttpContext.Connection.RemoteIpAddress?.ToString()
                                ?? controller.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                                ?? "Unknown",
                UserAgent = controller.Request.Headers.UserAgent.ToString()
            };
        }
    }

}
