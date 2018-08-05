using MongoDB.Driver;
using TvMazeScraper.Configuration.Options;

namespace TvMazeScraper.Services
{
    public class ClientFactory : IMongoDbClientFactory
    {
        private readonly MongoDbOptions _mongoDbOptions;

        public ClientFactory(MongoDbOptions mongoDbOptions)
        {
            _mongoDbOptions = mongoDbOptions;
        }
        
        public MongoClient GetClient()
        {
            // TODO Move those properties to the appsettings
            var dbUser = _mongoDbOptions.User;
            var dbPassword = _mongoDbOptions.Password;

            var mongoDbConnectionString = $"mongodb://{dbUser}:{dbPassword}@ds113482.mlab.com:13482/tvmazescraper";

            var client = new MongoClient();

            return client;
        }   
    }
}