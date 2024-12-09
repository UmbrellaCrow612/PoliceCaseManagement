using Identity.Infrastructure.Data.Models;

namespace Identity.Infrastructure.Data.Stores
{
    public interface ILoginAttemptStore
    {
        IQueryable<LoginAttempt> LoginAttempts { get; }

        /// <summary>
        /// Adds login attempt to context tracking - dose not call save changes
        /// </summary>
        Task SetLoginAttempt(LoginAttempt loginAttempt);
        Task StoreLoginAttempt(LoginAttempt loginAttempt);
        Task<LoginAttempt?> GetLoginAttemptById(string loginAttemptId);
        Task<ICollection<LoginAttempt>> GetUserLoginAttempts(ApplicationUser user);
        void SetToUpdateAttempt(LoginAttempt loginAttempt);
    }
}
