using Identity.API.DTOs;
using Identity.Core.Models;

namespace Identity.API.Mappings
{
    public class UserMapping : IMapper<ApplicationUser, UserDto, UpdateUserDto>
    {
        public UserDto ToDto(ApplicationUser @base)
        {
            return new UserDto
            {
                Id = @base.Id,
            };
        }

        public void Update(ApplicationUser @base, UpdateUserDto updateDto)
        {
            @base.UserName = updateDto.UserName;
            @base.Email = updateDto.Email;
            @base.PhoneNumber = updateDto.PhoneNumber;
        }
    }
}
