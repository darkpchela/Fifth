﻿using Fifth.Interfaces;
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
            SessionData gameData = new SessionData(gameName, userCreator);
            unitOfWork.DbContext.Sessions.Add(gameData);
            await unitOfWork.DbContext.SaveChangesAsync();
            GameSession game = new GameSession(gameData);
            await unitOfWork.GamesStore.Create(game.GameInstance);
            return game.GameData.Id;
        }

        public async Task UpdateAsync(GameSession game)
        {
            unitOfWork.DbContext.Entry(game.GameData).State = EntityState.Modified;
            await unitOfWork.DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int gameId)
        {
            var gameData = await unitOfWork.DbContext.Sessions.FirstOrDefaultAsync(d => d.Id == gameId);
            await unitOfWork.GamesStore.Delete(gameId);
            if (gameData is null)
                return;
            unitOfWork.DbContext.Sessions.Remove(gameData);
            await unitOfWork.DbContext.SaveChangesAsync();
        }

        public async Task<GameSession> GetGameAsync(int id)
        {
            var gameData = await unitOfWork.DbContext.Sessions.FindAsync(id);
            var gameInstance = await unitOfWork.GamesStore.Get(id);
            var game = new GameSession(gameData, gameInstance);
            return game;
        }

        public async Task<IEnumerable<GameSession>> GetAllGamesAsync()
        {
            var games = new List<GameSession>();
            foreach (var gd in unitOfWork.DbContext.Sessions)
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