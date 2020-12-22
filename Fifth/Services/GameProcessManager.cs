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

        public async Task StartGame(int gameId)
        {
            if (!await gamesManager.IsAlive(gameId))
                return;
            var proccess = await gamesManager.GetProcess(gameId);
            using (GameMaster gameMaster = new GameMaster(proccess))
            {
                var res = gameMaster.PrepareToStart();
                if (!res)
                    return;
                await SendChars(gameId);
                await gameHubContext.Clients.Groups(gameId.ToString()).SendAsync("OnGameStarted");
            }
        }

        private async Task SendChars(int gameId)
        {
            var game = await gamesManager.GetProcess(gameId);
            var starter = game.GetState().CurrentPlayer;
            await gameHubContext.Clients.GroupExcept(game.Id, starter.ConnectionId).SendAsync("AcceptChar", "o");
            await gameHubContext.Clients.Client(starter.ConnectionId).SendAsync("AcceptChar", "x");
        }
    }
}