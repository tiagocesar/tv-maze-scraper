using MongoDB.Driver;

namespace TvMazeScraper.Services
{
    public interface IMongoDbClientFactory
    {
        MongoClient GetClient();
    }
}