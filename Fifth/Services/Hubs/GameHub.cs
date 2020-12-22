using Fifth.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Fifth.Services
{
    public class GameHub : Hub
    {
        private IGamesManager gamesManager;

        private IGameProcessManager gameProcessManager;

        public GameHub(IGamesManager gamesManager, IGameProcessManager gameProcessManager)
        {
            this.gamesManager = gamesManager;
            this.gameProcessManager = gameProcessManager;
        }

        public async override Task OnConnectedAsync()
        {
            int gameId = GetCurrentGameId();
            await Clients.Caller.SendAsync("AcceptConnectionId", Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
            if (await gameProcessManager.RegistPlayer(gameId, Context.ConnectionId, Context.User.Identity.Name))
                await gameProcessManager.StartGame(gameId);
            else
            {
                await Clients.Caller.SendAsync("OnDisconnect");
                Context.Abort();
            }
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            int gameId = GetCurrentGameId();
            var process = await gamesManager.GetProcess(gameId);
            if (process != null && process.GetState().Players.Any(p => p.ConnectionId == Context.ConnectionId))
            {
                await gamesManager.CloseGameAsync(gameId);
                await Clients.Group(gameId.ToString()).SendAsync("OnDisconnect");
            }
        }

        public async Task MakeMove(string position)
        {
            int gameId = GetCurrentGameId();
            if (!await gamesManager.IsAlive(gameId))
                await Clients.Group(gameId.ToString()).SendAsync("OnDisconnect");
            else if (int.TryParse(position, out int index))
                await gameProcessManager.MakeMove(gameId, Context.ConnectionId, index);
        }

        private int GetCurrentGameId()
        {
            var id = Context.GetHttpContext().Session.GetInt32("gameId");
            if (id.HasValue)
                return id.Value;
            else
                return -1;
        }
    }
}