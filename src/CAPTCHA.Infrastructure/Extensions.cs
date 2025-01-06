using CAPTCHA.Infrastructure.Data;
using CAPTCHA.Infrastructure.Data.Stores;
using CAPTCHA.Infrastructure.Data.Stores.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CAPTCHA.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddCAPTCHAInfra(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentException("DefaultConnection is null or whitespace.");

            services.AddDbContext<CAPTCHAApplicationDbContext>(options =>
                options.UseSqlite(connectionString));


            services.AddScoped<ICAPTCHAMathQuestionStore, CAPTCHAMathQuestionStore>();

            return services;
        }
    }
}
