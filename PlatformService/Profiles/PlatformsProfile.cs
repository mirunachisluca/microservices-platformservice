using AutoMapper;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.Protos;

namespace PlatformService.Profiles
{
    public class PlatformsProfile : Profile
    {
        public PlatformsProfile()
        {
            // source -> d

            CreateMap<Platform, PlatformReadDTO>();
            CreateMap<PlatformCreateDTO, Platform>();
            CreateMap<PlatformReadDTO, PlatformPublishedDTO>();
            CreateMap<Platform, GrpcPlatformModel>()
                .ForMember(d => d.PlatformId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.Publisher, opt => opt.MapFrom(s => s.Publisher));
        }
    }
}
