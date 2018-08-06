using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using RestSharp;
using TvMazeScraper.Configuration.Options;
using TvMazeScraper.Models;
using TvMazeScraper.Services.Factories;
using TvMazeScraper.Services.Helpers;
using TvMazeScraper.Services.Mappers;

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

        public async Task ScrapeShows()
        {
            var endOfList = false;
            var page = 1;

            while (!endOfList)
            {
                var shows = (await ScrapeShowsInfo(page)).ToList();

                page++;

                if (!shows.Any())
                {
                    endOfList = true;
                }
            }
        }
        
        private async Task<IEnumerable<Show>> ScrapeShowsInfo(int page)
        {
            if (page == default)
            {
                throw new ArgumentException("Specify a valid page number");
            }

            var endpoint = _tvMazeAPIOptions.ShowPaginationTemplate;
            var showRequest = new RestRequest(endpoint, Method.GET);
                
            showRequest.AddUrlSegment("page", page);

            var client = _clientFactory.GetRestClient();

            var showsResponse = await client.ExecuteTaskAsync<List<Show>>(showRequest);
            
            if (showsResponse.ErrorException != null)
            {
                throw new Exception(showsResponse.ErrorMessage, showsResponse.ErrorException);
            }

            var shows = showsResponse.Data;
            
            await GetCastTasks(shows);
            
            return shows;
        }

        private async Task GetCastTasks(IEnumerable<Show> shows)
        {
            var tasks = shows.Select(GetCastFromAPI);

            await Task.WhenAll(tasks);
        }

        private async Task GetCastFromAPI(Show show)
        {
            var endpoint = _tvMazeAPIOptions.CastInfoTemplate;
            var castRequest = new RestRequest(endpoint, Method.GET);

            castRequest.AddUrlSegment("id", show.Id);
            
            var client = _clientFactory.GetRestClient();
            var ct = new CancellationToken();
            var policy = GetAsyncRetryPolicy<List<CastResult>>();
            
            var response = await client.ExecuteTaskAsyncWithPolicy(castRequest, ct, policy);
            
            if (response.ErrorException != null)
            {
                throw new Exception(response.ErrorMessage, response.ErrorException);
            }

            var castResult = response.Data;

            if (castResult != null)
            {
                var cast = castResult.Where(x => x != null).Select(CastMapper.Map);

                show.Cast = cast.OrderByDescending(x => x.Birthday).ToList();
            }
        }

        private static RetryPolicy<IRestResponse<T>> GetAsyncRetryPolicy<T>()
        {
            var jitterer = new Random();

            var policy = Policy
                .HandleResult<IRestResponse<T>>(result => result.StatusCode == (HttpStatusCode) 429)
                .Or<WebException>()
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                                                      + TimeSpan.FromMilliseconds(jitterer.Next(0, 100)));

            return policy;
        }
    }
}