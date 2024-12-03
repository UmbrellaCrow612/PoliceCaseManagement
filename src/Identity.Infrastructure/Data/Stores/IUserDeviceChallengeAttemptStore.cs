using Identity.Infrastructure.Data.Models;

namespace Identity.Infrastructure.Data.Stores
{
    public interface IUserDeviceChallengeAttemptStore
    {
        IQueryable<UserDeviceChallengeAttempt> UserDeviceChallengeAttempts { get; }

        void SetToUpdateAttempt(UserDeviceChallengeAttempt attempt);
        Task UpdateAttemptAsync(UserDeviceChallengeAttempt attempt);
        Task<(bool canMakeAttempt, ICollection<string> Errors)> AddAttempt(UserDeviceChallengeAttempt attempt);
        Task<(bool isValid, UserDeviceChallengeAttempt? attempt)> ValidateAttemptAsync(string email, string code);
    }
}
