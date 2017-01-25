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

        // GET api/values
        [HttpGet("characters")]
        public async Task<IEnumerable<Character>> GetCharacters(int limit = 5)
        {
            _logger.LogDebug("Get {0} characters", limit);
            return await _starWarsService.GetCharacters(limit);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
