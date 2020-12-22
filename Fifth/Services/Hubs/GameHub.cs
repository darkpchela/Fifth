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
            await TryStartGame(gameId);
            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            int gameId = GetCurrentGameId();
            await Clients.Group(gameId.ToString()).SendAsync("OnDisconnect");
            await gamesManager.CloseGameAsync(gameId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task AcceptMoveRequest(string index)
        {
            int gameId = GetCurrentGameId();
            if (!await gamesManager.IsAlive(gameId))
                await Clients.Group(gameId.ToString()).SendAsync("OnDisconnect");
            else if (int.TryParse(index, out int posIndex))
                await HandleMoveRequest(gameId, posIndex);
        }

        private async Task TryStartGame(int gameId)
        {
            var res = await gameProcessManager.StartGame(gameId);
            if (!res)
                return;
            var game = await gamesManager.GetProcess(gameId);
            await AssignChars(game);
            await Clients.Groups(game.Id.ToString()).SendAsync("OnGameStarted");
        }

        private async Task HandleMoveRequest(int id, int index)
        {
            var game = await gamesManager.GetProcess(id);
            var res = game.MakeMove(index, Context.ConnectionId);
            if (res.MoveMaid)
                await Clients.Group(game.Id).SendAsync("OnMoveMaid", index);
            if (res.GameFinished)
                await Clients.Group(game.Id).SendAsync("OnGameOver", res);
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
                await Clients.Group(gameId.ToString()).SendAsync("AcceptPlayersInfo", game.Players);
            }
        }

        private async Task AssignChars(GameProcess game)
        {
            var starter = game.CurrentPlayer;
            await Clients.GroupExcept(game.Id, starter.ConnectionId).SendAsync("AcceptChar", "o");
            await Clients.Client(starter.ConnectionId).SendAsync("AcceptChar", "x");
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