using Reporting.Core.Models;

namespace Reporting.Infrastructure.Data.Stores.Interfaces
{
    public interface IReportingStore
    {
        IQueryable<Report> Reports { get; }

        Task CreateAsync(Report report);

        Task<Report?> FindByIdAsync(string reportId);
    }
}
