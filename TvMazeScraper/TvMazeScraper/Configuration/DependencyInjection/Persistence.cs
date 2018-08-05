using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TvMazeScraper.Configuration.Options;
using TvMazeScraper.Services;

namespace TvMazeScraper.Configuration.DependencyInjection
{
    public static class Persistence
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IMongoDbClientFactory, ClientFactory>();
            services.Configure<MongoDbOptions>(configuration.GetSection("Database:MongoDb"));

            return services;
        }
    }
}