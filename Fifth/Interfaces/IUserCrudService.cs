using Fifth.Models;
using Fifth.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fifth.Interfaces
{
    public interface IUserCrudService
    {
        Task CreateAsync(UserSignUpVM createUserVM);

        Task<User> GetByLoginAsync(string login);

        Task<IEnumerable<User>> GetAll();
    }
}