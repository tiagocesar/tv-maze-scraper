using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using TvMazeScraper.Configuration.Options;
using TvMazeScraper.Models;

namespace TvMazeScraper.Services
{
    public class ShowsesService : IShowsService
    {
        private readonly MongoClient _client;
        private readonly MongoDbOptions _mongoDbOptions;
        
        public ShowsesService(IMongoDbClientFactory clientFactory, MongoDbOptions mongoDbOptions)
        {
            _client = clientFactory.GetClient();
            _mongoDbOptions = mongoDbOptions;
        }

        private IMongoCollection<BsonDocument> GetShowsCollection()
        {
            var db = _client.GetDatabase(_mongoDbOptions.Database);
            
            var collection = db.GetCollection<BsonDocument>("show");

            return collection;
        }
        

        public Task<List<Show>> List()
        {
            var collection = GetShowsCollection();
            
            collection.
        }

        public Task<Show> GetShow(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task AddShow(Show show)
        {
            var collection = GetShowsCollection();

            await collection.InsertOneAsync(show.ToBsonDocument());
        }
    }
}