using Cases.Core.Models;

namespace Cases.Core.Services
{
    /// <summary>
    /// Contains method to interact purely with case actions - linking is done through <see cref="ICaseService"/> as it's the parent
    /// </summary>
    public interface ICaseActionService
    {
        /// <summary>
        /// Get a case action by it's ID
        /// </summary>
        /// <param name="caseActionId">The case action ID</param>
        /// <returns>The case action or null if it could not be found</returns>
        Task<CaseAction?> GetCaseActionByIdAsync(string caseActionId);
    }
}
