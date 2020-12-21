using Fifth.Interfaces.DataAccess;
using Fifth.Models;
using System.Collections.Generic;

namespace Fifth.Services.DataContext
{
    public class GameProcessesContext : IGameProcessesContext
    {
        private List<GameProcess> context;

        public IList<GameProcess> GameProcesses
        {
            get
            {
                return context ?? (context = new List<GameProcess>());
            }
        }
    }
}