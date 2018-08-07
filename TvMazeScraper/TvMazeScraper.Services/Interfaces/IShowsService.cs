using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Domain.Models;

namespace TvMazeScraper.Services.Interfaces
{
    public interface IShowsService
    {
        Task<List<Show>> List(int page, int count);
        Task<Show> GetShow(int id);
        Task AddShows(IEnumerable<Show> shows);
    }
}