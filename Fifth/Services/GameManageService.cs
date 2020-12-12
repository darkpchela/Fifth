using Fifth.Interfaces;
using Fifth.Models;
using Fifth.ViewModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fifth.Services
{
    public class GameManageService : IGameManageService
    {
        private List<GameSession> gameSessions = new List<GameSession>();

        private IHubContext<MainHub> hubContext;

        private FifthDbContext dbContext;

        public GameManageService(IHubContext<MainHub> hubContext, FifthDbContext dbContext)
        {
            this.hubContext = hubContext;
            this.dbContext = dbContext;
        }

        public Task CloseGameAsync()
        {
            throw new NotImplementedException();
        }

        public async Task CreateGameAsync(GameSessionVM createGameVM)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Login == createGameVM.UserName) ?? await CreateTempUser();
            var session = new GameSession
            {
                OwnerId = user.Id,
                SessionName = createGameVM.GameName
            };
            dbContext.GameSessions.Add(session);
            await dbContext.SaveChangesAsync();
            OnGameCreated(session);
        }

        private async void OnGameCreated(GameSession gameSession)
        {
            var sessionVm = new GameSessionVM
            {
                GameName = gameSession.SessionName,
                UserName = (await dbContext.Users.FirstOrDefaultAsync(u => u.Id == gameSession.OwnerId)).Login
            };
            await hubContext.Clients.All.SendAsync("OnGameCreated", sessionVm);
        }

        private async Task<User> CreateTempUser()
        {
            var user = new User
            {
                Login = Guid.NewGuid().ToString(),
                IsTemp = true
            };
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            return user;
        }

        //private async IEnumerable<Tag> MapTags(IEnumerable<string> tags)
        //{
        //    var allTags = await dbContext.Tags.ToListAsync();

        //}
    }
}