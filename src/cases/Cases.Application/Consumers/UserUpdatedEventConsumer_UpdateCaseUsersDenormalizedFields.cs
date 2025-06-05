using Cases.Core.Models.Joins;
using Cases.Infrastructure.Data;
using Events.Core;
using Events.User;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cases.Application.Consumers
{
    /// <summary>
    /// Listens to <see cref="Events.User.UserUpdatedEvent"/> and updates <see cref="Cases.Core.Models.Case"/> and updated <see cref="Events.IDenormalizedEntity"/> fields for 
    /// user fields
    /// </summary>
    [DenormalisedEventConsumer(nameof(CaseUser))]
    internal class UserUpdatedEventConsumer_UpdateCaseUsersDenormalizedFields(CasesApplicationDbContext dbContext, ILogger<UserUpdatedEventConsumer_UpdateCaseUsersDenormalizedFields> logger) : IConsumer<UserUpdatedEvent>
    {
        private readonly CasesApplicationDbContext _dbContext = dbContext;
        private readonly ILogger<UserUpdatedEventConsumer_UpdateCaseUsersDenormalizedFields> _logger = logger;

        public async Task Consume(ConsumeContext<UserUpdatedEvent> context)
        {
            _logger.LogInformation("{consumer} fired off for event {event} for user ID: {userId}", nameof(UserUpdatedEventConsumer_UpdateCaseUsersDenormalizedFields), nameof(UserUpdatedEvent), context.Message.UserId);

            var newUsername = context.Message.UserName;
            var newEmail = context.Message.Email;
            var userId = context.Message.UserId;

            await _dbContext.CaseUsers.Where(cu => cu.UserId == userId)
                .ExecuteUpdateAsync(up => up
                    .SetProperty(p => p.UserName, newUsername)
                    .SetProperty(p => p.UserEmail, newEmail));

            _logger.LogInformation("{consumer} finished for event {event} for user ID: {userId}", nameof(UserUpdatedEventConsumer_UpdateCaseUsersDenormalizedFields), nameof(UserUpdatedEvent), context.Message.UserId);
        }
    }
}
