using Identity.API.DTOs;
using Identity.Core.Models;

namespace Identity.API.Mappings
{
    public class DepartmentMapping : IMapper<Department, DepartmentDto, UpdateDepartmentDto, CreateDepartmentDto>
    {
        public Department Create(CreateDepartmentDto createDto)
        {
            return new Department { Name = createDto.Name };
        }

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
