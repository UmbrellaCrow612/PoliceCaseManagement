using Identity.API.DTOs;
using Identity.Core.Models;
using Mapper.Core;

namespace Identity.API.Mappings
{
    public class TokenMapping : IMapper<Token, TokenDto, UpdateTokenDto, Token>
    {
        [Obsolete("This method dose not make sense for tokens - we dont get a token dto becuase we create it internally.")]
        public Token Create(Token createDto)
        {
            throw new NotImplementedException();
        }

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
