using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TvMazeScraper.Services.Interfaces;

namespace TvMazeScraper.Controllers
{
    [Route("api/[controller]")]
    public class ShowScraperController : Controller
    {
        private readonly ITvShowScraperService _tvShowScraperService;

        private readonly ILogger<ShowScraperController> _logger;

        public ShowScraperController(ITvShowScraperService tvShowScraperService, ILogger<ShowScraperController> logger)
        {
            _tvShowScraperService = tvShowScraperService;

            _logger = logger;
        }

        [HttpPost]
        public async Task ScrapeShows()
        {
            _logger.LogInformation("Starting scraping of the shows API");

            await _tvShowScraperService.ScrapeShows();
        }
    }
}