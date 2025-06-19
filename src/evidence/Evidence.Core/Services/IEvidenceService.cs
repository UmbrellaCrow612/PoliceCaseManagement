namespace Evidence.Core.Services
{
    public interface IEvidenceService
    {
        /// <summary>
        /// Get a <see cref="Models.Evidence"/> by it's ID
        /// </summary>
        /// <param name="evidenceId">The ID of the specific evidence you want to find</param>
        Task<Models.Evidence?> FindByIdAsync(string evidenceId);

        /// <summary>
        /// Quick check up to see if a specific <see cref="Models.Evidence"/> exists by it's ID
        /// </summary>
        /// <param name="evidenceId">The ID of the specific evidence you want to find</param>
        Task<bool> ExistsAsync(string evidenceId);

    }
}
