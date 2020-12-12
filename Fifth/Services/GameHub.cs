using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifth.Services
{
    public class GameHub : Hub
    {
        public async override Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("GameHubTest", "You connected to gameHUB");
            await base.OnConnectedAsync();
        }
    }
}
