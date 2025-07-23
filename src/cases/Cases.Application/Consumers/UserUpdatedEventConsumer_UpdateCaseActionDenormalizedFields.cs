using Cases.Core.Models;
using Cases.Infrastructure.Data;
using Events.Core;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using User.Events.V1;

namespace Cases.Application.Consumers
{
    /// <summary>
    /// Listens to <see cref="Events.User.UserUpdatedEvent"/> and updates <see cref="Cases.Core.Models.CaseAction"/> and updated <see cref="Events.IDenormalizedEntity"/> fields for 
    /// user fields
    /// </summary>
    [DenormalisedEventConsumer(nameof(CaseAction))]
    internal class UserUpdatedEventConsumer_UpdateCaseActionDenormalizedFields(CasesApplicationDbContext dbContext, ILogger<UserUpdatedEventConsumer_UpdateCaseActionDenormalizedFields> logger) : IConsumer<UserUpdatedEvent>
    {
        private readonly CasesApplicationDbContext _dbcontext = dbContext;
        private readonly ILogger<UserUpdatedEventConsumer_UpdateCaseActionDenormalizedFields> _logger = logger;

        public async Task Consume(ConsumeContext<UserUpdatedEvent> context)
        {
            _logger.LogInformation("{consumer} fired off for event {event} for user ID: {userId}", nameof(UserUpdatedEventConsumer_UpdateCaseActionDenormalizedFields), nameof(UserUpdatedEvent), context.Message.UserId);

            var newUsername = context.Message.UserName;
            var newEmail = context.Message.UserEmail;
            var userId = context.Message.UserId;


            await _dbcontext.CaseActions.Where(ca => ca.CreatedById == userId)
                .ExecuteUpdateAsync(up => up
                    .SetProperty(p => p.CreatedByName, newUsername)
                    .SetProperty(p => p.CreatedByEmail, newEmail));

            _logger.LogInformation("{consumer} finished off for event {event} for user ID: {userId}", nameof(UserUpdatedEventConsumer_UpdateCaseActionDenormalizedFields), nameof(UserUpdatedEvent), context.Message.UserId);
        }
    }
}
