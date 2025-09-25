using AutoMapper;
using LocaRider.Application.DTO.Rental;
using LocaRider.Domain.Entities.Rental;

namespace LocaRider.Application.AutoMapper
{
    public class RentalMappingProfile: Profile
    {
        public RentalMappingProfile()
        {
            CreateMap<RentalDTO, Rental>()
           .ForMember(dest => dest.RentalId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
           .ForMember(dest => dest.DriverId, opt => opt.MapFrom(src => src.entregador_id))
           .ForMember(dest => dest.MotorcycleId, opt => opt.MapFrom(src => src.moto_id))
           .ForMember(dest => dest.DailyPrice, opt => opt.MapFrom(src => src.valor_diaria ?? 0))
           .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.data_inicio))
           .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.data_termino))
           .ForMember(dest => dest.EstimatedCompletionDate, opt => opt.MapFrom(src => src.data_previsao_termino))
           .ForMember(dest => dest.Plan, opt => opt.MapFrom(src => src.plano))
           .ForMember(dest => dest.DevolutionDate, opt => opt.Ignore())
           .ForMember(dest => dest.TotalPrice, opt => opt.Ignore());

            // Entidade para DTO
            CreateMap<Rental, RentalDTO>()
                .ForMember(dest => dest.entregador_id, opt => opt.MapFrom(src => src.DriverId))
                .ForMember(dest => dest.moto_id, opt => opt.MapFrom(src => src.MotorcycleId))
                .ForMember(dest => dest.valor_diaria, opt => opt.MapFrom(src => src.DailyPrice))
                .ForMember(dest => dest.data_inicio, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.data_termino, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.data_previsao_termino, opt => opt.MapFrom(src => src.EstimatedCompletionDate))
                .ForMember(dest => dest.plano, opt => opt.MapFrom(src => src.Plan));
        }
    }
}
