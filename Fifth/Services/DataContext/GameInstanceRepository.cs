using Fifth.Interfaces;
using Fifth.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifth.Services.DataContext
{
    public class GameInstanceRepository : IGameInstanceRepository
    {
        private List<GameInstance> instances = new List<GameInstance>();

        public Task Create(GameInstance entity)
        {
            instances.Add(entity);
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var gi = instances.FirstOrDefault(e => e.Id == id.ToString());
            if (gi != null)
                instances.Remove(gi);
            return Task.CompletedTask;
        }

        public Task<GameInstance> Get(int id)
        {
            var gi = instances.FirstOrDefault(e => e.Id == id.ToString());
            return Task.FromResult(gi);
        }

        public Task<IEnumerable<GameInstance>> GetAll()
        {
            return Task.FromResult(instances.AsEnumerable());
        }
    }
}