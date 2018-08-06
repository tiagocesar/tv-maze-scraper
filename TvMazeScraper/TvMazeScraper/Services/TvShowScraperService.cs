using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RestSharp;
using TvMazeScraper.Configuration.Options;
using TvMazeScraper.Models;
using TvMazeScraper.Services.Factories;
using TvMazeScraper.Services.Utils;

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

        public IEnumerable<Show> ScrapeShowsInfo(int page)
        {
            if (page == default)
            {
                throw new ArgumentException("Specify a valid page number");
            }
            
            var request = GetRequest();
            var shows = GetResponse(request);

            return shows;
            
            RestRequest GetRequest()
            {
                var endpoint = _tvMazeAPIOptions.ShowPaginationTemplate;

                var showRequest = new RestRequest(endpoint, Method.GET);
                showRequest.AddUrlSegment("page", page);

                return showRequest;
            }

            IEnumerable<Show> GetResponse(IRestRequest req)
            {
                var client = _clientFactory.GetRestClient();
                var policy = GetRetryPolicy();

                var showResponse = client.ExecuteWithPolicy(req, policy);

                return JsonConvert.DeserializeObject<List<Show>>(showResponse.Content);
            }
        }

        // ReSharper disable once MemberCanBeMadeStatic.Local
        private RetryPolicy<IRestResponse> GetRetryPolicy()
        {
            var jitterer = new Random();

            var policy = Policy
                .HandleResult<IRestResponse>(response => response.StatusCode == (HttpStatusCode)429)
                .Or<WebException>()
                .WaitAndRetry(5,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                                    + TimeSpan.FromMilliseconds(jitterer.Next(0, 100)));

            return policy;
        }
    }
}