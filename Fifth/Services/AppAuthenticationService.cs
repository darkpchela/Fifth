using Fifth.Interfaces;
using Fifth.Models;
using Fifth.Services.DataContext;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fifth.Services
{
    public class AppAuthenticationService : IAppAuthenticationService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly AppDbContext dbContext;
        public AppAuthenticationService(IHttpContextAccessor httpContextAccessor, AppDbContext dbContext)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }
        public async Task<bool> SignUpAsync(string userName, string password)
        {
            if (await dbContext.Users.AnyAsync(u => u.Login == userName))
                return false;

            var user = new User
            {
                Login = userName,
                Password = password
            };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
            await Authenticate(user.Login);
            return true;
        }

        public async Task<bool> SignInAsync(string userName, string password)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Login == userName && u.Password == password);
            if (user is null)
                return false;
            await Authenticate(user.Login);
            return true;
        }


        public async Task SignOutAsync()
        {
            await httpContextAccessor.HttpContext.SignOutAsync();
        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim> 
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}