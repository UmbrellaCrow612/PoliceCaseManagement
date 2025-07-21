using Evidence.Core.ValueObjects;
using Pagination.Abstractions;
using Results.Abstractions;

namespace Evidence.Core.Services
{
    /// <summary>
    /// Contract to interact with <see cref="Core.Models.Evidence"/>
    /// </summary>
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

        /// <summary>
        /// Create a <see cref="Core.Models.Evidence"/> into the system
        /// </summary>
        /// <param name="evidence">The evidence you want to add</param>
        Task<CreateEvidenceResult> CreateAsync(Models.Evidence evidence);

        /// <summary>
        /// Update a <see cref="Models.Evidence"/>
        /// </summary>
        /// <param name="evidence">The evidence to update with updated properties</param>
        Task<EvidenceServiceResult> UpdateAsync(Models.Evidence evidence);

        /// <summary>
        /// Delete a specific <see cref="Models.Evidence"/> from the system
        /// </summary>
        /// <param name="evidence">The evidence to delete</param>
        /// <param name="userId">The ID of the user who is performing the action</param>
        Task<EvidenceServiceResult> DeleteAsync(Models.Evidence evidence, string userId);

        /// <summary>
        /// Check if a <see cref="Core.Models.Evidence.ReferenceNumber"/> is taken
        /// </summary>
        /// <param name="referenceNumber">The number to check</param>
        /// <returns>Bool if it is or is not</returns>
        Task<bool> IsReferenceNumberTaken(string referenceNumber);

        /// <summary>
        /// Search <see cref="Models.Evidence"/> in the system using a query <see cref="SearchEvidenceQuery"/>
        /// </summary>
        /// <param name="query">Contains fields you can search by</param>
        /// <returns>Paginated result of matching items</returns>
        Task<PaginatedResult<Models.Evidence>> SearchAsync(SearchEvidenceQuery query);

        /// <summary>
        /// Download a piece of <see cref="Models.Evidence"/>
        /// </summary>
        /// <param name="evidence">The piece of evidence to download</param>
        /// <returns>Result object and a client side download URL if successful</returns>
        Task<DownloadEvidenceResult> DownloadAsync(Models.Evidence evidence);
    }

    public class DownloadEvidenceResult : IResult
    {
        public bool Succeeded { get; set; } = false;
        public ICollection<IResultError> Errors { get; set; } = [];
        public string DownloadUrl { get; set; } = "EMPTY";

        public void AddError(string code, string? message = null)
        {
            Errors.Add(new EvidenceServiceError { Code = code, Message = message });
        }
    }

    public class CreateEvidenceResult : IResult
    {
        public bool Succeeded { get; set; } = false;
        public ICollection<IResultError> Errors { get; set; } = [];
        public string UploadUrl { get; set; } = "";

        public void AddError(string code, string? message = null)
        {
            Errors.Add(new EvidenceServiceError { Code = code, Message = message });
        }
    }


    public class EvidenceServiceResult : IResult
    {
        public bool Succeeded { get; set; } = false;
        public ICollection<IResultError> Errors { get; set; } = [];

        public void AddError(string code, string? message = null)
        {
            Errors.Add(new EvidenceServiceError { Code = code, Message = message });
        }
    }

    public class EvidenceServiceError : IResultError
    {
        public required string Code { get; set; }
        public required string? Message { get; set; }
    }
}
