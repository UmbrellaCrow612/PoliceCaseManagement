using Identity.API.DTOs;
using Identity.Core.Models;

namespace Identity.API.Mappings
{
    public class DepartmentMapping : IMapper<Department, DepartmentDto, UpdateDepartmentDto>
    {
        public DepartmentDto ToDto(Department @base)
        {
            return new DepartmentDto { Id = @base.Id, Name = @base.Name };
        }

        public void Update(Department @base, UpdateDepartmentDto updateDto)
        {
            @base.Name = updateDto.Name;
        }
    }
}
