using Cases.API.DTOs;
using Cases.Core.Models.Joins;
using Mapper;

namespace Cases.API.Mappings
{
    public class CasePersonMapping : IMapper<CasePerson, CasePersonDto, EmptyDto, EmptyDto>
    {
        public CasePerson Create(EmptyDto createDto)
        {
            throw new NotImplementedException();
        }

        public CasePersonDto ToDto(CasePerson @base)
        {
            return new CasePersonDto
            {
                CaseId = @base.CaseId,
                Id = @base.Id,
                PersonDateBirth = @base.PersonDateBirth,
                PersonEmail = @base.PersonEmail,
                PersonFirstName = @base.PersonFirstName,
                PersonId = @base.PersonId,
                PersonLastName = @base.PersonLastName,
                PersonPhoneNumber = @base.PersonPhoneNumber,
                Role = @base.Role,
            };
        }

        public void Update(CasePerson @base, EmptyDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
