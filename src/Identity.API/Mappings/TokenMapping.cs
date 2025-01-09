using Identity.API.DTOs;
using Identity.Core.Models;

namespace Identity.API.Mappings
{
    public class TokenMapping : IMapper<Token, TokenDto, UpdateTokenDto>
    {
        public TokenDto ToDto(Token @base)
        {
            return new TokenDto { Id = @base.Id };
        }

        public void Update(Token @base, UpdateTokenDto updateDto)
        {
            @base.IsRevoked = updateDto.IsRevoked;
        }
    }
}
