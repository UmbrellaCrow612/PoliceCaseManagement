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
            return new CasePermissionDto { CanAssign = @base.CanAssign, CanEdit = @base.CanEdit, CaseId = @base.CaseId, Id = @base.Id, UserId = @base.UserId, UserName = @base.UserName };
        }

        public void Update(CasePermission @base, UpdateCasePermissionDto updateDto)
        {
            @base.CanAssign = updateDto.CanAssign;
            @base.CanEdit = updateDto.CanEdit;
        }
    }
}
