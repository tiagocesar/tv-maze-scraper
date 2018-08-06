using System.Collections.Generic;
using System.Threading.Tasks;

namespace TvMazeScraper.Services
{
    public interface ITvShowScraperService
    {
        Task<IEnumerable<(int page, int itens)>> ScrapeShows();
    }
}