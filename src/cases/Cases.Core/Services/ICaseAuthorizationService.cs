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
    }
}
