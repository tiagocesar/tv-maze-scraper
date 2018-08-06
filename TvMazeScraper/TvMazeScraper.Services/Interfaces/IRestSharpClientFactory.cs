using RestSharp;

namespace TvMazeScraper.Services.Interfaces
{
    public interface IRestSharpClientFactory
    {
        IRestClient GetRestClient();
    }
}