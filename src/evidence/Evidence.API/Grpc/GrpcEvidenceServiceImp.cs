using Evidence.Core.Services;
using Grpc.Core;
using Evidence.V1;

namespace Evidence.API.Grpc
{
    /// <summary>
    /// GRPC implementation of the grpc service 
    /// </summary>
    public class GrpcEvidenceServiceImp(IEvidenceService evidenceService, ILogger<GrpcEvidenceServiceImp> logger) : EvidenceService.EvidenceServiceBase
    {
        private readonly IEvidenceService _evidenceService = evidenceService;
        private readonly ILogger<GrpcEvidenceServiceImp> _logger = logger;

        public override async Task<DoesEvidenceExistResponse> DoseEvidenceExist(DoesEvidenceExistRequest request, ServerCallContext context)
        {
            _logger.LogInformation("GRPC fired off for evidence {id}", request.EvidenceId);

            var exists = await _evidenceService.ExistsAsync(request.EvidenceId);

            var response = new DoesEvidenceExistResponse
            {
                Exists = exists,
            };
            return response;
        }


        public override async Task<GetEvidenceByIdResponse> GetEvidenceById(GetEvidenceByIdRequest request, ServerCallContext context)
        {
            _logger.LogInformation("GRPC fired off for evidence {id}", request.EvidenceId);

            var evidence = await _evidenceService.FindByIdAsync(request.EvidenceId) ?? throw new RpcException(new Status(StatusCode.NotFound, $"Evidence with ID '{request.EvidenceId}' not found."));

            var response = new GetEvidenceByIdResponse
            {
                EvidenceId = evidence.Id,
                FileName = evidence.FileName,
                Name = evidence.Name,
                ReferenceNumber = evidence.ReferenceNumber,
            };
            return response;
        }
    }
}
