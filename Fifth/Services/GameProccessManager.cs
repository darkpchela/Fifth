using AutoMapper;
using Fifth.Interfaces;
using Fifth.Models;
using Fifth.ViewModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Fifth.Services
{
    public class GameProccessManager : IGameProccessManager
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IGamesCrudService gamesCrudService;

        private readonly IHubContext<MainHub> hubContext;

        private readonly IUserCrudService userCrudService;

        private readonly ISessionTagCrudService sessionTagCrudService;

        private readonly ITagCrudService tagCrudService;

        private readonly IMapper mapper;

        public GameProccessManager(IHubContext<MainHub> hubContext, IMapper mapper, IGamesCrudService gamesCrudService, 
            IUnitOfWork unitOfWork, IUserCrudService userCrudService, ISessionTagCrudService sessionTagCrudService, ITagCrudService tagCrudService)
        {
            this.hubContext = hubContext;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.gamesCrudService = gamesCrudService;
            this.userCrudService = userCrudService;
            this.sessionTagCrudService = sessionTagCrudService;
            this.tagCrudService = tagCrudService;
        }

        public async Task CloseGameAsync(int id)
        {
            await gamesCrudService.DeleteAsync(id);
            await OnGameStartedOrClosed(id);
        }

        public async Task<bool> TryEnterGameAsync(string connectionId, string login, int gameId)
        {
            var game = await gamesCrudService.GetGameAsync(gameId);
            if (!game.IsAlive())
                return false;
            var user = await userCrudService.GetByLoginAsync(login);
            var res = game.GameInstance.RegistPlayer(connectionId, user);
            return res;
        }

        public async Task<bool> TryStartGameAsync(GameSession game)
        {
            if (!game.IsAlive() || !game.GameInstance.IsReadyToStart)
                return false;
            game.Start();
            await gamesCrudService.UpdateAsync(game);
            await OnGameStartedOrClosed(game.GameData.Id);
            return true;
        }

        public async Task<int> OpenGameAsync(CreateGameVM createGameVM)
        {
            var user = await userCrudService.GetByLoginAsync(createGameVM.Username);
            var gameId = await gamesCrudService.CreateAsync(createGameVM.Name, user);
            var tags = JsonConvert.DeserializeObject<Tag[]>(createGameVM.Tags).Select(t=> t.Value);
            await UpdateTags(gameId, tags);
            await OnGameCreated(gameId);
            return gameId;
        }

        public async Task<IList<GameSessionVM>> GetOpenedGamesAsync()
        {
            var openedSessions = unitOfWork.DbContext.Sessions.Where(g => !g.Started).Include(t => t.Creator);
            var VMs = mapper.ProjectTo<GameSessionVM>(openedSessions);
            return await VMs.ToListAsync();
        }

        private async Task OnGameCreated(int gameId)
        {
            var game = await gamesCrudService.GetGameAsync(gameId);
            var gameVm = mapper.Map<GameSessionVM>(game.GameData);
            await hubContext.Clients.All.SendAsync("OnGameCreated", gameVm);
        }

        private async Task OnGameStartedOrClosed(int gameid)
        {
            await hubContext.Clients.All.SendAsync("OnGameStartedOrClosed", gameid);
        }

        private async Task UpdateTags(int gameId, IEnumerable<string> values)
        {
            foreach (var v in values)
            {
                Tag tag = await tagCrudService.GetAsync(v);
                if (tag is null)
                {
                    tag = new Tag
                    {
                        Value = v
                    };
                    await tagCrudService.CreateAsync(tag);
                }
                await sessionTagCrudService.CreateAsync(gameId, tag.Id);
            }
        }
    }
}