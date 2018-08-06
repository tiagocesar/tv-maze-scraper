using System.Collections.Generic;
using TvMazeScraper.Models;

namespace TvMazeScraper.Services
{
    public interface ITvShowScraperService
    {
        Show GetShowInfo(int id);
        IEnumerable<Show> ScrapeShowsInfo(int page);
    }
}