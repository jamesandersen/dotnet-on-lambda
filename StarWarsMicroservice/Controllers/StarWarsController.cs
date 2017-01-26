using System.Collections.Generic;
using StarWarsMicroservice.Models;
using Microsoft.AspNetCore.Mvc;
using StarWarsMicroservice.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace StarWarsMicroservice.Controllers
{
    [Route("api/[controller]")]
    public class StarWarsController : Controller
    {
        private readonly IStarWarsService _starWarsService;
        private readonly ILogger<StarWarsController> _logger;

        public StarWarsController(IStarWarsService swService, ILogger<StarWarsController> logger)
        {
            _starWarsService = swService;
            _logger = logger;
        }

        // GET api/starwars/characters?limit=5
        [HttpGet("characters")]
        public async Task<IEnumerable<Character>> GetCharacters(int limit = 5)
        {
            _logger.LogDebug($"Get {limit} characters");
            return await _starWarsService.GetCharacters(limit);
        }

        // GET api/starwars/characters/search/skywalker
        [HttpGet("characters/search/{query}")]
        public async Task<IEnumerable<Character>> SearchCharacters(string query)
        {
            _logger.LogDebug($"Search for characters like '{query}'");
            return await _starWarsService.SearchCharacters(query);
        }

        // POST api/starwars/characters
        [HttpPost("characters")]
        public async Task<ActionResult> CreateCharacter([FromBody]Character newCharacter)
        {
            if (newCharacter == null || string.IsNullOrEmpty(newCharacter.Name)) {
                return BadRequest("Invalid star wars character data");
            }

            _logger.LogDebug($"Creating character for '{newCharacter.Name}'");
            var createdCharacter = await _starWarsService.CreateCharacter(newCharacter);
            return Ok(createdCharacter);
        }
    }
}
