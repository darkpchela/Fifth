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

        public GameHub(IGameProccessManager gameManageService)
        {
            this.gameManageService = gameManageService;
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
            await Clients.Group(gameId.ToString()).SendAsync("Test", $"{Context.User.Identity.Name} left game. Game #{gameId} closed.");
            await base.OnDisconnectedAsync(exception);
        }

        private async Task TryStartGame()
        {
            int gameId = Context.GetHttpContext().Session.GetInt32("gameId").Value;
            var res = await gameManageService.TryStartGame(gameId);
            if (res)
                await Clients.Groups(gameId.ToString()).SendAsync("OnGameStarted");
        }

        public async Task MakeMove(int cellId)
        {

        }

        private async Task TryEnterGame()
        {
            int gameId = Context.GetHttpContext().Session.GetInt32("gameId").Value;
            var res = await gameManageService.TryEnterGameAsync(Context.ConnectionId, gameId);
            await Clients.Group(gameId.ToString()).SendAsync("OnPlayerEntered", Context.GetHttpContext().User.Identity.Name, Context.ConnectionId, res);
            if (!res)
            {
                await Clients.Caller.SendAsync("Diconnect");
                Context.Abort();
            }
        }
    }
}
