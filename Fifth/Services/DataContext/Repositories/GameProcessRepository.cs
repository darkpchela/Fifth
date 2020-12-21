using Fifth.Interfaces.DataAccess;
using Fifth.Models;
using System.Collections.Generic;
using System.Linq;

namespace Fifth.Services.DataContext.Repositories
{
    public class GameProcessRepository : IGameProcessRepository
    {
        private IGameProcessesContext processesContext;

        public GameProcessRepository(IGameProcessesContext processesContext)
        {
            this.processesContext = processesContext;
        }

        public void Create(GameProcess entity)
        {
            processesContext.GameProcesses.Add(entity);
        }

        public void Delete(string key)
        {
            var entity = processesContext.GameProcesses.FirstOrDefault(e => e.Id == key);
            if (entity != null)
                processesContext.GameProcesses.Remove(entity);
        }

        public GameProcess Get(string key)
        {
            var entity = processesContext.GameProcesses.FirstOrDefault(e => e.Id == key);
            return entity;
        }

        public IEnumerable<GameProcess> GetAll()
        {
            return processesContext.GameProcesses;
        }

        public void Update(GameProcess entity)
        {
            return;
        }
    }
}