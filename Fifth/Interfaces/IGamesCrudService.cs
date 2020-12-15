using Fifth.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fifth.Interfaces
{
    public interface IGamesCrudService
    {
        Task<int> CreateAsync(string gameName, User userCreator);

        Task UpdateAsync(GameSession game);

        Task DeleteAsync(int id);

        Task<GameSession> GetGameAsync(int id);

        Task<IEnumerable<GameSession>> GetAllGamesAsync();
    }
}