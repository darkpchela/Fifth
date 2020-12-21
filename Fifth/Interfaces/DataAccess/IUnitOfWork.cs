using System.Threading.Tasks;

namespace Fifth.Interfaces.DataAccess
{
    public interface IUnitOfWork
    {
        public IGameProcessRepository GameProcessRepository { get; }

        public ISessionDataRepository SessionDataRepository { get; }

        public ISessionTagRepository SessionTagRepository { get; }

        public ITagRepository TagRepository { get; }

        public IUserRepository UserRepository { get; }

        Task SaveChanges();
    }
}