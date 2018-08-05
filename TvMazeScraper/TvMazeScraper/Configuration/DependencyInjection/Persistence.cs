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
            services.Configure<MongoDbOptions>(configuration.GetSection("Database:MongoDb"));
            
            services.AddSingleton<IMongoDbClientFactory, ClientFactory>();

            return services;
        }
    }
}