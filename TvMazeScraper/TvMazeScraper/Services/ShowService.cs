using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using TvMazeScraper.Configuration.Options;
using TvMazeScraper.Models;

namespace TvMazeScraper.Services
{
    public class ShowService : IShowService
    {
        private readonly MongoClient _client;
        private readonly MongoDbOptions _mongoDbOptions;
        
        public ShowService(IMongoDbClientFactory clientFactory, MongoDbOptions mongoDbOptions)
        {
            _client = clientFactory.GetClient();
            _mongoDbOptions = mongoDbOptions;
        }
        
        public async Task AddShow(Show show)
        {
            var db = _client.GetDatabase(_mongoDbOptions.Database);
            
            var collection = db.GetCollection<BsonDocument>("show");

            await collection.InsertOneAsync(show.ToBsonDocument());
        }
    }

    public interface IShowService
    {
        
    }
}