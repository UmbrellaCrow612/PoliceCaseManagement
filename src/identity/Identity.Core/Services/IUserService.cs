namespace Identity.Core.Services
{
    /// <summary>
    /// Service to handle business logic todo with any user related model or action upon the user model
    /// </summary>
    public interface IUserService
    {
      
    }

    /// <summary>
    /// Standard result object for user service operations
    /// </summary>
    public class UserServiceResult : IServiceResult
    {
        public bool Succeeded { get; set; } = false;
        public ICollection<IServiceError> Errors { get; set; } = [];

        public void AddError(string code, string? message = null)
        {
            Errors.Add(new UserServiceError { Code = code, Message = message });
        }

    }

    public class UserServiceError : IServiceError
    {
        public required string Code { get; set; }
        public string? Message { get; set; } = null;
    }
}
