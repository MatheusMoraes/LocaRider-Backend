using LocaRider.Application.DTO.Users;

namespace LocaRider.Application.Interfaces.Users
{
    public interface IUsersService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO?> GetUserByIdAsync(Guid id);
        Task<UserDTO?> CreateUserAsync(UserCreateDTO userCreateDto);
        Task<bool> UpdateUserAsync(Guid id, UserUpdateDTO userUpdateDto);
        Task<bool> DeleteUserAsync(Guid id);
    }
}
