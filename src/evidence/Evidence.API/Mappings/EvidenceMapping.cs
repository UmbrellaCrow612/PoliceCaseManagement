using Evidence.API.DTOs;
using Mapper;

namespace Evidence.API.Mappings
{
    public class EvidenceMapping : IMapper<Core.Models.Evidence, EvidenceDto, UpdateEvidenceDto, CreateEvidenceDto>
    {
        public Core.Models.Evidence Create(CreateEvidenceDto createDto)
        {
            throw new NotImplementedException();
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
