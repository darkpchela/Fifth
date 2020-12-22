using Fifth.Interfaces;
using Fifth.Models;
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
            await TryEnterGame(gameId);
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            int gameId = GetCurrentGameId();
            await Clients.Group(gameId.ToString()).SendAsync("OnDisconnect");
            await gamesManager.CloseGameAsync(gameId);
        }

        public async Task AcceptMoveRequest(string position)
        {
            int gameId = GetCurrentGameId();
            if (!await gamesManager.IsAlive(gameId))
                await Clients.Group(gameId.ToString()).SendAsync("OnDisconnect");
            else if (int.TryParse(position, out int index))
                await gameProcessManager.MakeMove(gameId, Context.ConnectionId, index);
        }

        private async Task TryEnterGame(int gameId)
        {
            var res = await gameProcessManager.RegistPlayer(gameId, Context.ConnectionId, Context.User.Identity.Name);
            if (!res)
            {
                await Clients.Group(gameId.ToString()).SendAsync("OnDisconnect");
                Context.Abort();
            }
            else
            { 
                var game = await gamesManager.GetProcess(gameId);
                await Clients.Group(gameId.ToString()).SendAsync("AcceptPlayersInfo", game.GetState().Players);
                await gameProcessManager.StartGame(gameId);
            }
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