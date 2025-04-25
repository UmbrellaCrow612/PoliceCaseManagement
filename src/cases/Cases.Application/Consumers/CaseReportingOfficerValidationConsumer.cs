using Cases.Infrastructure.Data;
using Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Cases.Application.Consumers
{
    public class CaseReportingOfficerValidationConsumer(CasesApplicationDbContext dbContext, ILogger<CaseReportingOfficerValidationConsumer> logger) : IConsumer<CaseReportingOfficerValidationEvent>
    {
        private readonly CasesApplicationDbContext _dbContext = dbContext;
        private readonly ILogger<CaseReportingOfficerValidationConsumer> _logger = logger;

        public async Task Consume(ConsumeContext<CaseReportingOfficerValidationEvent> context)
        {
            _logger.LogInformation("CaseReportingOfficerValidationConsumer fired off");
            var _case = await _dbContext.Cases.FindAsync(context.Message.CaseId);
            if (context.Message.Exists && _case is not null)
            {
                _case.Status = Core.Models.CaseStatus.Reported;
                _dbContext.Update(_case);
                await _dbContext.SaveChangesAsync();


                _logger.LogInformation("case status updated as offcier exists");
            }
        }
    }
}
