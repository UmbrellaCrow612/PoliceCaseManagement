using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Core.Interfaces;
using PoliceCaseManagement.Infrastructure.Data;
using PoliceCaseManagement.Infrastructure.Data.Repositories;

namespace PoliceCaseManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ICaseRepository<Case, string>, CaseRepository>();
            services.AddScoped<ICrimeSceneRepository<CrimeScene, string>, CrimeSceneRepository>();
            services.AddScoped<IDepartmentRepository<Department, string>, DepartmentRepository>();
            services.AddScoped<IDocumentRepository<Document, string>, DocumentRepository>();
            services.AddScoped<IEvidenceRepository<Evidence, string>, EvidenceRepository>();
            services.AddScoped<IIncidentRepository<Incident, string>, IncidentRepository>();
            services.AddScoped<ILocationRepository<Location, string>, LocationRepository>();
            services.AddScoped<IPersonRepository<Person, string>, PersonRepository>();
            services.AddScoped<IPropertyRepository<Property, string>, PropertyRepository>();
            services.AddScoped<IReportRepository<Report, string>, ReportRepository>();
            services.AddScoped<IStatementRepository<Statement, string>, StatementRepository>();
            services.AddScoped<ITagRepository<Tag, string>, TagRepository>();
            services.AddScoped<IUserRepository<User, string>, UserRepository>();
            services.AddScoped<IVehicleRepository<Vehicle, string>, VehicleRepository>();

            return services;
        }
    }
}
