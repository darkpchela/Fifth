using Fifth.Interfaces;
using Fifth.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fifth.Services
{
    public class GamesCrudService : IGamesCrudService
    {
        private readonly IUnitOfWork unitOfWork;

        public GamesCrudService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<int> CreateAsync(string gameName, User userCreator)
        {
            if (await unitOfWork.DbContext.SessionDatas.AnyAsync(s => s.Name == gameName))
                return -1;
            SessionData gameData = new SessionData
            {
                Name = gameName,
                Creator = userCreator
            };
            unitOfWork.DbContext.SessionDatas.Add(gameData);
            await unitOfWork.DbContext.SaveChangesAsync();
            GameSession game = new GameSession(gameData);
            await unitOfWork.GamesStore.Create(game.Instance);
            return gameData.Id;
        }

        public async Task UpdateAsync(GameSession game)
        {
            unitOfWork.DbContext.Entry(game.Data).State = EntityState.Modified;
            await unitOfWork.DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int gameId)
        {
            var gameData = await unitOfWork.DbContext.SessionDatas.Include(t => t.Creator).FirstOrDefaultAsync(d => d.Id == gameId);
            if (gameData != null)
            {
                unitOfWork.DbContext.SessionDatas.Remove(gameData);
                unitOfWork.DbContext.SaveChanges();
            }
            await unitOfWork.GamesStore.Delete(gameId);
        }

        public async Task<GameSession> GetGameAsync(int id)
        {
            var gameData = await unitOfWork.DbContext.SessionDatas.FindAsync(id);
            var gameInstance = await unitOfWork.GamesStore.Get(id);
            var game = new GameSession(gameData, gameInstance);
            return game;
        }

        public async Task<GameSession> GetGameAsync(string name)
        {
            var gameData = await unitOfWork.DbContext.SessionDatas.FirstOrDefaultAsync(g=> g.Name == name);
            var gameInstance = await unitOfWork.GamesStore.Get(gameData.Id);
            var game = new GameSession(gameData, gameInstance);
            return game;
        }

        public async Task<IEnumerable<GameSession>> GetAllGamesAsync()
        {
            var games = new List<GameSession>();
            foreach (var gd in unitOfWork.DbContext.SessionDatas.Include(e => e.Creator))
            {
                var gi = await GetGameInstance(gd.Id);
                games.Add(new GameSession(gd, gi));
            }
            return games;
        }

        private async Task<GameInstance> GetGameInstance(int id)
        {
            var gameInstance = await unitOfWork.GamesStore.Get(id);
            return gameInstance;
        }
    }
}