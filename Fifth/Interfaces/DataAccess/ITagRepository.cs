using Fifth.Models;

namespace Fifth.Interfaces.DataAccess
{
    public interface ITagRepository : IRepository<Tag, int>
    {
        Tag Get(string value);
    }
}