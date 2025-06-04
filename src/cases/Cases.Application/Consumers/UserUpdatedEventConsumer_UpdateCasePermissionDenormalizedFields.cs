using Cases.Infrastructure.Data;
using Events.User;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cases.Application.Consumers
{
    /// <summary>
    /// Listens to <see cref="Events.User.UserUpdatedEvent"/> and updates <see cref="Cases.Core.Models.CasePermission"/> and updated <see cref="Events.IDenormalizedEntity"/> fields for 
    /// user fields
    /// </summary>
    internal class UserUpdatedEventConsumer_UpdateCasePermissionDenormalizedFields(CasesApplicationDbContext dbContext, ILogger<UserUpdatedEventConsumer_UpdateCasePermissionDenormalizedFields> logger) : IConsumer<UserUpdatedEvent>
    {
        private readonly CasesApplicationDbContext _dbContext = dbContext;
        private readonly ILogger<UserUpdatedEventConsumer_UpdateCasePermissionDenormalizedFields> _logger = logger;

        public async Task Consume(ConsumeContext<UserUpdatedEvent> context)
        {
            _logger.LogInformation("{consumer} fired off for event {event} for user ID: {userId}", nameof(UserUpdatedEventConsumer_UpdateCasePermissionDenormalizedFields), nameof(UserUpdatedEvent), context.Message.UserId);

            var newUsername = context.Message.UserName;
            var userId = context.Message.UserId;

            await _dbContext.CasePermissions.Where(ca => ca.UserId == userId)
                .ExecuteUpdateAsync(up => up
                    .SetProperty(p => p.UserName, newUsername));

            _logger.LogInformation("{consumer} finished for event {event} for user ID: {userId}", nameof(UserUpdatedEventConsumer_UpdateCasePermissionDenormalizedFields), nameof(UserUpdatedEvent), context.Message.UserId);

        }
    }
}
