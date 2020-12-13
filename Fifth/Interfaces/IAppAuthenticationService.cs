using System.Threading.Tasks;

namespace Fifth.Interfaces
{
    public interface IAppAuthenticationService
    {
        Task<bool> SignInAsync(string username, string password);

        Task<bool> SignUpAsync(string username, string password);

        Task SignOutAsync();
    }
}