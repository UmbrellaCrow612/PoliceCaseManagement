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
            return new CasePermissionDto 
            { 
                CanAssign = @base.CanAssign, 
                CanEdit = @base.CanEdit, 
                CaseId = @base.CaseId, 
                Id = @base.Id, 
                UserId = @base.UserId, 
                UserName = @base.UserName,
                CanAddActions = @base.CanAddActions,
                CanDeleteActions = @base.CanDeleteActions,
                CanDeleteFileAttachments = @base.CanDeleteFileAttachments,
                CanEditActions = @base.CanEditActions,
                CanEditPermissions = @base.CanEditPermissions,
                CanRemoveAssigned = @base.CanRemoveAssigned,
                CanViewActions = @base.CanViewActions,
                CanViewAssigned = @base.CanViewAssigned,
                CanViewFileAttachments = @base.CanViewFileAttachments,
                CanViewPermissions = @base.CanViewPermissions,
                CanEditIncidentType = @base.CanEditIncidentType,
            };
        }

        public void Update(CasePermission @base, UpdateCasePermissionDto updateDto)
        {
            @base.CanEdit = updateDto.CanEdit;
            @base.CanViewPermissions = updateDto.CanViewPermissions;
            @base.CanEditPermissions = updateDto.CanEditPermissions;
            @base.CanViewFileAttachments = updateDto.CanViewFileAttachments;
            @base.CanDeleteFileAttachments = updateDto.CanDeleteFileAttachments;
            @base.CanViewAssigned = updateDto.CanViewAssigned;
            @base.CanAssign = updateDto.CanAssign;
            @base.CanRemoveAssigned = updateDto.CanRemoveAssigned;
            @base.CanViewActions = updateDto.CanViewActions;
            @base.CanAddActions = updateDto.CanAddActions;
            @base.CanEditActions = updateDto.CanEditActions;
            @base.CanDeleteActions = updateDto.CanDeleteActions;
            @base.CanEditIncidentType = updateDto.CanEditIncidentType;
        }
    }
}
