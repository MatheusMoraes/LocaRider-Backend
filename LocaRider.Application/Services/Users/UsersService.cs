using AutoMapper;
using LocaRider.Application.DTO.Users;
using LocaRider.Application.Interfaces.Users;
using LocaRider.Domain.Entities.Users;
using LocaRider.Domain.Interfaces.Users;
using System.Security.Cryptography;
using System.Text;

namespace LocaRider.Application.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _userRepository; // Pode ser EF Core ou Mongo
        private readonly IMapper _mapper;

        public UsersService(IUsersRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<UserDTO?> GetUserByIdAsync(Guid id)
        {   
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? null : _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO?> CreateUserAsync(UserCreateDTO userCreateDto)
        {
            var user = _mapper.Map<User>(userCreateDto);

            if (await _userRepository.GetByEmailAsync(userCreateDto.Email) is not null)
            {
                return null;
            }
            await _userRepository.AddAsync(user);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<bool> UpdateUserAsync(Guid id, UserUpdateDTO userUpdateDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null) return false;

            // Mapear DTO para a entidade existente
            _mapper.Map(userUpdateDto, existingUser);
            await _userRepository.UpdateAsync(existingUser);
            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null) return false;

            await _userRepository.DeleteAsync(existingUser);
            return true;
        }
    }
}
