using Cases.Application.Codes;
using Cases.Core.Models;
using Cases.Core.Services;
using Cases.Core.ValueObjects;
using Cases.Infrastructure.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pagination.Abstractions;
using Results.Abstractions;

namespace Cases.Application.Implementations
{
    internal class CaseService(CasesApplicationDbContext dbContext, IPublishEndpoint publishEndpoint, ILogger<CaseService> logger, UserValidationService userValidationService) : ICaseService
    {
        private readonly CasesApplicationDbContext _dbcontext = dbContext;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly ILogger<CaseService> _logger = logger;
        private readonly UserValidationService _userValidationService = userValidationService;

        public async Task<IResult> AddUser(Case @case, string userId)
        {
            var result = new CaseResult();

            var isUserAlreadyAssignedToCase = await _dbcontext.CaseAccessLists.AnyAsync(x => x.CaseId == @case.Id && x.UserId == userId);
            if (isUserAlreadyAssignedToCase)
            {
                result.AddError(BusinessRuleCodes.UserAlreadyAssignedToCase, "User already assigned to case");
                return result;
            }

            var userExists = await _userValidationService.DoesUserExistAsync(userId);
            if (!userExists)
            {
                result.AddError(BusinessRuleCodes.UserNotFound, "User not found");
                return result;
            }

            var userDetails = await _userValidationService.GetUserById(userId);

            var link = new CaseAccessList
            {
                CaseId = @case.Id,
                CaseRole = CaseRole.Viewer,
                UserEmail = userDetails.Email,
                UserId = userId,
                UserName = userDetails.Username
            };
            await _dbcontext.CaseAccessLists.AddAsync(link);

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
                var creatorPermission = new CaseAccessList
                {
                    CaseId = caseToCreate.Id,
                    CaseRole = CaseRole.Owner,
                    UserEmail = createdByDetails.Email,
                    UserId = createdByDetails.UserId,
                    UserName = createdByDetails.Username
                };
                var reportingOfficerPermission = new CaseAccessList
                {
                    CaseId = caseToCreate.Id,
                    CaseRole = CaseRole.Editor,
                    UserEmail = reportingOfficerDetails.Email,
                    UserId = reportingOfficerDetails.UserId,
                    UserName = reportingOfficerDetails.Username
                };
                await _dbcontext.CaseAccessLists.AddRangeAsync([creatorPermission, reportingOfficerPermission]);
            } else
            {
                var creatorPermission = new CaseAccessList
                {
                    CaseId = caseToCreate.Id,
                    CaseRole = CaseRole.Owner,
                    UserEmail = createdByDetails.Email,
                    UserId = createdByDetails.UserId,
                    UserName = createdByDetails.Username
                };
                await _dbcontext.CaseAccessLists.AddAsync(creatorPermission);
            }

            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<Case?> FindById(string caseId)
        {
            return await _dbcontext.Cases.FindAsync(caseId);
        }

        public async Task<List<CaseAccessList>> GetUsersAsync(Case @case)
        {
            return await _dbcontext.CaseAccessLists.Where(x => x.CaseId == @case.Id).ToListAsync();
        }

        public async Task<bool> IsCaseNumberTaken(string caseNumber)
        {
            if (string.IsNullOrWhiteSpace(caseNumber))
            {
                return true;
            }

            return await _dbcontext.Cases.AnyAsync(x => x.CaseNumber == caseNumber);
        }

        public async Task<bool> IsUserLinkedToCase(Case @case, string userId)
        {
            return await _dbcontext.CaseAccessLists.AnyAsync(x => x.UserId == userId && x.CaseId == @case.Id);
        }

        public async Task<CaseResult> RemoveUser(Case @case, string userId)
        {
            var result = new CaseResult();

            var caseAccessList = await _dbcontext.CaseAccessLists.Where(x => x.UserId == userId && x.CaseId == @case.Id).FirstOrDefaultAsync();
            if (caseAccessList is null)
            {
                result.AddError(BusinessRuleCodes.ValidationError, "User is not linked to the given case already");
                return result;
            }

            _dbcontext.CaseAccessLists.Remove(caseAccessList);
            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<PaginatedResult<Case>> SearchCases(SearchCasesQuery query)
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

            if (query.AssignedUserIds != null && query.AssignedUserIds.Length > 0)
            {
                queryBuilder = queryBuilder.Where(c => c.CaseAccessLists.Any(cu => query.AssignedUserIds.Contains(cu.UserId)));
            }

            int count = await queryBuilder.CountAsync();
            int totalPages = (int)Math.Ceiling(count / (double)query.PageSize);

            queryBuilder = queryBuilder.OrderBy(x => x.Id);

            var items = await queryBuilder
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PaginatedResult<Case>
            {
                HasNextPage = query.PageNumber < totalPages,
                HasPreviousPage = query.PageNumber > 1,
                Pagination = new PaginationMetadata
                {
                    CurrentPage = query.PageNumber,
                    PageSize = query.PageSize,
                    TotalPages = totalPages,
                    TotalRecords = count
                },
                Data = items
            };
        }

    }
}
