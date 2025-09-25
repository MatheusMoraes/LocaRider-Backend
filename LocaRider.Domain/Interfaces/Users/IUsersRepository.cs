using LocaRider.Domain.Entities.Users;

namespace LocaRider.Domain.Interfaces.Users
{
    public interface IUsersRepository
    {
        Task<User> AddAsync(User User);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(Guid id);
        Task<IEnumerable<User>> GetAllAsync();

        Task UpdateAsync(User user);

        Task DeleteAsync(User user);
    }
}
