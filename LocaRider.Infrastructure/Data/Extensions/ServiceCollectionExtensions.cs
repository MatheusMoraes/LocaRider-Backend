using LocaRider.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace LocaRider.Infrastructure.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLocaRiderData(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LocaRiderDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Postgres")));

            services.AddSingleton<LocaRiderMongoDbContext>(sp =>
            {
                var mongoConnection = configuration.GetConnectionString("Mongo");
                var mongoDatabase = new MongoUrl(mongoConnection).DatabaseName;

                return new LocaRiderMongoDbContext(mongoConnection, mongoDatabase);
            });

            return services;
        }
    }
}
