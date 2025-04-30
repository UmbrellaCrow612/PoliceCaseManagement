using Cases.Infrastructure.Data;
using Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Cases.Application.Consumers
{
    internal class CaseActionCreatedByValidationEventConsumer(CasesApplicationDbContext dbContext, ILogger<CaseActionCreatedByValidationEventConsumer> logger) : IConsumer<CaseActionCreatedByValidationEvent>
    {
        private readonly CasesApplicationDbContext _dbcontext = dbContext;
        private readonly ILogger<CaseActionCreatedByValidationEventConsumer> _logger = logger;

        public async Task Consume(ConsumeContext<CaseActionCreatedByValidationEvent> context)
        {
            _logger.LogInformation("CaseActionCreatedByValidationEventConsumer fired off");

            var action = await _dbcontext.CaseActions.FindAsync(context.Message.CaseActionId);
            if (action is not null)
            {
                if (context.Message.CreatedByUserExists)
                {
                    action.ValidationStatus = Core.Models.CaseActionValidationStatus.Valid;
                } else
                {
                    action.ValidationStatus = Core.Models.CaseActionValidationStatus.Invalid;
                }

                _dbcontext.CaseActions.Update(action);
                await _dbcontext.SaveChangesAsync();
            }
        }
    }
}
