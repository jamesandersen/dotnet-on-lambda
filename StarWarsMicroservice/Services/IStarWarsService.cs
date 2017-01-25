using StarWarsMicroservice.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarWarsMicroservice.Services
{
    public interface IStarWarsService
    {
        Task<IEnumerable<Character>> GetCharacters(int limit = 5);
        Task<IEnumerable<Character>> SearchCharacters(string query);
        Task<Character> CreateCharacter(Character character);
    }
}