using Cases.API.DTOs;
using Cases.Core.Models;
using Mapper;

namespace Cases.API.Mappings
{
    public class CaseAttachmentFileMapping : IMapper<CaseAttachmentFile, CaseAttachmentFileDto, EmptyDto, EmptyDto>
    {
        public CaseAttachmentFile Create(EmptyDto createDto)
        {
            throw new NotImplementedException();
        }

        public CaseAttachmentFileDto ToDto(CaseAttachmentFile @base)
        {
            return new CaseAttachmentFileDto
            {
                Id = @base.Id,
                ContentType = @base.ContentType,
                FileName = @base.FileName,
                FileSize = @base.FileSize,
                UploadedAt = @base.UploadedAt,
            };
        }

        public void Update(CaseAttachmentFile @base, EmptyDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
