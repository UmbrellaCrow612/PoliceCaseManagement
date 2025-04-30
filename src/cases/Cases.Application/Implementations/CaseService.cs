using Cases.Application.Codes;
using Cases.Core.Models;
using Cases.Core.Models.Joins;
using Cases.Core.Services;
using Cases.Core.ValueObjects;
using Cases.Infrastructure.Data;
using Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Cases.Infrastructure;

namespace Cases.Application.Implementations
{
    internal class CaseService(CasesApplicationDbContext dbContext, IPublishEndpoint publishEndpoint, ILogger<CaseService> logger) : ICaseService
    {
        private readonly CasesApplicationDbContext _dbcontext = dbContext;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly ILogger<CaseService> _logger = logger;

        public async Task<CaseResult> AddCaseAction(Case @case, CaseAction caseAction)
        {
            _logger.LogInformation("Started trying to add a case action: {caseActionId} to a case: {caseId}", caseAction.Id, @case.Id);

            var result = new CaseResult();

            if (caseAction.CaseId != @case.Id)
            {
                result.AddError(BusinessRuleCodes.ValidationError, "Case action not properly linked to the provided case");
                return result;
            }

            await _dbcontext.CaseActions.AddAsync(caseAction);
            await _dbcontext.SaveChangesAsync();

            _logger.LogInformation("Saved and added case action: {caseActionId} to case: {caseId}.", caseAction.Id, @case.Id);

            _logger.LogInformation("Trying to publish a case action created event for case action: {caseActionId}", caseAction.Id);
            await _publishEndpoint.Publish(new CaseActionCreatedEvent { CaseActionId = caseAction.Id, CreatedById = caseAction.CreatedById });
            _logger.LogInformation("Publish a case action created event for case action: {caseActionId}", caseAction.Id);

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

        public async Task<CaseResult> CreateAsync(Case caseToCreate)
        {
            var result = new CaseResult();

            if (!string.IsNullOrWhiteSpace(caseToCreate.CaseNumber))
            {
                var isCaseNumberTaken = await IsCaseNumberTaken(caseToCreate.CaseNumber);
                if (isCaseNumberTaken)
                {
                    result.AddError(BusinessRuleCodes.CaseNumberTaken, $"Case number: {caseToCreate.CaseNumber} is taken.");
                    return result;
                }
            }

            await _dbcontext.Cases.AddAsync(caseToCreate);
            await _dbcontext.SaveChangesAsync();

            await _publishEndpoint.Publish(new CaseCreatedEvent { CaseId = caseToCreate.Id, ReportingOffcierId = caseToCreate.ReportingOfficerId });
            _logger.LogInformation("CaseCreatedEvent published");

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

        public async Task<Case?> FindById(string caseId)
        {
            return await _dbcontext.Cases.FindAsync(caseId);
        }

        public async Task<IncidentType?> FindIncidentTypeById(string incidentTypeId)
        {
            return await _dbcontext.IncidentTypes.FindAsync(incidentTypeId);
        }

        public async Task<List<IncidentType>> GetAllIncidentTypes()
        {
            return await _dbcontext.IncidentTypes.ToListAsync();
        }

        public async Task<int> GetCaseIncidentCount(IncidentType incidentType)
        {
            return await _dbcontext.CaseIncidentTypes.Where(x => x.IncidentTypeId == incidentType.Id).CountAsync();
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

            var count = await queryBuilder.CountAsync();

            queryBuilder = queryBuilder.OrderBy(x => x.Id);

            var items = await queryBuilder
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var result =  new PagedResult<Case>(items, count, query.PageNumber, query.PageSize);
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

            result.Succeeded= true;
            return result;
        }
    }
}
