using Fifth.Services;

namespace Fifth.Interfaces
{
    public interface IUnitOfWork
    {
        public AppDbContext DbContext { get; }
        public IGameInstanceRepository GamesStore { get; }
    }
}