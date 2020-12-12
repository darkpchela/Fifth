using Fifth.ViewModels;
using System.Threading.Tasks;

namespace Fifth.Interfaces
{
    public interface IGameManageService
    {
        Task<int> CreateGameAsync(GameSessionVM gameSession);

        Task CloseGameAsync();

        Task<bool> EnterGameAsync(string connectionId, string userName ,int gameId);
    }
}