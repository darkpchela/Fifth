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

        private readonly List<Game> gamesInstances = new List<Game>();

        public GameManageService(IHubContext<MainHub> hubContext, AppDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.hubContext = hubContext;
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task CloseGameAsync(int id)
        {
            var gameSession = dbContext.GameInfoDatas.Find(id);
            if (gameSession is null)
                return;
            dbContext.GameInfoDatas.Remove(gameSession);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> EnterGameAsync(string connectionId, string userName ,int gameId)
        {

            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<int> CreateGameAsync(CreateGameVM createGameVM)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Login == createGameVM.Username);
            var session = new GameInfoData
            {
                Creator = user,
                Name =createGameVM.Name,
            };
            dbContext.GameInfoDatas.Add(session);
            await dbContext.SaveChangesAsync();
            OnGameCreated(session);
            var game = new Game(session.Id.ToString());
            gamesInstances.Add(game);
            return session.Id;
        }

        public async Task<IList<GameSessionVM>> GetAllSessionsAsync()
        {
            var openedSessions = dbContext.GameInfoDatas.Where(s => !s.Started).Include(t => t.Creator);
            var VMs = mapper.ProjectTo<GameSessionVM>(openedSessions);
            return await VMs.ToListAsync();
        }

        private async void OnGameCreated(GameInfoData gameSession)
        {
            var gameVM = mapper.Map<GameSessionVM>(gameSession);
            await hubContext.Clients.All.SendAsync("OnGameCreated", gameVM);
        }

    }
}