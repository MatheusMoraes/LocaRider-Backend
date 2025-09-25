using AutoMapper;
using LocaRider.Application.DTO.Users;
using LocaRider.Domain.Entities.Users;

namespace LocaRider.Application.AutoMapper
    {
        public class UserMappingProfile : Profile
        {
            public UserMappingProfile()
            {
                // User -> UserDto (GET)
                CreateMap<User, UserDTO>();

                // UserCreateDto -> User (POST)
                CreateMap<UserCreateDTO, User>()
                 .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                 .AfterMap((src, dest) =>
                 {
                     dest.ChangePassword(src.Password);
                 });

                var map = CreateMap<UserUpdateDTO, User>();

                map.ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

                map.AfterMap((src, dest) =>
                {
                    if (!string.IsNullOrEmpty(src.Password))
                    {
                        dest.ChangePassword(src.Password);
                    }
                });

        }
        }
    }
