﻿using AutoMapper;
using PoliceCaseManagement.Application.DTOs.Users;
using PoliceCaseManagement.Application.Exceptions;
using PoliceCaseManagement.Application.Interfaces;
using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Infrastructure.Interfaces;
using PoliceCaseManagement.Shared.Utils;

namespace PoliceCaseManagement.Application.Services
{
    public class UserService(IUserRepository userRepository,IRoleRepository roleRepository, IMapper mapper) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<UserDto> CreateUserAsync(CreateUserDto user)
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

            return _mapper.Map<UserDto>(userToCreate);
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
        {
            if (!await _userRepository.ExistsAsync(userId))
            {
                throw new BusinessRuleException("User dose not exist");
            }

            return await _userRepository.GetRoles(userId);
        }

        public async Task LinkUserToRoles(string userId, IEnumerable<string> roles)
        {
            if (!await _userRepository.ExistsAsync(userId))
            {
                throw new BusinessRuleException("User dose not exists.");
            }

            foreach (var role in roles)
            {
                if (!await _roleRepository.RoleNameExistsAsync(role))
                {
                    throw new BusinessRuleException($"{role} does not exist");
                }

                if (await _roleRepository.UserLinkedToRoleAsync(userId, role))
                {
                    throw new BusinessRuleException($"User already linked to this role {role}");
                }

                await _roleRepository.LinkUserToRoleAsync(userId, role);
            }
        }
    }
}
