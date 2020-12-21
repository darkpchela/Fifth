using Fifth.Models;
using System.Collections.Generic;

namespace Fifth.Interfaces.DataAccess
{
    public interface IGameProcessesContext
    {
        IList<GameProcess> GameProcesses { get; }
    }
}