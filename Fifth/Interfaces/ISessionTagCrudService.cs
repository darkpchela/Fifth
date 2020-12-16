using Fifth.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifth.Interfaces
{
    public interface ISessionTagCrudService
    {
        Task CreateAsync(int sessionId, int tagId);

        Task<IEnumerable<SessionData>> GetSessionsByTagAsync(IEnumerable<int> tagId);

        Task<IEnumerable<Tag>> GetTagsBySessionAsync(int sessionId);

        Task<IQueryable<SessionTag>> GetAll();
    }
}