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
    public class GameManageService : IGameManageService
    {
        private List<GameSession> gameSessions = new List<GameSession>();

        private IHubContext<MainHub> hubContext;

        private List<string> connections = new List<string>();

        public GameManageService(IHubContext<MainHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public Task CloseGameAsync()
        {
            throw new NotImplementedException();
        }

        public async Task CreateGameAsync(CreateGameVM createGameVM)
        {
            var session = new GameSession
            {
                Owner = createGameVM.UserName,
                SessionName = createGameVM.GameName,
                Tags = createGameVM.Tags
            };
            gameSessions.Add(session);
            OnGameCreated(session);
        }

        private async void OnGameCreated(GameSession gameSession)
        {
            await hubContext.Clients.All.SendAsync("OnGameCreated", new CreateGameVM { UserName="a", GameName="b" });
        }
    }
}
