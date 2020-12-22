using Fifth.Interfaces;
using Fifth.Interfaces.DataAccess;
using Fifth.Services.DataContext.Repositories;
using System.Threading.Tasks;

namespace Fifth.Services.DataContext
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext dbContext;

        private IGameProcessesContext processesContext;

        private IGameProcessRepository gameProcessRepository;

        private ISessionDataRepository sessionDataRepository;

        private ISessionTagRepository sessionTagRepository;

        private ITagRepository tagRepository;

        private IUserRepository userRepository; 

        public UnitOfWork(AppDbContext dbContext, IGameProcessesContext processesContext)
        {
            this.dbContext = dbContext;
            this.processesContext = processesContext;
        }

        public IGameProcessRepository GameProcessRepository
        {
            get
            {
                return gameProcessRepository ?? (gameProcessRepository = new GameProcessRepository(processesContext));
            }
        }

        public ISessionDataRepository SessionDataRepository
        {
            get
            {
                return sessionDataRepository ?? (sessionDataRepository = new SessionDataRepository(dbContext));
            }
        }

        public ISessionTagRepository SessionTagRepository
        {
            get
            {
                return sessionTagRepository ?? (sessionTagRepository = new SessionTagRepository(dbContext));
            }
        }

        public ITagRepository TagRepository
        {
            get
            {
                return tagRepository ?? (tagRepository = new TagRepository(dbContext));
            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                return userRepository ?? (userRepository = new UserRepository(dbContext));
            }
        }

        public async Task SaveChanges()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}