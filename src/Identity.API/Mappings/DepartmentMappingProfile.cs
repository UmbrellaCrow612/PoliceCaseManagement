using AutoMapper;
using Identity.API.DTOs;
using Identity.Infrastructure.Data.Models;

namespace Identity.API.Mappings
{
    public class DepartmentMappingProfile : Profile
    {
        public DepartmentMappingProfile()
        {
            CreateMap<CreateDepartmentDto, Department>();
            CreateMap<Department, DepartmentDto>();
        }
    }
}
