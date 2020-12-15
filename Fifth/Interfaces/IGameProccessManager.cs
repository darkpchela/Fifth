using Fifth.Models;
using Fifth.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fifth.Interfaces
{
    public interface IGameProccessManager
    {
        Task<int> CreateGameAsync(CreateGameVM createGameVM);

        Task CloseGameAsync(int id);

        Task<bool> TryEnterGameAsync(string connectionId, int gameId);

        Task<IList<GameSessionVM>> GetOpenedGamesAsync();

        Task<bool> TryStartGameAsync(Game game);
    }
}