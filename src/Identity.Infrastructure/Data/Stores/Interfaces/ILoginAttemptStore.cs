using Identity.Core.Models;

namespace Identity.Infrastructure.Data.Stores.Interfaces
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
