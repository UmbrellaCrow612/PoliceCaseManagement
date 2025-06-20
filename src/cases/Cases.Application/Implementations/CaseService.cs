using System.Reflection;
using Cases.Application.Codes;
using Cases.Core.Models;
using Cases.Core.Models.Joins;
using Cases.Core.Services;
using Cases.Core.ValueObjects;
using Cases.Infrastructure.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StorageProvider.Abstractions;
using StorageProvider.AWS;
using User.V1;

namespace Cases.Application.Implementations
{
    internal class CaseService(CasesApplicationDbContext dbContext, IPublishEndpoint publishEndpoint, ILogger<CaseService> logger, UserValidationService userValidationService, IStorageProvider storageProvider, IOptions<AWSSettings> options) : ICaseService
    {
        private readonly CasesApplicationDbContext _dbcontext = dbContext;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly ILogger<CaseService> _logger = logger;
        private readonly UserValidationService _userValidationService = userValidationService;
        private readonly AWSSettings _awsSettings = options.Value;
        private readonly IStorageProvider _storageProvider = storageProvider;

        public Task<CaseResult> AddAttachment(Case @case, string file)
        {
            throw new NotImplementedException();
        }

        public async Task<CaseResult> AddCaseAction(Case @case, CaseAction caseAction)
        {
            _logger.LogInformation("Started trying to add a case action: {caseActionId} to a case: {caseId}", caseAction.Id, @case.Id);

            var result = new CaseResult();

            if (caseAction.CaseId is null)
            {
                result.AddError(BusinessRuleCodes.ValidationError, "Case Id for case action is null");
                return result;
            }

            if (caseAction.CaseId != @case.Id)
            {
                result.AddError(BusinessRuleCodes.ValidationError, "Case action not properly linked to the provided case id");
                return result;
            }

            if (caseAction.CreatedById is null)
            {
                result.AddError(BusinessRuleCodes.ValidationError, "Case action created by ID is null");
                return result;
            }

            var userExists = await _userValidationService.DoesUserExistAsync(caseAction.CreatedById);
            if (!userExists)
            {
                result.AddError(BusinessRuleCodes.ValidationError, "Created by ID user dose not exist");
                return result;
            }

            var deNormUserDetails = await _userValidationService.GetUserById(caseAction.CreatedById);
            caseAction.CreatedByName = deNormUserDetails.Username;
            caseAction.CreatedByEmail = deNormUserDetails.Email;

            await _dbcontext.CaseActions.AddAsync(caseAction);
            await _dbcontext.SaveChangesAsync();

            _logger.LogInformation("Saved and added case action: {caseActionId} to case: {caseId}.", caseAction.Id, @case.Id);

            result.Succeeded = true;
            return result;
        }

