using Fifth.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifth.Services
{
    public class GameHub : Hub
    {
        private IGameProccessManager gameManageService;
        private IGamesCrudService gamesCrudService;

        public GameHub(IGameProccessManager gameManageService, IGamesCrudService gamesCrudService)
        {
            this.gameManageService = gameManageService;
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
            await gameManageService.CloseGameAsync(gameId);
            await Clients.Group(gameId.ToString()).SendAsync("Disconnect");
            Context.Abort();
            await base.OnDisconnectedAsync(exception);
        }

        private async Task TryStartGame()
        {
            int gameId = Context.GetHttpContext().Session.GetInt32("gameId").Value;
            var res = await gameManageService.TryStartGame(gameId);
            if (!res)
                return;
            await AssignChars(gameId);
            await Clients.Groups(gameId.ToString()).SendAsync("OnGameStarted");
        }

        public async Task MakeMove(string index)
        {
            int gameId = Context.GetHttpContext().Session.GetInt32("gameId").Value;
            var game = await gamesCrudService.GetGameAsync(gameId);
            if(game is null)
            {
                await Clients.Group(gameId.ToString()).SendAsync("Disconnect");
                return;
            }
            var res = game.GameInstance.TryMakeMove(int.Parse(index), Context.ConnectionId);
            if (res)
                await Clients.Group(gameId.ToString()).SendAsync("OnMoveMaid", index);
        }

        private async Task TryEnterGame()
        {
            int gameId = Context.GetHttpContext().Session.GetInt32("gameId").Value;
            var res = await gameManageService.TryEnterGameAsync(Context.ConnectionId, gameId);
            await Clients.Group(gameId.ToString()).SendAsync("OnPlayerEntered", Context.GetHttpContext().User.Identity.Name, Context.ConnectionId, res);
            if (!res)
            {
                await Clients.Caller.SendAsync("Disconnect");
                Context.Abort();
            }
        }

        private async Task AssignChars(int gameId)
        {
            var game = await gamesCrudService.GetGameAsync(gameId);
            var starter = game.GameInstance.CurrentPlayer;
            await Clients.GroupExcept(gameId.ToString(), starter).SendAsync("AcceptChar", "o");
            await Clients.Client(starter).SendAsync("AcceptChar", "x");
        }

    }
}
