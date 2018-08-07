using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using TvMazeScraper.Domain.Options;
using TvMazeScraper.Services;
using TvMazeScraper.Services.Interfaces;
using Xunit;

namespace TvMazeScraper.Tests.Services
{
    public class ShowsServiceTests
    {
        [Fact]
        public async Task ListWithInvalidParametersShouldRaiseException()
        {
            // public ShowsService(IMongoDbClientFactory clientFactory, IOptions<MongoDbOptions> mongoDbOptions)

            var clientFactoryMock = new Mock<IMongoDbClientFactory>();
            var mongoDbOptionsMock = new Mock<IOptions<MongoDbOptions>>();

            mongoDbOptionsMock.Setup(x => x.Value).Returns(new MongoDbOptions());

            var showsService = new ShowsService(clientFactoryMock.Object, mongoDbOptionsMock.Object);

            await Assert.ThrowsAsync<ArgumentException>(() => showsService.List(0, 0));
        }
    }
}