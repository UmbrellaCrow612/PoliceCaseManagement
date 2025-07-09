using Evidence.Application.Codes;
using Evidence.Core.Services;
using Evidence.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StorageProvider.Abstractions;
using StorageProvider.AWS;

namespace Evidence.Application.Implementations
{
    public class EvidenceService(EvidenceApplicationDbContext dbContext, IStorageProvider storageProvider, IOptions<AWSSettings> awsSettings, UserValidationService userValidationService) : IEvidenceService
    {
        private readonly EvidenceApplicationDbContext _dbcontext = dbContext;
        private readonly IStorageProvider _storageProvider = storageProvider;
        private readonly AWSSettings _awsSettings = awsSettings.Value;
        private readonly UserValidationService _userService = userValidationService;

        public async Task<EvidenceServiceResult> CreateAsync(Core.Models.Evidence evidence)
        {
            var result = new EvidenceServiceResult();

            var isRefNumberTaken = await _dbcontext.Evidences.AnyAsync(x => x.ReferenceNumber == evidence.ReferenceNumber);
            if (isRefNumberTaken)
            {
                result.AddError(BusinessRuleCodes.EVIDENCE_REFERENCE_NUMBER_TAKEN, "Reference number taken");
                return result;
            }

            evidence.BucketName = _awsSettings.BucketName;

            var key = $"evidence/{evidence.Id}";
            evidence.S3Key = key;

            var uploadURL = await _storageProvider.GetPreSignedUploadUrlAsync(evidence.S3Key, evidence.ContentType);
            result.UploadUrl = uploadURL;

            var userExists = await _userService.DoseUserExist(evidence.UploadedById);
            if (!userExists)
            {
                result.AddError(BusinessRuleCodes.USER_DOES_NOT_EXIST, "User not found");
                return result;
            }

            var userDetails = await _userService.GetUserById(evidence.UploadedById);
            evidence.UploadedByEmail = userDetails.Email;
            evidence.UploadedByUsername = userDetails.Username;

            await _dbcontext.Evidences.AddAsync(evidence);
            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public Task<EvidenceServiceResult> DeleteAsync(Core.Models.Evidence evidence)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsAsync(string evidenceId)
        {
            return await _dbcontext.Evidences.AnyAsync(x => x.Id == evidenceId);
        }

        public async Task<Core.Models.Evidence?> FindByIdAsync(string evidenceId)
        {
            return await _dbcontext.Evidences.FindAsync(evidenceId);
        }

        public async Task<bool> IsReferenceNumberTaken(string referenceNumber)
        {
            return await _dbcontext.Evidences.AnyAsync(x => x.ReferenceNumber == referenceNumber);
        }

        public async Task<EvidenceServiceResult> UpdateAsync(Core.Models.Evidence evidence)
        {
            var result = new EvidenceServiceResult();

            var refNumberNotChanged = await _dbcontext.Evidences.AnyAsync(x => x.ReferenceNumber == evidence.ReferenceNumber);
            if (!refNumberNotChanged)
            {
                result.AddError(BusinessRuleCodes.EVIDENCE_REFERENCE_CHANGED, "You cannot change the ref number");
                return result;
            }

            _dbcontext.Evidences.Update(evidence);
            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }
    }
}
