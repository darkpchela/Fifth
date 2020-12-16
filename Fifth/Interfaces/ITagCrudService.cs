using Fifth.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fifth.Interfaces
{
    public interface ITagCrudService
    {
        Task<int> CreateAsync(Tag tag);

        Task DeleteAsync(int id);

        Task<Tag> GetAsync(int id);

        Task<Tag> GetAsync(string value);

        Task<IEnumerable<Tag>> GetAll();
    }
}