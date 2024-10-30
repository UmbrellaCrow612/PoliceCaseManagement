using AutoMapper;
using PoliceCaseManagement.Application.DTOs.Roles;
using PoliceCaseManagement.Application.Interfaces;
using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Core.Exceptions;
using PoliceCaseManagement.Infrastructure.Interfaces;

namespace PoliceCaseManagement.Application.Services
{
    public class RoleService(IRoleRepository roleRepository, IUserRepository userRepository, IMapper mapper) : IRoleService
    {
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        public async Task AddRoleAsync(CreateRoleDto role)
        {
            if(await _roleRepository.RoleNameExistsAsync(role.Name))
            {
                throw new BusinessRuleException("Role already exists");
            }

            var roleToCreate = _mapper.Map<Role>(role);

            await _roleRepository.AddAsync(roleToCreate);
        }

        public async Task LinkUserToRoleAsync(string userId, string roleName)
        {
            if (!await _roleRepository.RoleNameExistsAsync(roleName))
            {
                throw new BusinessRuleException("Role dose not exist.");
            }

            if (!await _userRepository.ExistsAsync(userId))
            {
                throw new BusinessRuleException("User dose not exist");
            }

            if(!await _roleRepository.UserLinkedToRoleAsync(userId, roleName))
            {
                throw new BusinessRuleException("User already linked to role.");
            }

            await _roleRepository.LinkUserToRoleAsync(userId, roleName);
        }

        public Task RoleNameExistsAsync(string roleName)
        {
            return _roleRepository.RoleNameExistsAsync(roleName);
        }
    }
}
