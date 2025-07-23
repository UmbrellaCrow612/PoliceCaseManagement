using Cases.API.DTOs;
using Cases.Core.Models.Joins;
using Mapper;

namespace Cases.API.Mappings
{
    public class CaseEvidenceMapping : IMapper<CaseEvidence, CaseEvidenceDto, EmptyDto, EmptyDto>
    {
        public CaseEvidence Create(EmptyDto createDto)
        {
            throw new NotImplementedException();
        }

        public CaseEvidenceDto ToDto(CaseEvidence @base)
        {
            return new CaseEvidenceDto
            {
                CaseId = @base.CaseId,
                EvidenceId = @base.EvidenceId,
                EvidenceName = @base.EvidenceName,
                EvidenceReferenceNumber = @base.EvidenceReferenceNumber,
                Id = @base.Id,
            };
        }

        public void Update(CaseEvidence @base, EmptyDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
