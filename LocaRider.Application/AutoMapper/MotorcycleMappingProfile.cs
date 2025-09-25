using AutoMapper;
using LocaRider.Application.DTO.Motorcycle;
using LocaRider.Domain.Entities.Motorcycles;

namespace LocaRider.Application.AutoMapper
{
    public class MotorcycleMappingProfile : Profile
    {
        public MotorcycleMappingProfile()
        {
            CreateMap<MotorcycleDTO, Motorcycle>()
                .ForMember(dest => dest.MotorcycleId, opt => opt.MapFrom(src => src.identificador))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.ano))
                .ForMember(dest => dest.Plate, opt => opt.MapFrom(src => src.placa.ToUpperInvariant()))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.modelo));

            CreateMap<Motorcycle, MotorcycleDTO>()
                .ForMember(dest => dest.identificador, opt => opt.MapFrom(src => src.MotorcycleId))
                .ForMember(dest => dest.ano, opt => opt.MapFrom(src => src.Year))
                .ForMember(dest => dest.modelo, opt => opt.MapFrom(src => src.Model))
                .ForMember(dest => dest.placa, opt => opt.MapFrom(src => src.Plate));
        }
    }
}
