using AutoMapper;
using PoliceCaseManagement.Application.DTOs.Roles;
using PoliceCaseManagement.Core.Entities;

namespace PoliceCaseManagement.Application.Mappings
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<CreateRoleDto, Role>();
            CreateMap<Role, RoleDto>();
            CreateMap<UpdateRoleDto, Role>();
        }
    }
}
