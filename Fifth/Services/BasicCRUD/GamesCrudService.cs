using AutoMapper;
using Fifth.Interfaces;
using Fifth.Models;
using Fifth.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifth.Services
{
    public class GamesCrudService : IGamesCrudService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IMapper mapper;

        public GamesCrudService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task CreateAsync(CreateGameVM createGameVM)
        {
            var user = await  unitOfWork.DbContext.Users.FirstOrDefaultAsync(u => u.Login == createGameVM.Username);
            var gameData = new GameData
            {
                Creator = user,
                Name = createGameVM.Name
            };
            unitOfWork.DbContext.GameInfoDatas.Add(gameData);
            await unitOfWork.DbContext.SaveChangesAsync();
            var game = new GameInstance(gameData.Id.ToString());
            await unitOfWork.GamesStore.Create(game);
        }

        public async Task DeleteAsync(int gameId)
        {
            var gameData = await unitOfWork.DbContext.GameInfoDatas.FirstOrDefaultAsync(d=>d.Id == gameId);
            await unitOfWork.GamesStore.Delete(gameId);
            if (gameData is null)
                return;
            unitOfWork.DbContext.GameInfoDatas.Remove(gameData);
            await unitOfWork.DbContext.SaveChangesAsync();
        }

        public async Task<GameInstance> GetInstance(int id)
        {
            var game = await unitOfWork.GamesStore.Get(id);
            return game;
        }

        public async Task<GameData> GetDataAsync(int id)
        {
            var data = await unitOfWork.DbContext.GameInfoDatas.FirstOrDefaultAsync(d => d.Id == id);
            return data;
        }

        public async Task<IEnumerable<GameInstance>> GetAllInstances()
        {
            var instnaces = await unitOfWork.GamesStore.GetAll();
            return instnaces;
        }

        public async Task<IEnumerable<GameData>> GetAllDatasAsync()
        {
            return await unitOfWork.DbContext.GameInfoDatas.Include(t => t.Creator).ToListAsync();
        }

    }
}
