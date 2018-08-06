using System.Collections.Generic;
using System.Linq;
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
        public IEnumerable<Show> Get(int page) => _tvShowScraperService.ScrapeShowsInfo(page);

        [HttpGet]
        public List<(int page, int itens)> ScrapeShows()
        {
            var status = _tvShowScraperService.ScrapeShows().ToList();

            return status;
        }
    }
}