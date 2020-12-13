using AutoMapper;
using Fifth.Interfaces;
using Fifth.Models;
using Fifth.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifth.Services
{
    public class GameManageService : IGameManageService
    {
        private readonly AppDbContext dbContext;
        
        private readonly IHubContext<MainHub> hubContext;

        private readonly IMapper mapper;

        private readonly IHttpContextAccessor httpContextAccessor;

        public GameManageService(IHubContext<MainHub> hubContext, AppDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.hubContext = hubContext;
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task CloseGameAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EnterGameAsync(string connectionId, string userName, int gameId)
        {
            var game = await dbContext.GameSessions.FirstOrDefaultAsync(s => s.Id == gameId);
            if (game is null || game.Started)
                return false;

            var newPlayer = await dbContext.Users.FirstOrDefaultAsync(u => u.Login == userName);
            if (newPlayer is null)
                return false;

            game.OpponentId = newPlayer.Id;
            game.Started = true;
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<int> CreateGameAsync(CreateGameVM createGameVM)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Login == httpContextAccessor.HttpContext.User.Identity.Name);
            var session = new GameSession
            {
                OwnerId = user.Id,
                Name = createGameVM.GameName
            };
            dbContext.GameSessions.Add(session);
            await dbContext.SaveChangesAsync();
            OnGameCreated(session);
            return session.Id;
        }

        public async Task<IList<GameSessionVM>> GetAllSessionsAsync()
        {
            var openedSessions = dbContext.GameSessions.Where(s => !s.Started).Include(t => t.Owner);
            var VMs = mapper.ProjectTo<GameSessionVM>(openedSessions);
            return await VMs.ToListAsync();
        }

        private async void OnGameCreated(GameSession gameSession)
        {
            var sessionVm = new GameSessionVM
            {
                Name = gameSession.Name,
                UserName = (await dbContext.Users.FirstOrDefaultAsync(u => u.Id == gameSession.OwnerId)).Login
            };
            await hubContext.Clients.All.SendAsync("OnGameCreated", sessionVm);
        }

    }
}