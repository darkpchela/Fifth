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
        private IGameManageService gameManageService;
        public GameHub(IGameManageService gameManageService)
        {
            this.gameManageService = gameManageService;
        }
        public async override Task OnConnectedAsync()
        {
            string currentId = Context.ConnectionId;
            int gameId = Context.GetHttpContext().Session.GetInt32("gameId").Value;

            //var res = await gameManageService.EnterGameAsync();
            await Clients.All.SendAsync("Test", $"{Context.ConnectionId} connected to game");


            await Groups.AddToGroupAsync(currentId, gameId.ToString());
            await base.OnConnectedAsync();
        }

        private async Task<bool> TryEntryGame(string connectionId, int gameId)
        {
            try
            {

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
