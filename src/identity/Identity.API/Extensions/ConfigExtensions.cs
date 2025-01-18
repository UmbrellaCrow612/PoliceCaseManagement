﻿using Identity.API.Settings;

namespace Identity.API.Extensions
{
    public static class ConfigExtensions
    {
        public static IServiceCollection AddIdentityApplicationRedirectUrls(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<IdentityApplicationRedirectUrls>()
                .Bind(configuration.GetSection("identity-redirect-urls"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            return services;
        }
    }
}
