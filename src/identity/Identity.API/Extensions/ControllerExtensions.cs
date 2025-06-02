using Authorization;
using Identity.Application.Constants;
using Identity.Application.Helpers;
using Identity.Core.ValueObjects;
using Microsoft.AspNetCore.Mvc;


namespace Identity.API.Extensions
{
    public static class ControllerExtensions
    {
        /// <summary>
        /// Custom method that sets the refresh http only cookie
        /// </summary>
        public static void SetAuthCookies(this ControllerBase controller, Tokens tokens, JwtBearerOptions options)
        {
            controller.Response.Cookies.Append(AuthCookieNamesConstant.REFRESH_TOKEN, tokens.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(options.RefreshTokenExpiriesInMinutes),
                IsEssential = true
            });
        }

        /// <summary>
        /// Used when logging out a user to remove the HTTP only cookies from the user browser as this cannot be done through JS
        /// </summary>
        public static void RemoveAuthCookies(this ControllerBase controller)
        {
            controller.Request.Cookies.TryGetValue(AuthCookieNamesConstant.REFRESH_TOKEN, out string? refreshToken);

            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                var expiredCookie = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(-1),
                    IsEssential = true
                };
                controller.Response.Cookies.Append(AuthCookieNamesConstant.REFRESH_TOKEN, "", expiredCookie);
            }
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
