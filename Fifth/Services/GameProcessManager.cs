using Fifth.Interfaces;
using Fifth.Interfaces.DataAccess;
using Fifth.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Fifth.Services
{
    public class GameProcessManager : IGameProcessManager
    {
        private IGamesManager gamesManager;

        private readonly IUnitOfWork unitOfWork;

        private readonly IHubContext<MainHub> hubContext;

        public GameProcessManager(IGamesManager gamesManager, IHubContext<MainHub> hubContext)
        {
            this.gamesManager = gamesManager;
            this.hubContext = hubContext;
        }

        public async Task<MoveResult> MakeMove(int gameId, string connectionId, int position)
        {
            var game = await gamesManager.GetProcess(gameId);
            var res = game.MakeMove(position, connectionId);
            return res;
        }

        public async Task<bool> RegistPlayer(int gameId, string connectionId, string userName )
        {
            if (!gamesManager.IsAlive(gameId))
                return false;
            var proccess = await gamesManager.GetProcess(gameId);
            if (proccess.Players.Count >= 2)
                return false;
            var connection = new UserConnection(userName, connectionId);
            proccess.Players.Add(connection);
            return true;
        }

        public async Task<bool> StartGame(int id)
        {
            if (!gamesManager.IsAlive(id))
                return false;
            var proccess = await gamesManager.GetProcess(id);
            if (proccess.Players.Count != 2)
                return false;
            proccess.PrepareToStart();
            var data = await gamesManager.GetData(id);
            data.Started = true;
            unitOfWork.SessionDataRepository.Update(data);
            await unitOfWork.SaveChanges();
            OnGameStarted();
            return true;
        }

        private async void OnGameStarted()
        {
            await hubContext.Clients.All.SendAsync("UpdateGamesTable");
        }
    }
}