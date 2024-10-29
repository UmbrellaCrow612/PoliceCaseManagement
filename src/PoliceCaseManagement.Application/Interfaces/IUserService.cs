using PoliceCaseManagement.Application.DTOs.Users;

namespace PoliceCaseManagement.Application.Interfaces
{
    public interface IUserService
    {
        Task CreateUser(CreateUserDto user);
    }
}
