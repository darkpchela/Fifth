using Fifth.Services;
using Fifth.Services.DataContext;

namespace Fifth.Interfaces
{
    public interface IUnitOfWork
    {
        public AppDbContext DbContext { get; }
        public IGameInstanceRepository GamesStore { get; }
    }
}