using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace CORS
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Custom way of adding cors to out apps which comes from the config file
        /// </summary>
        public static IApplicationBuilder UseApplicationCors(this IApplicationBuilder app, IConfiguration configuration)
        {
            var config = configuration.GetSection(CORSConfigNamesConstant.Name).Get<CORSConfigSettings>() ?? throw new ApplicationException("Cors missing from settings");

            foreach (var policy in config.Policies)
            {
                app.UseCors(policy.Key);
            }

            return app;
        }
    }
}
