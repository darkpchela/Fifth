using Fifth.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fifth.Interfaces
{
    public interface ITagsProvider
    {
        Task<IEnumerable<Tag>> GetAllTags();

        Task<IEnumerable<SessionData>> GetSessionsByTag(IEnumerable<Tag> tags);

        Task<IEnumerable<SessionData>> GetSessionsByTag(string tagsJson);

        Task<IEnumerable<Tag>> GetTagsBySession(int sessionId);

        Task AssignTags(int sessionId, string tagsJson);
    }
}