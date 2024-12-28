using AutoMapper;
using Identity.API.DTOs;
using Identity.Core.Models;

namespace Identity.API.Mappings
{
    public class TokenMappingProfile : Profile
    {
        public TokenMappingProfile()
        {
            CreateMap<Token, QueryTokenDto>();
        }
    }
}
