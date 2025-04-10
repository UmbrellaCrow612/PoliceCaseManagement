using Cases.API.DTOs;
using Cases.Core.Models;
using Mapper;

namespace Cases.API.Mappings
{
    public class CasesMapping : IMapper<Case, CaseDto, UpdateCaseDto, CreateCaseDto>
    {
        public Case Create(CreateCaseDto createDto)
        {
            return new Case 
            { 
                IncidentDateTime = createDto.IncidentDateTime, 
                CaseNumber = createDto.CaseNumber, 
                Summary = createDto.Summary, 
                Description = createDto.Description 
            };
        }

        public CaseDto ToDto(Case @base)
        {
            return new CaseDto 
            { 
                Id = @base.Id,
                CaseNumber = @base.CaseNumber, 
                Description = @base.Description, 
                IncidentDateTime = @base.IncidentDateTime, 
                LastModifiedDate = @base.LastModifiedDate, 
                Priority = @base.Priority,
                ReportedDateTime = @base.ReportedDateTime,
                Status = @base.Status,
                Summary = @base.Summary
            };
        }

        public void Update(Case @base, UpdateCaseDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
