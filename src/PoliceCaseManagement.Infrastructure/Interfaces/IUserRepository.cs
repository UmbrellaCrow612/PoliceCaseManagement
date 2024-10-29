namespace PoliceCaseManagement.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> UsernameExists(string username); 
        Task<bool> EmailExists (string email);
    }
}
