using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TvMazeScraper.Services;
using TvMazeScraper.Services.Interfaces;

namespace TvMazeScraper.Configuration.DependencyInjection
{
    public static class Services
    {
        public static IServiceCollection AddServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<IShowsService, ShowsService>();
            
            services.AddHostedService<TvShowScraperService>();

            return services;
        }
    }
}