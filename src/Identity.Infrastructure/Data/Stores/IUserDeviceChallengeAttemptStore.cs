using Identity.Infrastructure.Data.Models;

namespace Identity.Infrastructure.Data.Stores
{
    public interface IUserDeviceChallengeAttemptStore
    {
        IQueryable<UserDeviceChallengeAttempt> UserDeviceChallengeAttempts { get; }

        Task<(bool canMakeAttempt, ICollection<string> Errors)> AddAttempt(UserDeviceChallengeAttempt attempt);
        Task<(bool isValid, UserDeviceChallengeAttempt? attempt)> ValidateAttempt(string email, string code);
    }
}
