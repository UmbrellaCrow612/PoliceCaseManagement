using Identity.Application.Settings;
using Microsoft.AspNetCore.Builder;

namespace Identity.Application
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseApplicationCors(this IApplicationBuilder app, CORSConfigSettings corsConfig)
        {
            foreach (var policy in corsConfig.Policies)
            {
                app.UseCors(policy.Key);
            }

            return app;
        }
    }

}
