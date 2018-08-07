using MongoDB.Driver;

namespace TvMazeScraper.Services.Interfaces
{
    public interface IMongoDbClientFactory
    {
        IMongoClient GetMongoDbClient();
    }
}