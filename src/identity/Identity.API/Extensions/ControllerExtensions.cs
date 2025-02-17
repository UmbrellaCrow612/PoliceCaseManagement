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
    }

}
