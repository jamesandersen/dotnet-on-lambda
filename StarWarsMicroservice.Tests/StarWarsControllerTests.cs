namespace StarWarsMicroservice.Tests
{
    using StarWarsMicroservice.Controllers;
    using StarWarsMicroservice.Models;
    using StarWarsMicroservice.Services;
    using Xunit;
    using Moq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using System.Linq;

    public class StarWarsControllerTests
    {
        private Mock<IStarWarsService> _mockStarWarsService;
        private Mock<ILogger<StarWarsController>> _mockLogger;
        private readonly StarWarsController _swController;

        public StarWarsControllerTests() {
            _mockStarWarsService = new Mock<IStarWarsService>();
            _mockLogger = new Mock<ILogger<StarWarsController>>();
            _swController = new StarWarsController(_mockStarWarsService.Object, _mockLogger.Object);
        }
        
        [Fact]
        public async Task FetchesCharactersFromService()
        {
            _mockStarWarsService.Setup(m => m.GetCharacters(It.IsAny<int>()))
                .Returns((int limit) => Task.FromResult(
                    Enumerable.Range(1,12).Select(i => new Character { Name = "Character " + i })
                ));

            var result = await _swController.GetCharacters(3);

            Assert.Equal(3, result.Count());

            _mockStarWarsService.Verify(m => m.GetCharacters(3), Times.Once);
        }
    }
}