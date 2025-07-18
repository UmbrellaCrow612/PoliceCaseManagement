using Cases.Core.Models;

namespace Cases.Core.Services
{
    /// <summary>
    /// Handles the business logic for authorization-related actions on a <see cref="Core.Models.Case"/>.
    /// </summary>
    public interface ICaseAuthorizationService
    {
        /// <summary>
        /// Checks if a user is assigned to a case.
        /// </summary>
        /// <param name="userId">The ID of the user to check.</param>
        /// <param name="caseId">The ID of the case to check.</param>
        /// <returns>True if the user is assigned to the case; otherwise, false.</returns>
        Task<bool> IsAssigned(string userId, string caseId);

        /// <summary>
        /// Checks if a user is an <see cref="CaseRole.Editor"/> on a case.
        /// </summary>
        /// <param name="userId">The ID of the user to check.</param>
        /// <param name="caseId">The ID of the case to check.</param>
        /// <returns>True if the user is an editor or owner of the case; otherwise, false.</returns>
        Task<bool> IsEditor(string userId, string caseId);

        /// <summary>
        /// Checks if a user is the <see cref="CaseRole.Owner"/> of a given case.
        /// </summary>
        /// <param name="userId">The ID of the user to check.</param>
        /// <param name="caseId">The ID of the case to check.</param>
        /// <returns>True if the user is the owner of the case; otherwise, false.</returns>
        Task<bool> IsOwner(string userId, string caseId);

        /// <summary>
        /// Gets a user's <see cref="CaseRole"/> for a specific <see cref="Case"/> by their ID.
        /// </summary>
        /// <param name="case">The <see cref="Case"/> for which to retrieve the user's role.</param>
        /// <param name="userId">The ID of the user whose role is to be retrieved.</param>
        /// <returns>The <see cref="CaseRole"/> assigned to the user for the specified case.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the user is not linked to the case.</exception>
        Task<CaseRole> GetUserRole(Case @case, string userId);
    }
}
