using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Domain.Models;

namespace TvMazeScraper.Services.Interfaces
{
    public interface ITvShowScraperService
    {
        Task ScrapeShows();
        Task<IEnumerable<Show>> ScrapeShowsInfo(int page);
    }
}