using LocaRider.Domain.Entities.Users;
using MongoDB.Driver;

namespace LocaRider.Infrastructure.Data.Context
{
    public class LocaRiderMongoDbContext
    {
        public IMongoDatabase Database { get; }

        public LocaRiderMongoDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            Database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<User> Usuarios => Database.GetCollection<User>("Usuarios");
    }
}