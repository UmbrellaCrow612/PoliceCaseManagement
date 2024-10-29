using AutoMapper;
using PoliceCaseManagement.Application.DTOs.Users;
using PoliceCaseManagement.Application.Interfaces;
using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Core.Exceptions;
using PoliceCaseManagement.Infrastructure.Interfaces;
using PoliceCaseManagement.Shared.Utils;

namespace PoliceCaseManagement.Application.Services
{
    public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        public async Task CreateUser(CreateUserDto user)
        {
            if (await _userRepository.UsernameExistsAsync(user.Username))
            {
                throw new BusinessRuleException("Username already exists");
            }

            if(await _userRepository.EmailExistsAsync(user.Email))
            {
                throw new BusinessRuleException("Email is already taken");
            }

            var userToCreate = _mapper.Map<User>(user);

            var hashedPassword = PasswordHasher.HashPassword(user.Password);
            userToCreate.PasswordHash = hashedPassword;

            await _userRepository.AddAsync(userToCreate);
        }
    }
}
