using Cases.API.DTOs;
using Cases.Core.Models;
using Mapper;

namespace Cases.API.Mappings
{
    public class CasePermissionMapping : IMapper<CasePermission, CasePermissionDto, UpdateCasePermissionDto, CreateCaseActionDto>
    {
        public CasePermission Create(CreateCaseActionDto createDto)
        {
            throw new NotImplementedException();
        }

        public CasePermissionDto ToDto(CasePermission @base)
        {
            throw new NotImplementedException();
        }

        public void Update(CasePermission @base, UpdateCasePermissionDto updateDto)
        {
            @base.
        }
    }
}