        public async Task<CaseResult> AddToIncidentType(Case @case, IncidentType incidentType)
        {
            var result = new CaseResult();

            var isIncidentTypeAlreadyLinkedToCase = await _dbcontext.CaseIncidentTypes.AnyAsync(x => x.CaseId == @case.Id && x.IncidentTypeId == incidentType.Id);
            if (isIncidentTypeAlreadyLinkedToCase)
            {
                result.AddError(BusinessRuleCodes.IncidentTypeAlreadyLinkedToCase, $@"Incident type ${incidentType.Name} already linked to case.");
                return result;
            }

            var join = new CaseIncidentType
            {
                CaseId = @case.Id,
                IncidentTypeId = incidentType.Id
            };
            await _dbcontext.CaseIncidentTypes.AddAsync(join);
            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<CaseResult> AddUsers(Case @case, List<string> userIds)
        {
            var result = new CaseResult();

            if (userIds.Count < 1)
            {
                result.Succeeded = true;
                return result;
            }

            var currentAssignedUsers = await _dbcontext.CaseUsers.Where(x => x.CaseId == @case.Id).ToListAsync();

            bool anyMatch = currentAssignedUsers.Any(cu => userIds.Contains(cu.UserId)); // we do not want to assign users currently assigned
            if (anyMatch)
            {
                result.AddError(BusinessRuleCodes.ValidationError, "Cannot assign a user to this case who is already assigned to it");
                return result;
            }

            foreach (var userId in userIds)
            {
                var exists = await _userValidationService.DoesUserExistAsync(userId);
                if (!exists)
                {
                    result.AddError(BusinessRuleCodes.ValidationError, "User to assign dose not exist");
                    return result;
                }
            }

            List<CasePermission> defaultCasePermissions = [];
            List<GetUserByIdResponse> userDetails = [];
            foreach (var userId in userIds)
            {
                userDetails.Add(await _userValidationService.GetUserById(userId));
            }

            foreach (var user in userDetails)
            {
                defaultCasePermissions.Add(new CasePermission
                {
                    CanAssign = false,
                    CanEdit = false,
                    CaseId = @case.Id,
                    UserId = user.UserId,
                    UserName = user.Username,
                    CanAddActions = false,
                    CanDeleteActions = false,
                    CanDeleteFileAttachments = false,
                    CanEditActions = false,
                    CanEditPermissions = false,
                    CanRemoveAssigned = false,
                    CanViewActions = false,
                    CanViewAssigned = false,
                    CanViewFileAttachments = false,
                    CanViewPermissions = false,
                    CanEditIncidentType = false,
                });
            }

            List<CaseUser> linksToThisCase = [.. userDetails.Select(x => new CaseUser { CaseId = @case.Id, UserEmail = x.Email, UserId = x.UserId, UserName = x.Username })];
            await _dbcontext.CaseUsers.AddRangeAsync(linksToThisCase);

            await _dbcontext.CasePermissions.AddRangeAsync(defaultCasePermissions);

            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<CaseResult> CanUserCreateCaseActions(string caseId, string userId)
        {
            var result = new CaseResult();

            var hasPerm = await _dbcontext.CasePermissions.Where(x => x.CaseId == caseId && x.UserId == userId && x.CanAddActions == true).AnyAsync();
            if (!hasPerm)
            {
                result.AddError(BusinessRuleCodes.CasePermissions);
                return result;
            }

            result.Succeeded = true;
            return result;
        }

        public async Task<CaseResult> CanUserEditCasePermissions(string caseId, string userId)
        {
            var result = new CaseResult();
            var hasPerm = await _dbcontext.CasePermissions.Where(x => x.CaseId == caseId && x.UserId == userId && x.CanEditPermissions == true).AnyAsync();
            if (!hasPerm)
            {
                result.AddError(BusinessRuleCodes.CasePermissions);
                return result;
            }

            result.Succeeded = true;
            return result;
        }

        public async Task<CaseResult> CanUserViewCaseActions(string caseId, string userId)
        {
            var result = new CaseResult();
            var hasPermission = await _dbcontext.CasePermissions.Where(x => x.CaseId == caseId && x.UserId == userId && x.CanViewActions == true).AnyAsync();
            if (!hasPermission)
            {
                result.AddError(BusinessRuleCodes.CasePermissions);
                return result;
            }

            result.Succeeded = true;
            return result;
        }

        public async Task<CaseResult> CanUserViewCaseDetails(string caseId, string userId)
        {
            var result = new CaseResult();
            var linkExists = await _dbcontext.CaseUsers.Where(x => x.CaseId == caseId && x.UserId == userId).AnyAsync();
            if (!linkExists)
            {
                result.AddError(BusinessRuleCodes.CasePermissions);
                return result;
            }

            result.Succeeded = true;
            return result;
        }

        public async Task<CaseResult> CanUserViewCasePermissions(string caseId, string userId)
        {
            var result = new CaseResult();
            var hasPerms = await _dbcontext.CasePermissions.Where(x => x.CaseId == caseId && x.UserId == userId && x.CanViewPermissions == true).AnyAsync();
            if (!hasPerms)
            {
                result.AddError(BusinessRuleCodes.CasePermissions);
                return result;
            }
            result.Succeeded = true;
            return result;
        }

        public async Task<CaseResult> CreateAsync(Case caseToCreate)
        {
            var result = new CaseResult();

            var isCaseNumberTaken = await IsCaseNumberTaken(caseToCreate.CaseNumber);
            if (isCaseNumberTaken)
            {
                result.AddError(BusinessRuleCodes.CaseNumberTaken, $"Case number: {caseToCreate.CaseNumber} is taken.");
                return result;
            }

            var reportingOfficerExists = await _userValidationService.DoesUserExistAsync(caseToCreate.ReportingOfficerId);
            var createdByUserExists = await _userValidationService.DoesUserExistAsync(caseToCreate.CreatedById);

            if (!createdByUserExists || !reportingOfficerExists)
            {
                result.AddError(BusinessRuleCodes.ValidationError, "User dose not exist");
                return result;
            }

            var reportingOfficerDetails = await _userValidationService.GetUserById(caseToCreate.ReportingOfficerId);
            var createdByDetails = await _userValidationService.GetUserById(caseToCreate.CreatedById);

            caseToCreate.ReportingOfficerEmail = reportingOfficerDetails.Email;
            caseToCreate.ReportingOfficerUserName = reportingOfficerDetails.Username;

            caseToCreate.CreatedByUserName = createdByDetails.Username;
            caseToCreate.CreatedByEmail = createdByDetails.Email;

            await _dbcontext.Cases.AddAsync(caseToCreate);

            if (caseToCreate.ReportingOfficerId != caseToCreate.CreatedById)
            {
                var permissionsForCreator = new CasePermission
                {
                    CanAssign = true,
                    CanEdit = true,
                    CanAddActions = true,
                    CanDeleteActions = true,
                    CanDeleteFileAttachments = true,
                    CaseId = caseToCreate.Id,
                    UserId = createdByDetails.UserId,
                    UserName = createdByDetails.Username,
                    CanEditActions = true,
                    CanEditPermissions = true,
                    CanRemoveAssigned = true,
                    CanViewActions = true,
                    CanViewAssigned = true,
                    CanViewFileAttachments = true,
                    CanViewPermissions = true,
                    CanEditIncidentType = true,
                };
                var permissionsForReportingOfficer = new CasePermission
                {
                    CanAssign = true,
                    CanEdit = true,
                    CaseId = caseToCreate.Id,
                    UserId = reportingOfficerDetails.UserId,
                    UserName = reportingOfficerDetails.Username,
                    CanAddActions = true,
                    CanDeleteActions = true,
                    CanDeleteFileAttachments = true,
                    CanEditActions = true,
                    CanEditPermissions = true,
                    CanRemoveAssigned = true,
                    CanViewActions = true,
                    CanViewAssigned = true,
                    CanViewFileAttachments = true,
                    CanViewPermissions = true,
                    CanEditIncidentType = true,
                };

                var assignReportingOfficerToCaseLink = new CaseUser { CaseId = caseToCreate.Id, UserEmail = reportingOfficerDetails.Email, UserId = reportingOfficerDetails.UserId, UserName = reportingOfficerDetails.Username };
                var assignCreatorToCaseLink = new CaseUser { CaseId = caseToCreate.Id, UserEmail = createdByDetails.Email, UserId = createdByDetails.UserId, UserName = createdByDetails.Username };

                await _dbcontext.CasePermissions.AddRangeAsync([permissionsForCreator, permissionsForReportingOfficer]);
                await _dbcontext.CaseUsers.AddRangeAsync([assignReportingOfficerToCaseLink, assignCreatorToCaseLink]);
            }
            else
            {
                var permission = new CasePermission
                {
                    CanAssign = true,
                    CanEdit = true,
                    CaseId = caseToCreate.Id,
                    UserId = createdByDetails.UserId,
                    UserName = createdByDetails.Username,
                    CanAddActions = true,
                    CanDeleteActions = true,
                    CanDeleteFileAttachments = true,
                    CanEditActions = true,
                    CanEditPermissions = true,
                    CanRemoveAssigned = true,
                    CanViewActions = true,
                    CanViewAssigned = true,
                    CanViewFileAttachments = true,
                    CanViewPermissions = true,
                    CanEditIncidentType = true,
                };
                var assignUserToCaseLink = new CaseUser { CaseId = caseToCreate.Id, UserEmail = createdByDetails.Email, UserId = createdByDetails.UserId, UserName = createdByDetails.Username };

                await _dbcontext.CasePermissions.AddAsync(permission);
                await _dbcontext.CaseUsers.AddAsync(assignUserToCaseLink);
            }

            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<CaseResult> CreateIncidentType(IncidentType incidentType)
        {
            var result = new CaseResult();

            var incidentTypeAlreadyExists = await _dbcontext.IncidentTypes.AnyAsync(x => x.Name == incidentType.Name);
            if (incidentTypeAlreadyExists)
            {
                result.AddError(BusinessRuleCodes.IncidentTypeAlreadyExists, "Trying to create a Incident Type that already exists.");
                return result;
            }

            await _dbcontext.IncidentTypes.AddAsync(incidentType);
            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }


        public async Task<CaseResult> DeleteAttachment(CaseAttachmentFile file)
        {
            var result = new CaseResult();

            if (file.IsDeleted)
            {
                result.AddError(BusinessRuleCodes.ValidationError, "File already deleted");
                return result;
            }

            file.Delete();
            _dbcontext.CaseAttachmentFiles.Update(file);
            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<CaseResult> DeleteIncidentType(IncidentType incidentType)
        {
            var result = new CaseResult();

            var joins = await _dbcontext.CaseIncidentTypes.Where(x => x.IncidentTypeId == incidentType.Id).ToListAsync();
            _dbcontext.RemoveRange(joins);
            _dbcontext.Remove(incidentType);

            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public Task<string> DownloadCaseAttachment(CaseAttachmentFile caseAttachmentFile)
        {
            throw new NotImplementedException();
        }

        public async Task<Case?> FindById(string caseId)
        {
            return await _dbcontext.Cases.FindAsync(caseId);
        }

        public async Task<CaseAttachmentFile?> FindCaseAttachmentById(string caseAttachmentId)
        {
            return await _dbcontext.CaseAttachmentFiles.FindAsync(caseAttachmentId);
        }

        public async Task<CasePermission?> FindCasePermissionById(Case @case, string permissionId)
        {
            return await _dbcontext.CasePermissions.Where(x => x.CaseId == @case.Id && x.Id == permissionId).FirstOrDefaultAsync();
        }

        public async Task<IncidentType?> FindIncidentTypeById(string incidentTypeId)
        {
            return await _dbcontext.IncidentTypes.FindAsync(incidentTypeId);
        }

        public async Task<List<IncidentType>> GetAllIncidentTypes()
        {
            return await _dbcontext.IncidentTypes.ToListAsync();
        }

        public async Task<List<CaseAction>> GetCaseActions(Case @case)
        {
            return await _dbcontext.CaseActions.Where(x => x.CaseId == @case.Id).ToListAsync();
        }

        public async Task<List<CaseAttachmentFile>> GetCaseAttachments(Case @case)
        {
            return await _dbcontext.CaseAttachmentFiles.Where(x => x.CaseId == @case.Id).ToListAsync();
        }

        public async Task<int> GetCaseIncidentCount(IncidentType incidentType)
        {
            return await _dbcontext.CaseIncidentTypes.Where(x => x.IncidentTypeId == incidentType.Id).CountAsync();
        }

        public async Task<MyCasePermissionResult> GetUserCasePermissions(Case @case, string userId)
        {
            var result = new MyCasePermissionResult();

            var perm = await _dbcontext.CasePermissions
                .Where(x => x.CaseId == @case.Id && x.UserId == userId)
                .FirstOrDefaultAsync();

            if (perm is null)
            {
                result.AddError(BusinessRuleCodes.ValidationError, "Permissions not found");
                return result;
            }

            var perms = typeof(CasePermission)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(bool))
                .Where(p => (bool)p.GetValue(perm)!)
                .Select(p => p.Name)
                .ToList();

            result.Permissions = perms;
            result.Succeeded = true;
            return result;
        }


        public async Task<List<CasePermission>> GetCasePermissions(Case @case)
        {
            return await _dbcontext.CasePermissions.Where(x => x.CaseId == @case.Id).ToListAsync();
        }

        public async Task<List<CaseUser>> GetCaseUsers(Case @case)
        {
            return await _dbcontext.CaseUsers.Where(x => x.CaseId == @case.Id).ToListAsync();
        }

        public async Task<List<IncidentType>> GetIncidentTypes(Case @case)
        {
            return await _dbcontext.CaseIncidentTypes.Where(x => x.CaseId == @case.Id).Select(x => x.IncidentType).ToListAsync();
        }

        public async Task<bool> IsCaseNumberTaken(string caseNumber)
        {
            if (string.IsNullOrWhiteSpace(caseNumber))
            {
                return true;
            }

            return await _dbcontext.Cases.AnyAsync(x => x.CaseNumber == caseNumber);
        }

        public async Task<CaseResult> RemoveUser(Case @case, string userId)
        {
            var result = new CaseResult();

            var join = await _dbcontext.CaseUsers.Where(x => x.UserId == userId && x.CaseId == @case.Id).FirstOrDefaultAsync();
            if (join is null)
            {
                result.AddError(BusinessRuleCodes.ValidationError, "User is not linked to the given case already");
                return result;
            }

            var permissionForThisCase = await _dbcontext.CasePermissions.Where(x => x.CaseId == @case.Id && x.UserId == userId).FirstOrDefaultAsync();
            if (permissionForThisCase is not null)
            {
                _dbcontext.CasePermissions.Remove(permissionForThisCase);
            }

            _dbcontext.CaseUsers.Remove(join);
            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<PagedResult<Case>> SearchCases(SearchCasesQuery query)
        {
            IQueryable<Case> queryBuilder = _dbcontext.Cases.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.IncidentTypeId))
            {
                queryBuilder = queryBuilder.Where(c => c.CaseIncidentType.Any(cit => cit.IncidentTypeId == query.IncidentTypeId));
            }

            if (!string.IsNullOrWhiteSpace(query.CaseNumber))
            {
                queryBuilder = queryBuilder.Where(x => x.CaseNumber.Contains(query.CaseNumber));
            }

            if (query.IncidentDateTime.HasValue)
            {
                DateTime startDate = query.IncidentDateTime.Value.Date;
                DateTime endDate = startDate.AddDays(1);

                queryBuilder = queryBuilder.Where(x => x.IncidentDateTime >= startDate && x.IncidentDateTime < endDate);
            }

            if (query.ReportedDateTime.HasValue)
            {
                DateTime startDate = query.ReportedDateTime.Value.Date;
                DateTime endDate = startDate.AddDays(1);
                queryBuilder = queryBuilder.Where(x => x.ReportedDateTime >= startDate && x.ReportedDateTime < endDate);
            }

            if (query.Status.HasValue)
            {
                queryBuilder = queryBuilder.Where(x => x.Status == query.Status);
            }

            if (query.Priority.HasValue)
            {
                queryBuilder = queryBuilder.Where(x => x.Priority == query.Priority);
            }

            if (!string.IsNullOrWhiteSpace(query.ReportingOfficerId))
            {
                queryBuilder = queryBuilder.Where(x => x.ReportingOfficerId == query.ReportingOfficerId);
            }

            if (!string.IsNullOrWhiteSpace(query.CreatedById))
            {
                queryBuilder = queryBuilder.Where(x => x.CreatedById == query.CreatedById);
            }

            if (query.AssignedUserIds.Length != 0)
            {
                queryBuilder = queryBuilder.Where(c => c.CaseUsers.Any(cu => query.AssignedUserIds.Contains(cu.UserId)));
            }

            var count = await queryBuilder.CountAsync();

            queryBuilder = queryBuilder.OrderBy(x => x.Id);

            var items = await queryBuilder
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var result = new PagedResult<Case>(items, count, query.PageNumber, query.PageSize);
            return result;
        }

        public async Task<CaseResult> UpdateCaseLinkedIncidentTypes(Case @case, List<IncidentType> incidentTypes)
        {
            var result = new CaseResult();

            var currentlyLinkedIncidentTypes = await _dbcontext.CaseIncidentTypes.Where(x => x.CaseId == @case.Id).ToListAsync();
            _dbcontext.CaseIncidentTypes.RemoveRange(currentlyLinkedIncidentTypes);

            List<CaseIncidentType> newlyLinkedIncidentTypes = [];
            foreach (var incidentType in incidentTypes)
            {
                newlyLinkedIncidentTypes.Add(new CaseIncidentType { CaseId = @case.Id, IncidentTypeId = incidentType.Id });
            }
            await _dbcontext.CaseIncidentTypes.AddRangeAsync(newlyLinkedIncidentTypes);
            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<CaseResult> UpdateCasePermission(CasePermission permission)
        {
            var result = new CaseResult();

            _dbcontext.CasePermissions.Update(permission);
            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<CaseResult> UpdateIncidentType(IncidentType incidentType)
        {
            var result = new CaseResult();

            var isIncidentTypeNameTaken = await _dbcontext.IncidentTypes.AnyAsync(x => x.Name == incidentType.Name && x.Id != incidentType.Id);
            if (isIncidentTypeNameTaken)
            {
                result.AddError(BusinessRuleCodes.IncidentTypeAlreadyExists, $@"Incident type {incidentType.Name} is already taken, try another.");
                return result;
            }

            _dbcontext.IncidentTypes.Update(incidentType);
            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<CaseResult> CanUserEditLinkedIncidentTypes(string caseId, string userId)
        {
            var result = new CaseResult();
            var hasPermission = await _dbcontext.CasePermissions.Where(x => x.CaseId == caseId && x.UserId == userId && x.CanEditIncidentType == true).AnyAsync();
            if (!hasPermission)
            {
                result.AddError(BusinessRuleCodes.CasePermissions);
                return result;
            }

            result.Succeeded = true;
            return result;
        }
    }
}
