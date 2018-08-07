using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TvMazeScraper.Domain.Models;
using TvMazeScraper.Services.Interfaces;

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
        public async Task<List<Show>> Get(int page, int count) => await _showsService.List(page, count);

        [HttpGet("{id}")]
        public async Task<Show> Get(int id) => await _showsService.GetShow(id);
    }
}