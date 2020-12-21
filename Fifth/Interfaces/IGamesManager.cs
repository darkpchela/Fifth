using Fifth.Models;
using Fifth.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fifth.Interfaces
{
    public interface IGamesManager
    {
        Task<int> CreateGameAsync(CreateGameVM createGameVM, string userName);

        Task CloseGameAsync(int id);

        Task<SessionData> GetData(int id);

        Task<GameProcess> GetProcess(int id);

        bool IsAlive(int id);

    }
}