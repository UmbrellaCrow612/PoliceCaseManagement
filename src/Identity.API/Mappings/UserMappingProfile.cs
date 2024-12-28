using AutoMapper;
using Identity.API.DTOs;
using Identity.Core.Models;

namespace Identity.API.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UpdateUserDto, ApplicationUser>();
            CreateMap<ApplicationUser, UserDto>();
        }
    }
}
