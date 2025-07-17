using Cases.Core.Models;
using Results.Abstractions;

namespace Cases.Core.Services
{
    /// <summary>
    /// Way to interact with <see cref="CaseAction"/>
    /// </summary>
    public interface ICaseActionService
    {
        Task<CaseAction?> FindByIdAsync(string caseActionId);

        Task<IResult> CreateAsync(Case @case, CaseAction action);

        Task<List<CaseAction>> GetAsync(Case @case);
    }
}
