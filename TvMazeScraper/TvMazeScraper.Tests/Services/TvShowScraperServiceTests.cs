using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TvMazeScraper.Domain.Models;
using TvMazeScraper.Domain.Options;
using TvMazeScraper.Services;
using TvMazeScraper.Services.Interfaces;
using Xunit;

namespace TvMazeScraper.Tests.Services
{
    public class TvShowScraperServiceTests
    {
        private TvShowScraperService GetService()
        {
            var restSharpClientFactoryMock = new Mock<IRestSharpClientFactory>();
            var tvMazeAPIOptions = new Mock<IOptions<TvMazeAPIOptions>>();
            var showsServiceMock = new Mock<IShowsService>();

            var logMock = new Mock<ILogger<TvShowScraperService>>();

            return new TvShowScraperService(restSharpClientFactoryMock.Object, tvMazeAPIOptions.Object,
                showsServiceMock.Object, logMock.Object);
        }

        [Fact]
        public async Task ScrapeShowsInfoWithInvalidParametersShouldThrow()
        {
            var tvShowScraperService = GetService();

            await Assert.ThrowsAsync<ArgumentException>(() => tvShowScraperService.ScrapeShowsInfo(0));
        }

        [Fact]
        public async Task ScrapeShowsShouldRunUntilAPIReturnsEmpty()
        {
            var restSharpClientFactoryMock = new Mock<IRestSharpClientFactory>();
            var tvMazeAPIOptions = new Mock<IOptions<TvMazeAPIOptions>>();
            var showsServiceMock = new Mock<IShowsService>();

            var logMock = new Mock<ILogger<TvShowScraperService>>();

            var tvShowScraperServiceMock = new Mock<TvShowScraperService>(restSharpClientFactoryMock.Object,
                tvMazeAPIOptions.Object, showsServiceMock.Object, logMock.Object);

            tvShowScraperServiceMock.Setup(x => x.ScrapeShowsInfo(1))
                .ReturnsAsync(new List<Show> {new Show {Id = 1, Name = "Test 1"}});
            tvShowScraperServiceMock.Setup(x => x.ScrapeShowsInfo(2))
                .ReturnsAsync(new List<Show> {new Show {Id = 2, Name = "Test 2"}});
            tvShowScraperServiceMock.Setup(x => x.ScrapeShowsInfo(3))
                .ReturnsAsync(new List<Show>());

            tvShowScraperServiceMock.CallBase = true;

            var tvShowScraperService = tvShowScraperServiceMock.Object;

            await tvShowScraperService.ScrapeShows();

            showsServiceMock.Verify(x => x.AddShows(It.IsAny<List<Show>>()), Times.Exactly(2));
        }
    }
}