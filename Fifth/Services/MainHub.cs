using Fifth.Etc;
using Fifth.Interfaces;
using Fifth.Models;
using Fifth.ViewModels;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifth.Services
{
    public class MainHub : Hub
    {
        
        public async override Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Test", new { 
                Context.ConnectionId,
                Context.User
            });

            await base.OnConnectedAsync();
        }
    }
}
