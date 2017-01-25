
using Newtonsoft.Json;
using StarWarsMicroservice.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace StarWarsMicroservice.Services
{
    public class FakeStarWarsService : IStarWarsService
    {
        static List<Character> characters;
        static FakeStarWarsService()
        {
            var serializer = new JsonSerializer();
            using (var sr = new StreamReader(GetEmbeddedJSONResource("characters.json")))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                characters = serializer.Deserialize<List<Character>>(jsonTextReader);
            }
        }

        public Task<IEnumerable<Character>> GetCharacters(int limit = 5)
        {
            return Task.FromResult(characters.Take(limit));
        }

        public Task<IEnumerable<Quote>> SearchQuotes(string query)
        {
            throw new NotImplementedException();
        }

        private static Stream GetEmbeddedJSONResource(string filename)
        {
            var assembly = typeof(FakeStarWarsService).GetTypeInfo().Assembly;
            var shortName = assembly.FullName.Substring(0, assembly.FullName.IndexOf(","));
            var resourceName = string.Format("{0}.Services.{1}", shortName, filename);
            var names = assembly.GetManifestResourceNames();

            return assembly.GetManifestResourceStream(resourceName);
        }
    }
}