using AutoMapper;
using PoliceCaseManagement.Application.DTOs.Users;
using PoliceCaseManagement.Core.Entities;

namespace PoliceCaseManagement.Application.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();
        }
    }
}
