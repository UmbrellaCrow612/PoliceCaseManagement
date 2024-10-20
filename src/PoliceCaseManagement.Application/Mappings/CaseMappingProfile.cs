using AutoMapper;
using PoliceCaseManagement.Application.DTOs.Cases;
using PoliceCaseManagement.Core.Entities;

namespace PoliceCaseManagement.Application.Mappings
{
    public class CaseMappingProfile : Profile
    {
        public CaseMappingProfile() 
        {
            CreateMap<Case, CaseDto>();
            CreateMap<CreateCaseDto, Case>();
            CreateMap<UpdateCaseDto, Case>();
        }
    }
}
