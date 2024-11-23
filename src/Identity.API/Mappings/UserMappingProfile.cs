using AutoMapper;
using Identity.API.DTOs;
using Identity.Infrastructure.Data.Models;

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
