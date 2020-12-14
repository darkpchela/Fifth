using Fifth.Models;
using Fifth.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fifth.Interfaces
{
    public interface IGamesCrudService
    {
        Task CreateAsync(CreateGameVM createModel);

        Task DeleteAsync(int id);

        Task<GameInstance> GetInstance(int id);

        Task<IEnumerable<GameInstance>> GetAllInstances();

        Task<GameData> GetDataAsync(int id);

        Task<IEnumerable<GameData>> GetAllDatasAsync();
    }
}