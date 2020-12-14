using Fifth.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fifth.Interfaces
{
    public interface IGameInstanceRepository
    {
        Task Create(GameInstance entity);

        Task<GameInstance> Get(int id);

        Task<IEnumerable<GameInstance>> GetAll();

        Task Delete(int id);
    }
}