using MongoDB.Driver;

namespace TvMazeScraper.Services.Factories
{
    public interface IMongoDbClientFactory
    {
        MongoClient GetClient();
    }
}