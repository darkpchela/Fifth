using Fifth.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fifth.Interfaces
{
    public interface IGameProccessManager
    {
        Task<int> CreateGameAsync(CreateGameVM createGameVM);

        Task CloseGameAsync(int id);

        Task<bool> EnterGameAsync(string connectionId, int gameId);

        Task<IList<GameSessionVM>> GetOpenedGamesAsync();
    }
}