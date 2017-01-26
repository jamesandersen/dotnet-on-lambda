
using Newtonsoft.Json;
using StarWarsMicroservice.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace StarWarsMicroservice.Services
{
    public class FakeStarWarsService : IStarWarsService
    {
        static List<Character> characters;
        ILogger<FakeStarWarsService> _logger;

        static FakeStarWarsService()
        {
            var serializer = new JsonSerializer();
            using (var sr = new StreamReader(GetEmbeddedJSONResource("characters.json")))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                characters = serializer.Deserialize<List<Character>>(jsonTextReader);
            }
        }

        public FakeStarWarsService(ILogger<FakeStarWarsService> logger) {
            _logger = logger;
        }

        public Task<IEnumerable<Character>> GetCharacters(int limit = 5)
        {
            return Task.FromResult(characters.Take(limit));
        }

        public Task<IEnumerable<Character>> SearchCharacters(string query)
        {
            return Task.FromResult(characters.Where(c => 
                c.Name.ToLower().Contains(query.ToLower())));
        }

        public Task<Character> CreateCharacter(Character newCharacter)
        {
            _logger.LogWarning($"{newCharacter.Name} not created because this is a fake service :-)");
            newCharacter.Url = $"http://swapi.co/api/people/{new Random().Next() + 10000}/";
            return Task.FromResult(newCharacter);
        }

        private static Stream GetEmbeddedJSONResource(string filename)
        {
            var assembly = typeof(FakeStarWarsService).GetTypeInfo().Assembly;
            var shortName = assembly.FullName.Substring(0, assembly.FullName.IndexOf(","));
            var resourceName = $"{shortName}.Services.{filename}";
            var names = assembly.GetManifestResourceNames();

            return assembly.GetManifestResourceStream(resourceName);
        }
    }
}