using AutoMapper;
using Fifth.Interfaces;
using Fifth.Models;
using Fifth.ViewModels;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace Fifth.Services
{
    public class GameProccessManager : IGameProccessManager
    {
        private readonly IGamesCrudService gamesCrudService;

        private readonly IHubContext<MainHub> hubContext;

        private readonly IUserCrudService userCrudService;

        private readonly ISessionTagCrudService sessionTagCrudService;

        private readonly ITagCrudService tagCrudService;

        private readonly IMapper mapper;

        public GameProccessManager(IHubContext<MainHub> hubContext, IMapper mapper, IGamesCrudService gamesCrudService,
            IUserCrudService userCrudService, ISessionTagCrudService sessionTagCrudService, ITagCrudService tagCrudService)
        {
            this.hubContext = hubContext;
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
            {
                await CloseGameAsync(gameId);
                return false;
            }
            var user = await userCrudService.GetByLoginAsync(login);
            var res = game.Instance.RegistPlayer(connectionId, user);
            return res;
        }

        public async Task<bool> TryStartGameAsync(GameSession game)
        {
            if (!game.IsAlive() || !game.Instance.IsReadyToStart)
                return false;
            game.Data.Started = true;
            game.Instance.StartGame();
            await gamesCrudService.UpdateAsync(game);
            await OnGameStartedOrClosed(game.Data.Id);
            return true;
        }

        public async Task<int> OpenGameAsync(CreateGameVM createGameVM, string userName)
        {
            var user = await userCrudService.GetByLoginAsync(userName);
            await gamesCrudService.CreateAsync(createGameVM.Name, user);
            var game = await gamesCrudService.GetGameAsync(createGameVM.Name);
            if (!game.IsAlive())
                return -1;
            await UpdateTags(game.Data.Id, createGameVM.Tags);
            await OnGameCreated(game);
            return game.Data.Id;
        }

        private async Task OnGameCreated(GameSession game)
        {
            ;
            var gameVm = mapper.Map<GameSessionVM>(game.Data);
            await hubContext.Clients.All.SendAsync("OnGameCreated", gameVm);
        }

        private async Task OnGameStartedOrClosed(int gameid)
        {
            await hubContext.Clients.All.SendAsync("OnGameStartedOrClosed", gameid);
        }

        private async Task UpdateTags(int gameId, string tagsJson)
        {
            if (string.IsNullOrEmpty(tagsJson))
                return;
            var values = JsonConvert.DeserializeObject<Tag[]>(tagsJson).Select(t => t.Value);
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