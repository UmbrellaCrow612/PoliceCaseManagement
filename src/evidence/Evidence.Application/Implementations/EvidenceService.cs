using Evidence.Application.Codes;
using Evidence.Core.Services;
using Evidence.Core.ValueObjects;
using Evidence.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pagination.Abstractions;
using StorageProvider.Abstractions;
using StorageProvider.AWS;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Evidence.Application.Implementations
{
    public class EvidenceService(EvidenceApplicationDbContext dbContext, IStorageProvider storageProvider, IOptions<AWSSettings> awsSettings, UserValidationService userValidationService) : IEvidenceService
    {
        private readonly EvidenceApplicationDbContext _dbcontext = dbContext;
        private readonly IStorageProvider _storageProvider = storageProvider;
        private readonly AWSSettings _awsSettings = awsSettings.Value;
        private readonly UserValidationService _userService = userValidationService;

        public async Task<CreateEvidenceResult> CreateAsync(Core.Models.Evidence evidence)
        {
            var result = new CreateEvidenceResult();

            var isRefNumberTaken = await _dbcontext.Evidences.AnyAsync(x => x.ReferenceNumber == evidence.ReferenceNumber);
            if (isRefNumberTaken)
            {
                result.AddError(BusinessRuleCodes.EVIDENCE_REFERENCE_NUMBER_TAKEN, "Reference number taken");
                return result;
            }

            var userExists = await _userService.DoseUserExist(evidence.UploadedById);
            if (!userExists)
            {
                result.AddError(BusinessRuleCodes.USER_DOES_NOT_EXIST, "User not found");
                return result;
            }

            evidence.BucketName = _awsSettings.BucketName;

            var key = $"evidence/{evidence.Id}";
            evidence.S3Key = key;

            var uploadURL = await _storageProvider.GetPreSignedUploadUrlAsync(evidence.S3Key, evidence.ContentType);
            result.UploadUrl = uploadURL;

            var userDetails = await _userService.GetUserById(evidence.UploadedById);
            evidence.UploadedByEmail = userDetails.Email;
            evidence.UploadedByUsername = userDetails.Username;

            await _dbcontext.Evidences.AddAsync(evidence);
            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<EvidenceServiceResult> DeleteAsync(Core.Models.Evidence evidence, string userId)
        {
            var result = new EvidenceServiceResult();

            if (evidence.IsDeleted)
            {
                result.AddError(BusinessRuleCodes.EVIDENCE_ALREADY_DELETED, "Item already deleted");
                return result;
            }

            evidence.Delete(userId);

            _dbcontext.Evidences.Update(evidence);
            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
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

        public async Task<PaginatedResult<Core.Models.Evidence>> SearchAsync(SearchEvidenceQuery query)
        {
            var queryBuilder = _dbcontext.Evidences.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.ReferenceNumber))
            {
                queryBuilder = queryBuilder.Where(x => x.ReferenceNumber == query.ReferenceNumber);
            }

            if (!string.IsNullOrWhiteSpace(query.FileName))
            {
                queryBuilder = queryBuilder.Where(x => x.FileName.Contains(query.FileName));
            }

            if (!string.IsNullOrWhiteSpace(query.ContentType))
            {
                queryBuilder = queryBuilder.Where(x => x.ContentType == query.ContentType);
            }

            if (query.UploadedAt.HasValue)
            {
                var date = query.UploadedAt.Value.Date;
                var nextDate = date.AddDays(1);

                queryBuilder = queryBuilder.Where(x => x.UploadedAt >= date && x.UploadedAt < nextDate);
            }

            if (query.CollectionDate.HasValue)
            {
                var date = query.CollectionDate.Value.Date;
                var nextDate = date.AddDays(1);

                queryBuilder = queryBuilder.Where(x => x.CollectionDate >= date && x.CollectionDate < nextDate);
            }

            queryBuilder = query.OrderBy switch
            {
                SearchEvidenceOrderByValues.CollectionDate => queryBuilder.OrderBy(x => x.CollectionDate),
                SearchEvidenceOrderByValues.UploadedAt => queryBuilder.OrderBy(x => x.UploadedAt),
                _ => queryBuilder.OrderBy(x => x.Id),
            };

            int pageSize = query.PageSize ?? 10;
            int pageNumber = query.PageNumber > 0 ? query.PageNumber : 1;
            int totalItemsCount = await queryBuilder.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItemsCount / pageSize);

            var items = await queryBuilder
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<Core.Models.Evidence>
            {
                Data = items,
                HasNextPage = pageNumber < totalPages,
                HasPreviousPage = pageNumber > 1,
                Pagination = new PaginationMetadata
                {
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    TotalRecords = totalItemsCount
                }
            };
        }

        public async Task<EvidenceServiceResult> UpdateAsync(Core.Models.Evidence evidence)
        {
            var result = new EvidenceServiceResult();

            _dbcontext.Evidences.Update(evidence);
            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }
    }
}
