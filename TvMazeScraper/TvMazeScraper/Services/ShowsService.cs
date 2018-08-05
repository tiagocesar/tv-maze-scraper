using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using TvMazeScraper.Configuration.Options;
using TvMazeScraper.Models;

namespace TvMazeScraper.Services
{
    public class ShowsService : IShowsService
    {
        private readonly MongoClient _client;
        private readonly MongoDbOptions _mongoDbOptions;
        
        public ShowsService(IMongoDbClientFactory clientFactory, MongoDbOptions mongoDbOptions)
        {
            _client = clientFactory.GetClient();
            _mongoDbOptions = mongoDbOptions;
        }

        private IMongoCollection<Show> GetShowsCollection()
        {
            var db = _client.GetDatabase(_mongoDbOptions.Database);
            
            var collection = db.GetCollection<Show>("show");

            return collection;
        }
        

        public async Task<List<Show>> List()
        {
            var collection = GetShowsCollection();

            var shows = await collection.AsQueryable<Show>().ToListAsync();

            return shows;
        }

        public Task<Show> GetShow(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task AddShow(Show show)
        {
            var collection = GetShowsCollection();

            await collection.InsertOneAsync(show);
        }
    }
}