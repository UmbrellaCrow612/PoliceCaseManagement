using Identity.API.DTOs;
using Identity.Core.Models;
using Mapper.Core;

namespace Identity.API.Mappings
{
    public class UserMapping : IMapper<ApplicationUser, UserDto, UpdateUserDto, RegisterRequestDto>
    {
        public ApplicationUser Create(RegisterRequestDto createDto)
        {
            return new ApplicationUser { UserName = createDto.UserName, Email = createDto.Email , PhoneNumber = createDto.PhoneNumber, PasswordHash = createDto.Password};
        }

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
