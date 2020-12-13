using AutoMapper;
using Fifth.Interfaces;
using Fifth.Models;
using Fifth.ViewModels;
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
        private readonly IHubContext<MainHub> hubContext;

        private readonly AppDbContext dbContext;

        private readonly IMapper mapper;

        public GameManageService(IHubContext<MainHub> hubContext, AppDbContext dbContext, IMapper mapper)
        {
            this.hubContext = hubContext;
            this.dbContext = dbContext;
            this.mapper = mapper;
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

        public async Task<int> CreateGameAsync(GameSessionVM createGameVM)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Login == createGameVM.UserName);
            var session = new GameSession
            {
                OwnerId = user.Id,
                SessionName = createGameVM.GameName
            };
            dbContext.GameSessions.Add(session);
            await dbContext.SaveChangesAsync();
            OnGameCreated(session);
            return session.Id;
        }

        public async Task<IList<GameSessionVM>> GetAllSessions()
        {
            var openedSessions = dbContext.GameSessions.Where(s => !s.Started);
            var VMs = mapper.ProjectTo<GameSessionVM>(openedSessions);
            return await VMs.ToListAsync();
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

    }
}