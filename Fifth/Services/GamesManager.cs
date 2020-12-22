﻿using Fifth.Interfaces;
using Fifth.Interfaces.DataAccess;
using Fifth.Models;
using Fifth.ViewModels;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;

namespace Fifth.Services
{
    public class GamesManager : IGamesManager
    {
        private readonly IHubContext<MainHub> hubContext;

        private readonly ITagsProvider tagsProvider;

        private readonly IUnitOfWork unitOfWork;

        public GamesManager(IHubContext<MainHub> hubContext, IUnitOfWork unitOfWork, ITagsProvider tagsProvider)
        {
            this.hubContext = hubContext;
            this.unitOfWork = unitOfWork;
            this.tagsProvider = tagsProvider;
        }

        public async Task CloseGameAsync(int id)
        {
            unitOfWork.SessionDataRepository.Delete(id);
            unitOfWork.GameProcessRepository.Delete(id.ToString());
            await unitOfWork.SaveChanges();
            await OnGameClosed();
        }

        public async Task<int> CreateGameAsync(CreateGameVM createGameVM, string userName)
        {
            if (unitOfWork.SessionDataRepository.GetAll().Any(sd => sd.Name == createGameVM.Name))
                return -1;
            var user = unitOfWork.UserRepository.Get(userName);
            var data = new SessionData
            {
                Creator = user,
                Name = createGameVM.Name
            };
            unitOfWork.SessionDataRepository.Create(data);
            await unitOfWork.SaveChanges();
            var proccess = new GameProcess(data.Id.ToString());
            unitOfWork.GameProcessRepository.Create(proccess);
            await tagsProvider.AssignTags(data.Id, createGameVM.Tags);
            await OnGameCreated();
            return data.Id;
        }

        public Task<SessionData> GetData(int id)
        {
            var data = unitOfWork.SessionDataRepository.Get(id);
            return Task.FromResult(data);
        }

        public Task<GameProcess> GetProcess(int id)
        {
            var process = unitOfWork.GameProcessRepository.Get(id.ToString());
            return Task.FromResult(process);
        }

        public async Task<bool> IsAlive(int id)
        {
            var proccess = unitOfWork.GameProcessRepository.Get(id.ToString());
            var data = unitOfWork.SessionDataRepository.Get(id);
            if (data is null || proccess is null)
            {
                await CloseGameAsync(id);
                return false;
            }
            return true;
        }

        public async Task SetStart(int gameId)
        {
            var data = await GetData(gameId);
            if (data is null)
                return;
            data.Started = true;
            unitOfWork.SessionDataRepository.Update(data);
            await unitOfWork.SaveChanges();
            await OnGameStarted();
        }

        private async Task OnGameCreated()
        {
            await hubContext.Clients.All.SendAsync("UpdateGamesTable");
        }

        private async Task OnGameStarted()
        {
            await hubContext.Clients.All.SendAsync("UpdateGamesTable");
        }

        private async Task OnGameClosed()
        {
            await hubContext.Clients.All.SendAsync("UpdateGamesTable");
        }
    }
}