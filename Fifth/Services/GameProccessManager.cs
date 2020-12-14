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
    public class GameProccessManager : IGameProccessManager
    {

        private readonly IUnitOfWork unitOfWork;

        private readonly IGamesCrudService gamesCrudService;

        private readonly IHubContext<MainHub> hubContext;

        private readonly IUserCrudService userCrudService;

        private readonly IMapper mapper;


        public GameProccessManager(IHubContext<MainHub> hubContext, IMapper mapper, IGamesCrudService gamesCrudService, IUnitOfWork unitOfWork, IUserCrudService userCrudService)
        {
            this.hubContext = hubContext;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.gamesCrudService = gamesCrudService;
            this.userCrudService = userCrudService;
        }

        public async Task CloseGameAsync(int id)
        {
            await gamesCrudService.DeleteAsync(id);
        }

        public async Task<bool> EnterGameAsync(string connectionId, string userName ,int gameId)
        {

            return true;
        }

        public async Task<int> CreateGameAsync(CreateGameVM createGameVM)
        {
            var user = await userCrudService.GetByLoginAsync(createGameVM.Username);
            var gameId = await gamesCrudService.CreateAsync(createGameVM.Name, user);

            return gameId;
        }

        public async Task<IList<GameSessionVM>> GetOpenedGamesAsync()
        {
            var openedSessions = unitOfWork.DbContext.GameInfoDatas.Where(g => !g.Started).Include(t => t.Creator);
            var VMs = mapper.ProjectTo<GameSessionVM>(openedSessions);
            return await VMs.ToListAsync();
        }

        private async void OnGameCreated(int gameId)
        {
            var gameVM = await gamesCrudService.GetGameAsync(gameId);
            await hubContext.Clients.All.SendAsync("OnGameCreated", gameVM.GameInstance);
        }

    }
}