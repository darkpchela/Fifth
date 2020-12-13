﻿using Fifth.Etc;
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
            await Clients.All.SendAsync("Test", $"{Context.ConnectionId} connected to mainHub");

            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("Test", $"{Context.ConnectionId} diconnected");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
