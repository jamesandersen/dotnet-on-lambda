using Microsoft.Extensions.Logging;using StarWarsMicroservice.Services;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace StarWarsMicroservice.Tests
{
    public class FakeStarWarsServiceTests
    {
        private FakeStarWarsService _fakeStarWarsService;

        public FakeStarWarsServiceTests() {
            _fakeStarWarsService = new FakeStarWarsService(Mock.Of<ILogger<FakeStarWarsService>>());
        }
        
        [Fact]
        public async Task LoadsCharacterDataInStaticConstructor()
        {
            var characters = await _fakeStarWarsService.GetCharacters(10);
            Assert.Equal(10, characters.Count());
        }

        [Fact]
        public async Task DeserializesCharacterDataInStaticConstructor()
        {
            var luke = (await _fakeStarWarsService.GetCharacters(1)).First();
            Assert.Equal("Luke Skywalker", luke.Name);
            Assert.Equal("http://swapi.co/api/people/1/", luke.Url);
            Assert.Equal("blue", luke.EyeColor);
            Assert.Equal("19BBY", luke.BirthYear);
        }
    }
}