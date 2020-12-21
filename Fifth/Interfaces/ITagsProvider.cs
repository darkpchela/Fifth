using Fifth.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fifth.Interfaces
{
    public interface ITagsProvider
    {
        Task<IEnumerable<SessionData>> GetSessionsByTag(IEnumerable<int> tagId);

        Task<IEnumerable<Tag>> GetTagsBySession(int sessionId);

        Task AssignTags(int sessionId, string tagsJson);
    }
}