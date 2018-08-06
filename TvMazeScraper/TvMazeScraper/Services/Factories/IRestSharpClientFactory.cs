using RestSharp;

namespace TvMazeScraper.Services.Factories
{
    public interface IRestSharpClientFactory
    {
        IRestClient GetRestClient();
    }
}