using Cases.API.DTOs;
using Cases.Core.Models;
using Mapper;

namespace Cases.API.Mappings
{
    public class IncidentTypeMapping : IMapper<IncidentType, IncidentTypeDto, UpdateIncidentTypeDto, CreateIncidentTypeDto>
    {
        public IncidentType Create(CreateIncidentTypeDto createDto)
        {
            return new IncidentType { Name = createDto.Name, Description = createDto.Description };
        }

        public IncidentTypeDto ToDto(IncidentType @base)
        {
            return new IncidentTypeDto { Id = @base.Id, Name = @base.Name, Description = @base.Description };
        }

        public void Update(IncidentType @base, UpdateIncidentTypeDto updateDto)
        {
            @base.Name = updateDto.Name;
            @base.Description = updateDto.Description;
        }
    }
}
