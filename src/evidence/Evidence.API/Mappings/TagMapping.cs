using Evidence.API.DTOs;
using Evidence.Core.Models;
using Mapper;

namespace Evidence.API.Mappings
{
    public class TagMapping : IMapper<Tag, TagDto, EmptyDto, EmptyDto>
    {
        public Tag Create(EmptyDto createDto)
        {
            throw new NotImplementedException();
        }

        public TagDto ToDto(Tag @base)
        {
            return new TagDto { Name = @base.Name, Description = @base.Description };
        }

        public void Update(Tag @base, EmptyDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
