using AutoMapper;
using Identity.API.DTOs;
using Identity.Core.Models;

namespace Identity.API.Mappings
{
    public class DepartmentMappingProfile : Profile
    {
        public DepartmentMappingProfile()
        {
            CreateMap<CreateDepartmentDto, Department>();
            CreateMap<Department, DepartmentDto>();
            CreateMap<UpdateDepartmentDto, Department>();
        }
    }
}
