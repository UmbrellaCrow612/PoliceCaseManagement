using Cases.API.DTOs;
using Cases.Core.Models;
using Mapper;

namespace Cases.API.Mappings
{
    public class CaseAccessListMapping : IMapper<CaseAccessList, CaseAccessListDto, EmptyDto, EmptyDto>
    {
        public CaseAccessList Create(EmptyDto createDto)
        {
            throw new NotImplementedException();
        }

        public CaseAccessListDto ToDto(CaseAccessList @base)
        {
            return new CaseAccessListDto
            {
                CaseId = @base.CaseId,
                CaseRole = @base.CaseRole,
                Id = @base.Id,
                UserEmail = @base.UserEmail,
                UserId = @base.UserId,
                UserName = @base.UserName,
            };
        }

        public void Update(CaseAccessList @base, EmptyDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
