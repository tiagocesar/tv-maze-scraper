using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TvMazeScraper.Configuration.Options;
using TvMazeScraper.Services.Factories;

namespace TvMazeScraper.Configuration.DependencyInjection
{
    public static class Persistence
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<MongoDbOptions>(configuration.GetSection("Database:MongoDb"));
            services.Configure<TvMazeAPIOptions>(configuration.GetSection("TvMazeAPI"));
            
            services.AddSingleton<IMongoDbClientFactory, ClientFactory>();
            services.AddSingleton<IRestSharpClientFactory, ClientFactory>();
            
            return services;
        }
    }
}