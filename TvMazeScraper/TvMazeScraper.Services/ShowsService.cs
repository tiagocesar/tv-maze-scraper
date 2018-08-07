using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using TvMazeScraper.Domain.Models;
using TvMazeScraper.Domain.Options;
using TvMazeScraper.Services.Interfaces;

namespace TvMazeScraper.Services
{
    public class ShowsService : IShowsService
    {
        private readonly MongoClient _client;
        private readonly MongoDbOptions _mongoDbOptions;
        
        public ShowsService(IMongoDbClientFactory clientFactory, IOptions<MongoDbOptions> mongoDbOptions)
        {
            _client = clientFactory.GetMongoDbClient();
            _mongoDbOptions = mongoDbOptions.Value;
        }

        private IMongoCollection<Show> GetShowsCollection()
        {
            var db = _client.GetDatabase(_mongoDbOptions.Database);
            
            var collection = db.GetCollection<Show>("show");

            return collection;
        }
        
        public async Task<List<Show>> List(int page = 1, int count = 10)
        {
            if (count > 100)
            {
                throw new ArgumentException("The maximum number of documents per page is 100");
            }
            
            var elementsToSkip = (page - 1) * count;
            
            var collection = GetShowsCollection();

            var shows = await collection
                .Find(new BsonDocument())
                .Skip(elementsToSkip)
                .Limit(count)
                .ToListAsync();

            return shows;
        }

        public async Task<Show> GetShow(int id)
        {
            if (id == default)
            {
                throw new ArgumentException("Inform a valid show id");
            }

            var collection = GetShowsCollection();

            var filter = Builders<Show>.Filter.Eq("_id", id);

            var show = await collection.Find(filter).FirstOrDefaultAsync();

            return show;
        }

        public async Task AddShows(IEnumerable<Show> shows)
        {
            var collection = GetShowsCollection();

            await collection.InsertManyAsync(shows);
        }
    }
}