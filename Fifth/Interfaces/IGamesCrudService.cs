using Fifth.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fifth.Interfaces
{
    public interface IGamesCrudService
    {
        Task<int> CreateAsync(string gameName, User userCreator);

        Task UpdateAsync(Game game);

        Task DeleteAsync(int id);

        Task<Game> GetGameAsync(int id);

        Task<IEnumerable<Game>> GetAllGamesAsync();
    }
}