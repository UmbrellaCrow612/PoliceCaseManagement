using Events;
using Identity.Infrastructure.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Identity.Application.Consumers
{
    public class CaseCreatedConsumer(IdentityApplicationDbContext dbContext, IPublishEndpoint publishEndpoint, ILogger<CaseCreatedConsumer> logger) : IConsumer<CaseCreatedEvent>
    {
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly ILogger<CaseCreatedConsumer> _logger = logger;

        public async Task Consume(ConsumeContext<CaseCreatedEvent> context)
        {
            _logger.LogInformation("CaseCreatedConsumer fired off");

            var offcierId = context.Message.ReportingOffcierId;

            var exists = await _dbcontext.Users.AnyAsync(x => x.Id == offcierId);

            await _publishEndpoint.Publish(new CaseReportingOfficerValidationEvent { Exists = exists, CaseId = context.Message.CaseId });

            _logger.LogInformation("CaseReportingOfficerValidationEvent fired off");
        }
    }
}
