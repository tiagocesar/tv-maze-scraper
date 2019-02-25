using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using TvMazeScraper.Domain.Models;
using TvMazeScraper.Domain.Options;
using TvMazeScraper.Services;
using TvMazeScraper.Services.Interfaces;
using Xunit;

namespace TvMazeScraper.Tests.Services
{
    public class ShowsServiceTests
    {
        private ShowsService GetShowsServiceForListExceptionTests()
        {
            var mongoCollectionMock = new Mock<IMongoCollection<Show>>();

            var mongoDatabaseMock = new Mock<IMongoDatabase>();

            mongoDatabaseMock.Setup(x => x.GetCollection<Show>(It.IsAny<string>(), null))
                .Returns(mongoCollectionMock.Object);

            var mongoClientMock = new Mock<IMongoClient>();
            mongoClientMock.Setup(x => x.GetDatabase(It.IsAny<string>(), null)).Returns(mongoDatabaseMock.Object);

            var clientFactoryMock = new Mock<IMongoDbClientFactory>();

            clientFactoryMock.Setup(x => x.GetMongoDbClient()).Returns(mongoClientMock.Object);

            var mongoDbOptionsMock = new Mock<IOptions<MongoDbOptions>>();

            mongoDbOptionsMock.Setup(x => x.Value).Returns(new MongoDbOptions());

            var loggerMock = new Mock<ILogger<ShowsService>>();

            var showsService = new ShowsService(clientFactoryMock.Object, mongoDbOptionsMock.Object, loggerMock.Object);

            return showsService;
        }

        [Fact]
        public async Task ListWithInvalidParametersShouldThrow()
        {
            var showsService = GetShowsServiceForListExceptionTests();

            await Assert.ThrowsAsync<ArgumentException>(() => showsService.List(0, 0));
        }

        [Fact]
        public async Task ListWithInvalidCountParameterShouldThrow()
        {
            var showsService = GetShowsServiceForListExceptionTests();

            await Assert.ThrowsAsync<ArgumentException>(() => showsService.List(1, 0));
        }

        [Fact]
        public async Task GetShowWithInvalidParameterShoudThrow()
        {
            var showService = GetShowsServiceForListExceptionTests();

            await Assert.ThrowsAsync<ArgumentException>(() => showService.GetShow(0));
        }

        [Fact]
        public async Task ShouldAddShows()
        {
            var clientFactoryMock = new Mock<IMongoDbClientFactory>();
            var mongoDbOptionsMock = new Mock<IOptions<MongoDbOptions>>();

            var mongoCollectionMock = new Mock<IMongoCollection<Show>>();

            var mongoDatabaseMock = new Mock<IMongoDatabase>();

            mongoDatabaseMock.Setup(x => x.GetCollection<Show>(It.IsAny<string>(), null))
                .Returns(mongoCollectionMock.Object);

            var mongoClientMock = new Mock<IMongoClient>();
            mongoClientMock.Setup(x => x.GetDatabase(It.IsAny<string>(), null)).Returns(mongoDatabaseMock.Object);

            clientFactoryMock.Setup(x => x.GetMongoDbClient()).Returns(mongoClientMock.Object);
            mongoDbOptionsMock.Setup(x => x.Value).Returns(new MongoDbOptions());

            var loggerMock = new Mock<ILogger<ShowsService>>();

            var showsService = new ShowsService(clientFactoryMock.Object, mongoDbOptionsMock.Object, loggerMock.Object);

            await showsService.AddShows(new List<Show>());

            mongoCollectionMock.Verify(x => x.InsertManyAsync(new List<Show>(), null, default(CancellationToken)),
                Times.Once);
        }
    }
}