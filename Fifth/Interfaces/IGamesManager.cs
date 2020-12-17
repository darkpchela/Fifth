using Fifth.Models;
using Fifth.ViewModels;
using System.Threading.Tasks;

namespace Fifth.Interfaces
{
    public interface IGamesManager
    {
        Task<int> OpenGameAsync(CreateGameVM createGameVM, string userName);

        Task CloseGameAsync(int id);

        Task<bool> TryEnterGameAsync(string connectionId, string userName, int gameId);

        Task<bool> TryStartGameAsync(GameSession game);
    }
}