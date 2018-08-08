using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        private readonly IMongoClient _client;
        private readonly MongoDbOptions _mongoDbOptions;

        private readonly ILogger<ShowsService> _logger;

        public ShowsService(IMongoDbClientFactory clientFactory, IOptions<MongoDbOptions> mongoDbOptions,
            ILogger<ShowsService> logger)
        {
            _client = clientFactory.GetMongoDbClient();
            _mongoDbOptions = mongoDbOptions.Value;
            _logger = logger;
        }

        private IMongoCollection<Show> GetShowsCollection()
        {
            var db = _client.GetDatabase(_mongoDbOptions.Database);

            var collection = db.GetCollection<Show>("show");

            return collection;
        }

        private static FindOptions<Show> GetListOptions(int page, int count)
        {
            if (page == default)
            {
                throw new ArgumentException("Inform a valid page number");
            }

            if (count == default)
            {
                throw new ArgumentException("Inform a valid number of documents per page");
            }

            if (count > 100)
            {
                throw new ArgumentException("The maximum number of documents per page is 100");
            }

            var elementsToSkip = (page - 1) * count;

            var options = new FindOptions<Show> {Skip = elementsToSkip, Limit = count};

            return options;
        }

        public async Task<List<Show>> List(int page = 1, int count = 10)
        {
            try
            {
                var collection = GetShowsCollection();

                var options = GetListOptions(page, count);

                var cursor = await collection.FindAsync(new BsonDocument(), options);

                var shows = await cursor.ToListAsync();

                return shows;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Show> GetShow(int id)
        {
            ValidateParameters();

            try
            {
                _logger.LogInformation("Trying to get show with id {id}", id);
                
                var collection = GetShowsCollection();

                var filter = Builders<Show>.Filter.Eq("_id", id);

                var show = await collection.Find(filter).FirstOrDefaultAsync();

                return show;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            void ValidateParameters()
            {
                if (id == default)
                {
                    throw new ArgumentException("Inform a valid show id");
                }
            }
        }

        public async Task AddShows(IEnumerable<Show> shows)
        {
            var collection = GetShowsCollection();

            await collection.InsertManyAsync(shows);
        }
    }
}