using AutoMapper;
using Evidence.API.DTOs.Create;
using Evidence.API.DTOs.Read;
using Evidence.API.DTOs.Update;
using Evidence.Infrastructure.Data.Models;

namespace Evidence.API.Mappings
{
    public class PhotoMappingProfile : Profile
    {
        public PhotoMappingProfile()
        {
            CreateMap<CreatePhotoDto, Photo>();
            CreateMap<Photo, PhotoDto>();
            CreateMap<UpdatePhotoDto, Photo>();
        }
    }
}
