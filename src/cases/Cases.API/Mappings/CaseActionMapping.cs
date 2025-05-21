using Cases.API.DTOs;
using Cases.Core.Models;
using Mapper;

namespace Cases.API.Mappings
{
    public class CaseActionMapping : IMapper<CaseAction, CaseActionDto, UpdateCaseActionDto, CreateCaseActionDto>
    {
        public CaseAction Create(CreateCaseActionDto createDto)
        {
            return new CaseAction
            {
                Description = createDto.Description,
                Notes = createDto.Notes,
            };
        }

        public CaseActionDto ToDto(CaseAction @base)
        {
            return new CaseActionDto
            {
                CreatedAt = @base.CreatedAt,
                CreatedById = @base.CreatedById,
                Description = @base.Description,
                Id = @base.Id,
                Notes = @base.Notes,
                CreatedByEmail = @base.CreatedByEmail,
                CreatedByName = @base.CreatedByName,
            };
        }

        public void Update(CaseAction @base, UpdateCaseActionDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
