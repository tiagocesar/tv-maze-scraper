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
        [ProducesResponseType(200, Type = typeof(Show))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(int id)
        {
            var show = await _showsService.GetShow(id);

            if (show == null)
            {
                return NotFound();
            }

            return Ok(show);
        }
    }
}