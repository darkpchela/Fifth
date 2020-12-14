using Fifth.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fifth.Interfaces
{
    public interface IGameProccessManager
    {
        Task<int> CreateGameAsync(CreateGameVM createGameVM);

        Task CloseGameAsync(int id);

        Task<bool> EnterGameAsync(string connectionId, string userName ,int gameId);

        Task<IList<GameSessionVM>> GetOpenedGamesAsync();
    }
}