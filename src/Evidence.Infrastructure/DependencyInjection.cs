using Evidence.Infrastructure.Data;
using Evidence.Infrastructure.Data.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evidence.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentException("DefaultConnection is empty");

            services.AddDbContext<EvidenceApplicationDbContext>(
                options => options.UseSqlite(connectionString));

            services.AddScoped<IEvidenceItemStore, EvidenceItemStore>();
            services.AddScoped<ICustodyLogStore, CustodyLogStore>();
            services.AddScoped<ILabResultStore, LabResultStore>();
            services.AddScoped<INoteStore, NoteStore>();
            services.AddScoped<IPhotoStore, PhotoStore>();

            return services;
        }
    }
}
