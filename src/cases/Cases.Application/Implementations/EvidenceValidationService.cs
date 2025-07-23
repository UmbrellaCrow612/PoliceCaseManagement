using Evidence.V1;

namespace Cases.Application.Implementations
{
    /// <summary>
    /// Service used to validate and get evidence related information over GRPC
    /// </summary>
    public class EvidenceValidationService(EvidenceService.EvidenceServiceClient evidenceServiceClient)
    {
        private readonly EvidenceService.EvidenceServiceClient _client = evidenceServiceClient;

        public async Task<bool> DoseEvidenceExistAsync(string evidenceId)
        {
            var response = await _client.DoseEvidenceExistAsync(new DoesEvidenceExistRequest { EvidenceId = evidenceId });

            return response.Exists;
        }

        public async Task<GetEvidenceByIdResponse> GetEvidenceByIdAsync(string evidenceId)
        {
            return await _client.GetEvidenceByIdAsync(new GetEvidenceByIdRequest { EvidenceId = evidenceId });
        }
    }
}
