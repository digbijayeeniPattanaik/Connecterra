using API.Dtos;
using AutoMapper;
using Infrastructure.Entities;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Cow, CowDto>()
                .ForMember(a => a.Farm, o => o.MapFrom(a => a.Farm.Name)).ReverseMap();
            CreateMap<Sensor, SensorDto>()
                .ForMember(a => a.Farm, o => o.MapFrom(a => a.Farm.Name)).ReverseMap();
        }
    }
}
