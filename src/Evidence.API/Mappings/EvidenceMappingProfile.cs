using AutoMapper;
using Evidence.API.DTOs.Create;
using Evidence.API.DTOs.Read;
using Evidence.API.DTOs.Update;
using Evidence.Infrastructure.Data.Models;

namespace Evidence.API.Mappings
{
    public class EvidenceMappingProfile : Profile
    {
        public EvidenceMappingProfile()
        {
            CreateMap<CreateEvidenceItemDto, EvidenceItem>();
            CreateMap<EvidenceItem, EvidenceItemDto>();
            CreateMap<UpdateEvidenceItemDto, EvidenceItem>();
        }
    }
}
