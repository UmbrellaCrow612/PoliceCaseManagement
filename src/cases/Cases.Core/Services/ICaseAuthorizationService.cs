using Cases.Core.Models;

namespace Cases.Core.Services
{
    /// <summary>
    /// Handles the business logic of authorization related actions for <see cref="Core.Models.Case"/>
    /// </summary>
    public interface ICaseAuthorizationService
    {
        /// <summary>
        /// Checks if the given user can view details on a given case
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="caseId"></param>
        /// <returns></returns>
        Task<bool> CanUserViewCase(string userId, string caseId);


        Task<bool> CanUserEditIncidentTypes(string userId, string caseId);

        Task<bool> CanUserAddActions(string userId, string caseId);

        Task<bool> CanUserViewCaseActions(string userId, string caseId);

        Task<bool> CanUserAssignCaseUsers(string userId, string caseId);

        /// <summary>
        /// Gets a user's <see cref="CaseRole"/> for a specific <see cref="Case"/> by their ID.
        /// </summary>
        /// <param name="case">
        /// The <see cref="Case"/> for which to retrieve the user's role.
        /// </param>
        /// <param name="userId">
        /// The unique identifier of the user whose role is to be retrieved.
        /// </param>
        /// <returns>
        /// The <see cref="CaseRole"/> assigned to the user in the specified case.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the user is not linked to the case.
        /// </exception>
        Task<CaseRole> GetUserRole(Case @case, string userId);

    }
}
