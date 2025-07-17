using Cases.Application.Codes;
using Cases.Core.Models;
using Cases.Core.Services;
using Cases.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Results.Abstractions;

namespace Cases.Application.Implementations
{
    internal class CaseActionService(CasesApplicationDbContext dbContext, UserValidationService userValidationService) : ICaseActionService
    {
        private readonly CasesApplicationDbContext _dbcontext = dbContext;
        private readonly UserValidationService _userValidationService = userValidationService;

        public async Task<IResult> CreateAsync(Case @case, CaseAction action)
        {
            var result = new Result();

            if (@case.Id != action.CaseId)
            {
                result.AddError(BusinessRuleCodes.ValidationError);
                return result;
            }

            var userExists = await _userValidationService.DoesUserExistAsync(action.CreatedById);
            if (!userExists)
            {
                result.AddError(BusinessRuleCodes.UserNotFound, "User not found");
                return result;
            }

            var userDetails = await _userValidationService.GetUserById(action.CreatedById);

            action.CreatedByName = userDetails.Username;
            action.CreatedByEmail = userDetails.Email;

            await _dbcontext.CaseActions.AddAsync(action);
            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<CaseAction?> FindByIdAsync(string caseActionId)
        {
            return await _dbcontext.CaseActions.FindAsync(caseActionId);
        }

        public async Task<List<CaseAction>> GetAsync(Case @case)
        {
            return await _dbcontext.CaseActions.Where(x => x.CaseId == @case.Id).ToListAsync();
        }


        private class Result : IResult
        {
            public bool Succeeded { get; set; } = false;
            public ICollection<IResultError> Errors { get; set; } = [];

            public void AddError(string code, string? message = null)
            {
                Errors.Add(new Error { Code = code, Message = message });
            }
        }

        private class Error : IResultError
        {
            public required string Code { get; set; }
            public required string? Message { get; set; } = null;
        }
    }
}
