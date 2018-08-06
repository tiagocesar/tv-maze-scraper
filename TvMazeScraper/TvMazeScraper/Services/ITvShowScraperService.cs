using TvMazeScraper.Models;

namespace TvMazeScraper.Services
{
    public interface ITvShowScraperService
    {
        Show GetShowInfo(int id);
    }
}