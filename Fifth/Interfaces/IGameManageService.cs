using Fifth.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fifth.Interfaces
{
    public interface IGameManageService
    {
        Task<int> CreateGameAsync(CreateGameVM createGameVM);

        Task CloseGameAsync();

        Task<bool> EnterGameAsync(string connectionId, string userName ,int gameId);

        Task<IList<GameSessionVM>> GetAllSessionsAsync();
    }
}