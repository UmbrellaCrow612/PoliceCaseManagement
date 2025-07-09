using Evidence.API.DTOs;
using Mapper;

namespace Evidence.API.Mappings
{
    public class EvidenceMapping : IMapper<Core.Models.Evidence, EvidenceDto, UpdateEvidenceDto, CreateEvidenceDto>
    {
        public Core.Models.Evidence Create(CreateEvidenceDto createDto)
        {
            return new Core.Models.Evidence
            {
                BucketName = "EMPTY", // EMPTY will be set later as we cannot do it here now
                CollectionDate = createDto.CollectionDate,
                ContentType = createDto.ContentType,
                Description = createDto.Description,
                FileName = createDto.FileName,
                FileSize = createDto.FileSize,
                ReferenceNumber = createDto.ReferenceNumber,
                S3Key = "EMPTY",
                UploadedByEmail = "EMPTY",
                UploadedById = "EMPTY",
                UploadedByUsername = "EMPTY",
            };
        }

        public EvidenceDto ToDto(Core.Models.Evidence @base)
        {
            throw new NotImplementedException();
        }

        public void Update(Core.Models.Evidence @base, UpdateEvidenceDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
