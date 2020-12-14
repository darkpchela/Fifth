using Fifth.Interfaces;

namespace Fifth.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        public AppDbContext DbContext { get; }

        public IGameInstanceRepository GamesStore { get; }

        public UnitOfWork(AppDbContext dbContext, IGameInstanceRepository gameInstanceRepository)
        {
            this.DbContext = dbContext;
            this.GamesStore = gameInstanceRepository;
        }
    }
}