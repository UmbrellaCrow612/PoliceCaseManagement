using Results.Abstractions;

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
        Task<EvidenceServiceResult> DeleteAsync(Models.Evidence evidence);

        /// <summary>
        /// Check if a <see cref="Core.Models.Evidence.ReferenceNumber"/> is taken
        /// </summary>
        /// <param name="referenceNumber">The number to check</param>
        /// <returns>Bool if it is or is not</returns>
        Task<bool> IsReferenceNumberTaken(string referenceNumber);
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
