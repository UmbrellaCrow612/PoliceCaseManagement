using Microsoft.Extensions.DependencyInjection;

namespace Grpc.JwtInterceptor
{
    public static class Extensions
    {
        /// <summary>
        /// Adds <see cref="JwtForwardInterceptor"/> to the DI and <see cref=" Microsoft.AspNetCore.Http.HttpContextAccessor"/> for it to work
        /// </summary>
        public static IServiceCollection AddGrpcJwtInterceptor(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddSingleton<JwtForwardInterceptor>();

            return services;
        }
    }
}
