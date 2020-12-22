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

        private readonly IHubContext<GameHub> gameHubContext;

        public GameProcessManager(IGamesManager gamesManager, IUnitOfWork unitOfWork, IHubContext<GameHub> gameHubContext)
        {
            this.gamesManager = gamesManager;
            this.unitOfWork = unitOfWork;
            this.gameHubContext = gameHubContext;
        }

        public async Task MakeMove(int gameId, string connectionId, int position)
        {
            var game = await gamesManager.GetProcess(gameId);
            using (GameMaster gameMaster = new GameMaster(game))
            {
                var res = gameMaster.MakeMove(position, connectionId, out MoveResult moveResult);
                if (!res)
                    return;
                await gameHubContext.Clients.Group(gameId.ToString()).SendAsync("OnMoveMaid", position);
                if (moveResult.GameFinished)
                    await gameHubContext.Clients.Group(gameId.ToString()).SendAsync("OnGameOver", moveResult);
            }
        }

        public async Task<bool> RegistPlayer(int gameId, string connectionId, string userName)
        {
            if (!await gamesManager.IsAlive(gameId))
                return false;
            var proccess = await gamesManager.GetProcess(gameId);
            using (GameMaster gameMaster = new GameMaster(proccess))
            {
                var res = gameMaster.RegistPlayer(connectionId, userName);
                return res;
            }
        }

        public async Task<bool> StartGame(int id)
        {
            if (!await gamesManager.IsAlive(id))
                return false;
            var proccess = await gamesManager.GetProcess(id);
            using (GameMaster gameMaster = new GameMaster(proccess))
            {
                var res = gameMaster.PrepareToStart();
                return res;
            }
        }
    }
}