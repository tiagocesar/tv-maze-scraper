using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TvMazeScraper.Configuration.Options;

namespace TvMazeScraper.Services
{
    public class ClientFactory : IMongoDbClientFactory
    {
        private readonly MongoDbOptions _mongoDbOptions;

        public ClientFactory(IOptions<MongoDbOptions> mongoDbOptions)
        {
            _mongoDbOptions = mongoDbOptions.Value;
        }
        
        public MongoClient GetClient()
        {
            // TODO Move those properties to the appsettings
            var user = _mongoDbOptions.User;
            var password = _mongoDbOptions.Password;
            var database = _mongoDbOptions.Database;

            var mongoDbConnectionString = $"mongodb://{user}:{password}@ds113482.mlab.com:13482/{database}";

            var client = new MongoClient(mongoDbConnectionString);

            return client;
        }   
    }
}