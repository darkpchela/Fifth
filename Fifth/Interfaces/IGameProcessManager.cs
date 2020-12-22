using Fifth.Models;
using System.Threading.Tasks;

namespace Fifth.Interfaces
{
    public interface IGameProcessManager
    {
        Task<MoveResult> MakeMove(int gameId, string connectionId, int position);

        Task<bool> RegistPlayer(int gameId, string connectionId, string userName);

        Task<bool> StartGame(int gameId);
    }
}