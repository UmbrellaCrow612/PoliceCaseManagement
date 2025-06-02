using Identity.API.DTOs;
using Identity.Core.Models;
using Mapper;

namespace Identity.API.Mappings
{
    public class UserMapping : IMapper<ApplicationUser, UserDto, UpdateUserDto, RegisterRequestDto>
    {
        public ApplicationUser Create(RegisterRequestDto createDto)
        {
            return new ApplicationUser { UserName = createDto.UserName, Email = createDto.Email, PhoneNumber = createDto.PhoneNumber };
        }

        public UserDto ToDto(ApplicationUser user)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                UserName = user.UserName!,
                PhoneNumber = user.PhoneNumber!

            };
        }

        public RestrictedUserDto ToRestrictedUserDto(ApplicationUser user)
        {
            return new RestrictedUserDto { Id = user.Id, UserName = user.UserName! };
        }

        public void Update(ApplicationUser @base, UpdateUserDto updateDto)
        {
            @base.UserName = updateDto.UserName;
            @base.Email = updateDto.Email;
            @base.PhoneNumber = updateDto.PhoneNumber;
        }
    }
}
