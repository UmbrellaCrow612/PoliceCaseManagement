using Cases.Core.Models.Joins;
using Mapper;

namespace Cases.API.Mappings
{
    public class CaseUserMapping : IMapper<CaseUser, CaseUserDto, EmptyDto, CreateCaseUserDto>
    {
        public CaseUser Create(CreateCaseUserDto createDto)
        {
            throw new NotImplementedException();
        }

        public CaseUserDto ToDto(CaseUser @base)
        {
            return new CaseUserDto
            {
                Id = @base.Id,
                CaseId = @base.CaseId,
                UserEmail = @base.UserEmail,
                UserId = @base.UserId,
                UserName = @base.UserName,
            };
        }

        public void Update(CaseUser @base, EmptyDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
