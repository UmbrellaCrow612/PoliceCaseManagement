using Identity.API.Settings;

namespace Identity.API.Extensions
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
