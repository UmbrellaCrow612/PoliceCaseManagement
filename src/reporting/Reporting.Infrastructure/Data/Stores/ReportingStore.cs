using Reporting.Core.Models;
using Reporting.Infrastructure.Data.Stores.Interfaces;

namespace Reporting.Infrastructure.Data.Stores
{
    internal class ReportingStore(ReportingApplicationDbContext dbContext) : IReportingStore
    {
        private readonly ReportingApplicationDbContext _dbContext = dbContext;

        public IQueryable<Report> Reports => _dbContext.Reports.AsQueryable();

        public async Task CreateAsync(Report report)
        {
            await _dbContext.Reports.AddAsync(report);
        }

        public async Task<Report?> FindByIdAsync(string reportId)
        {
            return await _dbContext.Reports.FindAsync(reportId);
        }
    }
}
