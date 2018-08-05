using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Models;

namespace TvMazeScraper.Services
{
    public interface IShowsService
    {
        Task<List<Show>> List();
        Task<Show> GetShow(int id);
        Task AddShow(Show show);
    }
}