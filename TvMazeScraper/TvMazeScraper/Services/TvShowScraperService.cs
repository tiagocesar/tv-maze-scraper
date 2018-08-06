using System;
using Microsoft.Extensions.Options;
using RestSharp;
using TvMazeScraper.Configuration.Options;
using TvMazeScraper.Models;
using TvMazeScraper.Services.Factories;

namespace TvMazeScraper.Services
{
    public class TvShowScraperService : ITvShowScraperService
    {
        private readonly IRestSharpClientFactory _clientFactory;
        private readonly TvMazeAPIOptions _tvMazeAPIOptions;

        public TvShowScraperService(IRestSharpClientFactory clientFactory, IOptions<TvMazeAPIOptions> tvMazeAPIOptions)
        {
            _clientFactory = clientFactory;
            _tvMazeAPIOptions = tvMazeAPIOptions.Value;
        }

        public Show GetShowInfo(int id)
        {
            if (id == default)
            {
                throw new ArgumentException("Inform a valid show id");
            }
            
            var request = GetRequest();
            var show = GetResponse(request);

            return show;

            RestRequest GetRequest()
            {
                var showEndpoint = _tvMazeAPIOptions.ShowInfoTemplate;

                var showRequest = new RestRequest(showEndpoint, Method.GET);
                showRequest.AddUrlSegment("id", id);

                return showRequest;
            }

            Show GetResponse(IRestRequest req)
            {
                var client = _clientFactory.GetRestClient();
                
                var showResponse = client.Execute<Show>(req);

                return showResponse.Data;
            }
        }
    }
}