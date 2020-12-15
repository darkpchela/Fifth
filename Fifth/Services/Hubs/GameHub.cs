using Fifth.Interfaces;
using Fifth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Fifth.Services
{
    public class GameHub : Hub
    {
        private IGameProccessManager gameProccessManager;
        private IGamesCrudService gamesCrudService;

        public GameHub(IGameProccessManager gameProccessManager, IGamesCrudService gamesCrudService)
        {
            this.gameProccessManager = gameProccessManager;
            this.gamesCrudService = gamesCrudService;
        }

        public async override Task OnConnectedAsync()
        {
            int gameId = Context.GetHttpContext().Session.GetInt32("gameId").Value;
            await Clients.All.SendAsync("Test", $"{Context.User.Identity.Name} connected to hub");
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
            await TryEnterGame();
            await TryStartGame();
            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            int gameId = Context.GetHttpContext().Session.GetInt32("gameId").Value;
            await gameProccessManager.CloseGameAsync(gameId);
            await Clients.Group(gameId.ToString()).SendAsync("Disconnect");
            Context.Abort();
            await base.OnDisconnectedAsync(exception);
        }

        public async Task AcceptMoveRequest(string index)
        {
            int gameId = Context.GetHttpContext().Session.GetInt32("gameId").Value;
            var game = await gamesCrudService.GetGameAsync(gameId);
            if (!game.IsAlive() || !int.TryParse(index, out int posIndex))
                await Clients.Group(gameId.ToString()).SendAsync("Disconnect");
            else
                await HandleMoveRequest(game, posIndex);
        }

        private async Task TryStartGame()
        {
            int gameId = Context.GetHttpContext().Session.GetInt32("gameId").Value;
            Game game = await gamesCrudService.GetGameAsync(gameId);
            if (!game.IsAlive())
                return;
            else
                await HandleStartGameRequest(game);
        }

        private async Task HandleStartGameRequest(Game game)
        {
            var res = await gameProccessManager.TryStartGameAsync(game);
            if (res)
            {
                await AssignChars(game);
                await Clients.Groups(game.GameInstance.Id.ToString()).SendAsync("OnGameStarted");
            }
        }

        private async Task HandleMoveRequest(Game game, int index)
        {
            var res = game.GameInstance.MakeMove(index, Context.ConnectionId);
            if (res.MoveMaid)
                await Clients.Group(game.GameInstance.Id).SendAsync("OnMoveMaid", index);
            if (res.GameFinished)
                await Clients.Group(game.GameInstance.Id).SendAsync("OnGameOver", res.GameResult.ToString());
        }

        private async Task TryEnterGame()
        {
            int gameId = Context.GetHttpContext().Session.GetInt32("gameId").Value;
            var res = await gameProccessManager.TryEnterGameAsync(Context.ConnectionId, gameId);
            await Clients.Group(gameId.ToString()).SendAsync("OnPlayerEntered", Context.GetHttpContext().User.Identity.Name, Context.ConnectionId, res);
            if (!res)
            {
                await Clients.Caller.SendAsync("Disconnect");
                Context.Abort();
            }
        }

        private async Task AssignChars(Game game)
        {
            var starter = game.GameInstance.CurrentPlayer;
            await Clients.GroupExcept(game.GameInstance.Id, starter).SendAsync("AcceptChar", "o");
            await Clients.Client(starter).SendAsync("AcceptChar", "x");
        }
    }
}