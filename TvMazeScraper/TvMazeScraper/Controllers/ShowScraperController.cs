using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TvMazeScraper.Services.Interfaces;

namespace TvMazeScraper.Controllers
{
    [Route("api/[controller]")]
    public class ShowScraperController : Controller
    {
        private readonly ITvShowScraperService _tvShowScraperService;

        public ShowScraperController(ITvShowScraperService tvShowScraperService)
        {
            _tvShowScraperService = tvShowScraperService;
        }

        [HttpPost]
        public async Task ScrapeShows() => await _tvShowScraperService.ScrapeShows();
    }
}