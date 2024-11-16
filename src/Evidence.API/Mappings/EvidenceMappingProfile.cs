using AutoMapper;
using Evidence.API.DTOs;
using Evidence.Infrastructure.Data.Models;

namespace Evidence.API.Mappings
{
    public class EvidenceMappingProfile : Profile
    {
        public EvidenceMappingProfile()
        {
            CreateMap<CreateEvidenceItemDto, EvidenceItem>();
        }
    }
}
