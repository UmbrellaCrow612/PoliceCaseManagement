using Events;
using Identity.Infrastructure.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Identity.Application.Consumers
{
    internal class CaseActionCreatedEventConsumer(IdentityApplicationDbContext dbContext, IPublishEndpoint publishEndpoint, ILogger<CaseActionCreatedEventConsumer> logger) : IConsumer<CaseActionCreatedEvent>
    {
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly ILogger<CaseActionCreatedEventConsumer> _logger = logger;

        public async Task Consume(ConsumeContext<CaseActionCreatedEvent> context)
        {
            _logger.LogInformation("CaseActionCreatedEventConsumer fired off");

            var userExists = await _dbcontext.Users.AnyAsync(x => x.Id == context.Message.CreatedById);
            await _publishEndpoint.Publish(new CaseActionCreatedByValidationEvent { CreatedByUserExists = userExists, CaseActionId = context.Message.CaseActionId});
        }
    }
}
