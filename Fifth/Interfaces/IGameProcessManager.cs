using System.Threading.Tasks;

namespace Fifth.Interfaces
{
    public interface IGameProcessManager
    {
        Task MakeMove(int gameId, string connectionId, int position);

        Task<bool> RegistPlayer(int gameId, string connectionId, string userName);

        Task StartGame(int gameId);
    }
}