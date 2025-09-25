using AutoMapper;
using LocaRider.Application.DTO.Driver;
using LocaRider.Domain.Entities.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocaRider.Application.AutoMapper
{
    public class DriverMappingProfile : Profile
    {
        public DriverMappingProfile()
        {
            CreateMap<DriverDTO, Driver>()
            .ForCtorParam("name", opt => opt.MapFrom(src => src.nome))
            .ForCtorParam("cnpj", opt => opt.MapFrom(src => src.cnpj))
            .ForCtorParam("birthDate", opt => opt.MapFrom(src => src.data_nascimento.ToUniversalTime()))
            .ForCtorParam("cnhNumber", opt => opt.MapFrom(src => src.numero_cnh))
            .ForCtorParam("cnhType", opt => opt.MapFrom(src => src.tipo_cnh))
            .ForCtorParam("cnhImage", opt => opt.MapFrom(src => src.imagem_cnh))
            .AfterMap((src, dest) =>
            {
                // Mapeia o DriverId/identificador
                if (!string.IsNullOrWhiteSpace(src.identificador))
                    dest.DriverId = src.identificador;
            })
            .ReverseMap()
            .ForMember(dest => dest.identificador, opt => opt.MapFrom(src => src.DriverId));
        }
    }
}
