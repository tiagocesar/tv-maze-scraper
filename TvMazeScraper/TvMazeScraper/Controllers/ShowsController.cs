using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TvMazeScraper.Models;
using TvMazeScraper.Services;

namespace TvMazeScraper.Controllers
{
    [Route("api/[controller]")]
    public class ShowsController : Controller
    {
        private readonly IShowsService _showsService;
        private ITvShowScraperService _showScraperService;

        public ShowsController(IShowsService showsService, ITvShowScraperService showScraperService)
        {
            _showsService = showsService;
            _showScraperService = showScraperService;
        }
        
        [HttpGet]
        public async Task<List<Show>> Get() => await _showsService.List();

        [HttpGet("{id}")]
        public Show Get(int id)
        {
            var show = _showScraperService.GetShowInfo(id);

            return show;
        }
    }
}