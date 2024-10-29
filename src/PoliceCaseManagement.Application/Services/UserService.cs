using AutoMapper;
using PoliceCaseManagement.Application.Interfaces;
using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Infrastructure.Interfaces;

namespace PoliceCaseManagement.Application.Services
{
    public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;
    }
}
