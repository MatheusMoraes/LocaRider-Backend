using LocaRider.Domain.Entities.Users;
using LocaRider.Domain.Interfaces.Users;
using LocaRider.Infrastructure.Data.Context;
using MongoDB.Driver;

namespace LocaRider.Infrastructure.Data.Repositories.Users
{
    public class UsersMongoDbRepository : IUsersRepository
    {

        private readonly IMongoCollection<User> _collection;

        public UsersMongoDbRepository(LocaRiderMongoDbContext mongoContext)
        {
            _collection = mongoContext.Database.GetCollection<User>("Users");
        }

        public async Task<User> AddAsync(User User)
        {
            await _collection.InsertOneAsync(User);
            return User;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.UserId, userId);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public Task UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}

