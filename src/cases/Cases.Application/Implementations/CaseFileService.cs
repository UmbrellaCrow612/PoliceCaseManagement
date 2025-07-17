using Cases.Application.Codes;
using Cases.Core.Models;
using Cases.Core.Services;
using Cases.Core.ValueObjects;
using Cases.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Results.Abstractions;
using StorageProvider.Abstractions;
using StorageProvider.AWS;

namespace Cases.Application.Implementations
{
    public class CaseFileService(IStorageProvider storageProvider, CasesApplicationDbContext dbContext, IOptions<AWSSettings> awsSettings) : ICaseFileService
    {
        private readonly IStorageProvider _storageProvider = storageProvider;
        private readonly CasesApplicationDbContext _dbContext = dbContext;
        private readonly AWSSettings _awsSettings = awsSettings.Value;

        public async Task<(string preSignedUrl, string fileId)> AddAsync(Case @case, CaseAttachmentFileMetaData fileMetaData)
        {
            var fileId = Guid.NewGuid().ToString();
            var s3Key = $"evidence/{fileId}";

            var file = new CaseAttachmentFile
            {
                BucketName = _awsSettings.BucketName,
                CaseId = @case.Id,
                ContentType = fileMetaData.ContentType,
                FileName = fileMetaData.FileName,
                FileSize = fileMetaData.FileSize,
                Id = fileId,
                S3Key = s3Key,
            };
            await _dbContext.CaseAttachmentFiles.AddAsync(file);

            var _preSignedUrl = await _storageProvider.GetPreSignedUploadUrlAsync(s3Key, file.ContentType, 3);
            await _dbContext.SaveChangesAsync();

            return (_preSignedUrl, fileId);
        }

        public async Task<IResult> DeleteAsync(CaseAttachmentFile file)
        {
            var result = new Result();

            if (file.IsDeleted)
            {
                result.AddError(BusinessRuleCodes.ValidationError, "File already deleted");
                return result;
            }

            file.Delete();
            _dbContext.CaseAttachmentFiles.Update(file);
            await _dbContext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<string> DownloadAsync(CaseAttachmentFile file)
        {
            var preSignedUrl = await _storageProvider.GetPreSignedDownloadUrlAsync(file.S3Key, 3);
            return preSignedUrl;
        }

        public async Task<CaseAttachmentFile?> FindByIdAsync(string fileId)
        {
            return await _dbContext.CaseAttachmentFiles.FindAsync(fileId);
        }

        public async Task<List<CaseAttachmentFile>> GetAsync(Case @case)
        {
            return await _dbContext.CaseAttachmentFiles.Where(x => x.CaseId == @case.Id).ToListAsync();
        }

        public async Task<IResult> UploadComplete(CaseAttachmentFile file)
        {
            var result = new Result();

            if (file.IsAlreadyUploadCompleted())
            {
                result.AddError(BusinessRuleCodes.ValidationError, "File already uploaded");
                return result;
            }

            file.MarkUploadComplete();
            _dbContext.CaseAttachmentFiles.Update(file);
            await _dbContext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        private class Result : IResult
        {
            public bool Succeeded { get; set; } = false;
            public ICollection<IResultError> Errors { get; set; } = [];

            public void AddError(string code, string? message = null)
            {
                Errors.Add(new Error { Code = code, Message = message });
            }
        }

        private class Error : IResultError
        {
            public required string Code { get; set; }
            public required string? Message { get; set; }
        }
    }
}
