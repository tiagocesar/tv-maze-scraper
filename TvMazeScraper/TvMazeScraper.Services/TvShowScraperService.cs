using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using RestSharp;
using TvMazeScraper.Domain.Models;
using TvMazeScraper.Domain.Options;
using TvMazeScraper.Domain.Responses;
using TvMazeScraper.Services.Helpers;
using TvMazeScraper.Services.Interfaces;
using TvMazeScraper.Services.Mappers;

namespace TvMazeScraper.Services
{
    public class TvShowScraperService : ITvShowScraperService
    {
        private readonly IRestSharpClientFactory _clientFactory;
        private readonly TvMazeAPIOptions _tvMazeAPIOptions;
        private readonly IShowsService _showsService;

        private readonly ILogger<TvShowScraperService> _logger;

        public TvShowScraperService(IRestSharpClientFactory clientFactory, IOptions<TvMazeAPIOptions> tvMazeAPIOptions,
            IShowsService showsService, ILogger<TvShowScraperService> logger)
        {
            _clientFactory = clientFactory;
            _tvMazeAPIOptions = tvMazeAPIOptions.Value;
            _showsService = showsService;

            _logger = logger;
        }

        public async Task ScrapeShows()
        {
            _logger.LogInformation("Beginning scraping of the Shows API");

            var endOfList = false;
            var page = 1;

            while (!endOfList)
            {
                var shows = (await ScrapeShowsInfo(page)).ToList();

                page++;

                if (!shows.Any())
                {
                    endOfList = true;

                    continue;
                }

                await _showsService.AddShows(shows);
            }
        }

        public virtual async Task<IEnumerable<Show>> ScrapeShowsInfo(int page)
        {
            _logger.LogInformation("Scraping page {page} of the Shows API", page);

            try
            {
                var showRequest = GetRequest(page);

                var client = _clientFactory.GetRestClient();

                var showsResponse = await client.ExecuteTaskAsync<List<Show>>(showRequest);

                if (showsResponse.ErrorException != null)
                {
                    _logger.LogError(500, showsResponse.ErrorException, "Error when fetching data from the Shows API");

                    throw new Exception(showsResponse.ErrorMessage, showsResponse.ErrorException);
                }

                var shows = showsResponse.Data;

                await GetCastTasks(shows);

                return shows;
            }
            catch (Exception e)
            {
                _logger.LogError(500, e, "Failure when trying to scrape shows info, page {page}", page);

                throw;
            }
        }

        private IRestRequest GetRequest(int page)
        {
            if (page == default)
            {
                throw new ArgumentException("Specify a valid page number");
            }

            var req = new RestRequest(_tvMazeAPIOptions.ShowPaginationTemplate, Method.GET);

            req.AddUrlSegment("page", page);

            return req;
        }

        private async Task GetCastTasks(IEnumerable<Show> shows)
        {
            var tasks = shows.Select(GetCastFromAPI);

            await Task.WhenAll(tasks);
        }

        private async Task GetCastFromAPI(Show show)
        {
            var castRequest = GetRequest();

            var client = _clientFactory.GetRestClient();
            var ct = new CancellationToken();
            var policy = GetAsyncRetryPolicy<List<CastResult>>();

            var castResponse = await client.ExecuteTaskAsyncWithPolicy(castRequest, ct, policy);

            if (castResponse.ErrorException != null)
            {
                _logger.LogError(500, castResponse.ErrorException, "Error when fetching data from the Cast API");

                throw new Exception(castResponse.ErrorMessage, castResponse.ErrorException);
            }

            var castResult = castResponse.Data;

            if (castResult != null)
            {
                var cast = castResult.Where(x => x != null).Select(CastMapper.Map);

                show.Cast = cast.OrderByDescending(x => x.Birthday).ToList();
            }

            RestRequest GetRequest()
            {
                var req = new RestRequest(_tvMazeAPIOptions.CastInfoTemplate, Method.GET);

                req.AddUrlSegment("id", show.Id);

                return req;
            }
        }

        private static RetryPolicy<IRestResponse<T>> GetAsyncRetryPolicy<T>()
        {
            var jitterer = new Random();

            var policy = Policy
                .HandleResult<IRestResponse<T>>(result => result.StatusCode == (HttpStatusCode)429)
                .Or<WebException>()
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                                                      + TimeSpan.FromMilliseconds(jitterer.Next(0, 100)));

            return policy;
        }
    }
}