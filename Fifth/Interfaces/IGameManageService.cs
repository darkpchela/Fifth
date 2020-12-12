using Fifth.ViewModels;
using System.Threading.Tasks;

namespace Fifth.Interfaces
{
    public interface IGameManageService
    {
        Task CreateGameAsync(GameSessionVM gameSession);

        Task CloseGameAsync();
    }
}