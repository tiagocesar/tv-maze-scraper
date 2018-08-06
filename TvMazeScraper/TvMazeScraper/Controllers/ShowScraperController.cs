using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TvMazeScraper.Models;
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

        [HttpGet("{page}")]
        public IEnumerable<Show> Get(int page)
        {
            var shows = _tvShowScraperService.ScrapeShowsInfo(page);

            return shows;
        }
    }
}