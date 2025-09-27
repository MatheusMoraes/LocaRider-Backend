using LocaRider.Domain.Events;
using MongoDB.Driver;

namespace LocaRider.Infrastructure.Data.Context
{
    public class LocaRiderMongoDbContext
    {
        private readonly IMongoDatabase _database;
        public LocaRiderMongoDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<MotorcycleNotification> Notifications
           => _database.GetCollection<MotorcycleNotification>("MotorcycleNotifications");
    }
}