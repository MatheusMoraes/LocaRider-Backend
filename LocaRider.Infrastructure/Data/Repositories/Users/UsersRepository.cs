using LocaRider.Domain.Entities.Users;
using LocaRider.Domain.Interfaces.Users;
using LocaRider.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace LocaRider.Infrastructure.Data.Repositories.Users
{
    public class UsersRepository : IUsersRepository
    {
        private readonly LocaRiderDbContext _db;

        public UsersRepository(LocaRiderDbContext db)
        {
            _db = db;
        }

        public async Task<User> AddAsync(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task UpdateAsync(User user)
        {
            // Marca a entidade como modificada
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
        }
    }
}
