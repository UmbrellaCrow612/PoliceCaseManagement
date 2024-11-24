using AutoMapper;
using Identity.API.DTOs;
using Identity.Infrastructure.Data.Models;

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
