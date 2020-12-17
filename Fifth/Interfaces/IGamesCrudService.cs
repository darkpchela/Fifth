﻿using Fifth.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fifth.Interfaces
{
    public interface IGamesCrudService
    {
        Task CreateAsync(string gameName, User userCreator);

        Task UpdateAsync(GameSession game);

        Task DeleteAsync(int id);

        Task<GameSession> GetGameAsync(int id);

        Task<GameSession> GetGameAsync(string name);

        Task<IEnumerable<GameSession>> GetAllGamesAsync();
    }
}