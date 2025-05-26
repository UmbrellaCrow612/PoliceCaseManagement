using Cases.Infrastructure.Data;
using Events.User;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cases.Application.Consumers
{
    /// <summary>
    /// Run logic for this service off when a <see cref="Events.User.UserUpdatedEvent"/> is fired off, will attempt update all models that refer to a local copy of the user
    /// </summary>
    internal class UserUpdatedEventConsumer(CasesApplicationDbContext dbContext, ILogger<UserUpdatedEventConsumer> logger) : IConsumer<UserUpdatedEvent>
    {
        private readonly CasesApplicationDbContext _dbContext = dbContext;
        private readonly ILogger<UserUpdatedEventConsumer> _logger = logger;

        public async Task Consume(ConsumeContext<UserUpdatedEvent> context)
        {
            var nameOfConsumer = nameof(UserUpdatedEventConsumer);
            var nameOfEvent = nameof(UserUpdatedEvent);

            _logger.LogInformation("{consumer}: Fired off for event {event}", nameOfConsumer, nameOfEvent);

            var newUsername = context.Message.UserName;
            var newEmail = context.Message.Email;
            var userId = context.Message.UserId;

            _logger.LogInformation("{consumer}: Attempting to update all cases that refer to the given user {userId}", nameOfConsumer, userId);

            await _dbContext.Cases
            .Where(c => c.ReportingOfficerId == userId)
            .ExecuteUpdateAsync(up => up
                .SetProperty(p => p.ReportingOfficerUserName, newUsername)
                .SetProperty(p => p.ReportingOfficerEmail, newEmail)
                .SetProperty(p => p.LastModifiedDate, DateTime.UtcNow));

            _logger.LogInformation("{consumer}: Finished updating all cases that refer to the given user {userId}", nameOfConsumer, userId);


            _logger.LogInformation("{consumer}: Attempting to update all case actions that refer to the given user {userId}", nameOfConsumer, userId);

            await _dbContext.CaseActions.Where(ca => ca.CreatedById == userId)
                .ExecuteUpdateAsync(up => up
                    .SetProperty(p => p.CreatedByName, newUsername)
                    .SetProperty(p => p.CreatedByEmail, newEmail));


            _logger.LogInformation("{consumer}: Attempting to update all case users that refer to the given user {userId}", nameOfConsumer, userId);

            await _dbContext.CaseUsers.Where(cu => cu.UserId == userId)
                .ExecuteUpdateAsync(up => up
                    .SetProperty(p => p.UserName, newUsername)
                    .SetProperty(p => p.UserEmail, newEmail));

            _logger.LogInformation("{consumer}: Finished updating all case users that refer to the given user {userId}", nameOfConsumer, userId);


            _logger.LogInformation("{consumer}: Finished off for event {event} updating all local copy fields for user {userId}", nameOfConsumer, nameOfEvent, userId);
        }
    }
}
