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

        public ShowsController(IShowsService showsService)
        {
            _showsService = showsService;
        }
        
        [HttpGet]
        public async Task<List<Show>> Get() => await _showsService.List();

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
    }
}