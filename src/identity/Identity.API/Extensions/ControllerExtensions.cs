using Authorization;
using Identity.Application.Constants;
using Identity.Core.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SendGrid;

namespace Identity.API.Extensions
{
    public static class ControllerExtensions
    {
        /// <summary>
        /// Custom method that sets the JWT bearer and refresh tokens in the response cookies
        /// </summary>
        public static void SetAuthCookies(this ControllerBase controller, Tokens tokens, JwtBearerOptions options)
        {
            // TODO make it secure for HTTPS future
            controller.Response.Cookies.Append(CookieNamesConstant.JWT, tokens.JwtBearerToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, 
                SameSite =  SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(options.ExpiresInMinutes),
                IsEssential = true
            });

            controller.Response.Cookies.Append(CookieNamesConstant.REFRESH_TOKEN, tokens.RefreshToken, new CookieOptions
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
            controller.Request.Cookies.TryGetValue(CookieNamesConstant.JWT, out string? jwtCookie);
            controller.Request.Cookies.TryGetValue(CookieNamesConstant.REFRESH_TOKEN, out string? refreshToken);

            if (!string.IsNullOrWhiteSpace(jwtCookie))
            {
               
                var expiredCookie = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(-1),
                    IsEssential = true
                };
                controller.Response.Cookies.Append(CookieNamesConstant.JWT, "", expiredCookie);
            }

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
                controller.Response.Cookies.Append(CookieNamesConstant.REFRESH_TOKEN, "", expiredCookie);
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
