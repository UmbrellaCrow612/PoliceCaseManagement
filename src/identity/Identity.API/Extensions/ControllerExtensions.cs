using Authorization.Core;
using Identity.API.Settings;
using Identity.Application.Constants;
using Identity.Core.ValueObjects;
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
            // Change this flag manually or configure it dynamically (e.g., from appsettings.json)
            bool isProduction = true; // Set to true in production

            controller.Response.Cookies.Append(CookieNamesConstant.JWT, tokens.JwtBearerToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = isProduction,  // ✅ Secure=true for production, false for local testing
                SameSite = isProduction ? SameSiteMode.None : SameSiteMode.Lax, // ✅ None for production, Lax for localhost
                Expires = DateTime.UtcNow.AddMinutes(options.ExpiresInMinutes)
            });

            controller.Response.Cookies.Append(CookieNamesConstant.REFRESH_TOKEN, tokens.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = isProduction,  // ✅ Secure=true for production, false for local testing
                SameSite = isProduction ? SameSiteMode.None : SameSiteMode.Lax, // ✅ None for production, Lax for localhost
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
