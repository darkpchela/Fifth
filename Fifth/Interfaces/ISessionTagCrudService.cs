using Fifth.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fifth.Interfaces
{
    public interface ISessionTagCrudService
    {
        Task CreateAsync(int sessionId, int tagId);

        Task<IEnumerable<SessionData>> GetSessionsByTagAsync(int tagId);

        Task<IEnumerable<Tag>> GetTagsBySessionAsync(int sessionId);
    }
}