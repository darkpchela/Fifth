using AutoMapper;
using Fifth.Interfaces;
using Fifth.Interfaces.DataAccess;
using Fifth.Models;
using Fifth.ViewModels;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifth.Services
{
    public class GamesManager : IGamesManager
    {
        private readonly IHubContext<MainHub> hubContext;

        private readonly ITagsProvider tagsProvider;

        private readonly IUnitOfWork unitOfWork;

        private readonly IMapper mapper;

        public GamesManager(IHubContext<MainHub> hubContext, IMapper mapper, IUnitOfWork unitOfWork, ITagsProvider tagsProvider)
        {
            this.hubContext = hubContext;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.tagsProvider = tagsProvider;
        }

        public async Task CloseGameAsync(int id)
        {
            unitOfWork.SessionDataRepository.Delete(id);
            unitOfWork.GameProcessRepository.Delete(id.ToString());
            await unitOfWork.SaveChanges();
        }

        public async Task<int> CreateGameAsync(CreateGameVM createGameVM, string userName)
        {
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

        private async Task OnGameCreated()
        {
            await hubContext.Clients.All.SendAsync("UpdateGamesTable");
        }

        private async Task OnGameStartedOrClosed()
        {
            await hubContext.Clients.All.SendAsync("UpdateGamesTable");
        }

    }
}