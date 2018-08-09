using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RestSharp;
using TvMazeScraper.Domain.Options;
using TvMazeScraper.Services.Interfaces;

namespace TvMazeScraper.Services.Factories
{
    public class ClientFactory : IMongoDbClientFactory, IRestSharpClientFactory
    {
        private readonly MongoDbOptions _mongoDbOptions;
        private readonly TvMazeAPIOptions _tvMazeApiOptions;

        private readonly ILogger<ClientFactory> _logger;

        public ClientFactory(IOptions<MongoDbOptions> mongoDbOptions, IOptions<TvMazeAPIOptions> tvMazeAPIOptions,
            ILogger<ClientFactory> logger)
        {
            _mongoDbOptions = mongoDbOptions.Value;
            _tvMazeApiOptions = tvMazeAPIOptions.Value;
            
            _logger = logger;
        }

        public IMongoClient GetMongoDbClient()
        {
            try
            {
                var user = _mongoDbOptions.User;
                var password = _mongoDbOptions.Password;
                var database = _mongoDbOptions.Database;

                var mongoDbConnectionString = $"mongodb://{user}:{password}@ds113482.mlab.com:13482/{database}";

                var client = new MongoClient(mongoDbConnectionString);

                return client;
            }
            catch (Exception e)
            {
                _logger.LogError(500, e, "Failure when trying to retrieve a MongoDb client");
                
                throw;
            }
        }

        public IRestClient GetRestClient()
        {
            try
            {
                var client = new RestClient(_tvMazeApiOptions.Endpoint);

                return client;
            }
            catch (Exception e)
            {
                _logger.LogError(500, e, "Failure when trying to retrieve a Rest client");
                
                throw;
            }
        }
    }
}