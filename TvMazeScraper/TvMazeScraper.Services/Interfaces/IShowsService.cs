using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Domain.Models;

namespace TvMazeScraper.Services.Interfaces
{
    public interface IShowsService
    {
        Task<List<Show>> List();
        Task<Show> GetShow(int id);
        Task AddShow(Show show);
        Task AddShows(IEnumerable<Show> shows);
    }
}