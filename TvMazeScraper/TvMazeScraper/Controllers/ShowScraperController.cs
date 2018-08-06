using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TvMazeScraper.Services;

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

        [HttpGet]
        public async Task<List<(int page, int itens)>> ScrapeShows()
        {
            var status = (await _tvShowScraperService.ScrapeShows()).ToList();

            return status;
        }
    }
}