using Cases.Core.Models;
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
    [DenormalisedEventConsumer(nameof(Case))]
    internal class UserUpdatedEventConsumer_UpdateCaseDenormalizedFields(CasesApplicationDbContext dbContext, ILogger<UserUpdatedEventConsumer_UpdateCaseDenormalizedFields> logger) : IConsumer<UserUpdatedEvent>
    {
        private readonly CasesApplicationDbContext _dbContext = dbContext;
        private readonly ILogger<UserUpdatedEventConsumer_UpdateCaseDenormalizedFields> _logger = logger;

        public async Task Consume(ConsumeContext<UserUpdatedEvent> context)
        {
            _logger.LogInformation("{consumer} fired off for event {event} for user ID: {userId}", nameof(UserUpdatedEventConsumer_UpdateCaseDenormalizedFields), nameof(UserUpdatedEvent), context.Message.UserId);

            var newUsername = context.Message.UserName;
            var newEmail = context.Message.Email;
            var userId = context.Message.UserId;

            await _dbContext.Cases
            .Where(c => c.ReportingOfficerId == userId)
            .ExecuteUpdateAsync(up => up
                .SetProperty(p => p.ReportingOfficerUserName, newUsername)
                .SetProperty(p => p.ReportingOfficerEmail, newEmail)
                .SetProperty(p => p.LastModifiedDate, DateTime.UtcNow));

            await _dbContext.Cases
              .Where(c => c.CreatedById == userId)
              .ExecuteUpdateAsync(up => up
                  .SetProperty(p => p.CreatedByUserName, newUsername)
                  .SetProperty(p => p.CreatedByEmail, newEmail)
                  .SetProperty(p => p.LastModifiedDate, DateTime.UtcNow));

            _logger.LogInformation("{consumer} fired finished for event {event} for user ID: {userId}", nameof(UserUpdatedEventConsumer_UpdateCaseDenormalizedFields), nameof(UserUpdatedEvent), context.Message.UserId);
        }
    }
}
