using Fifth.Interfaces;
using Fifth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fifth.Services
{
    public class CookieAuthenticationService : ICookieAuthenticationService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly FifthDbContext dbContext;
        public CookieAuthenticationService(IHttpContextAccessor httpContextAccessor, FifthDbContext dbContext)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }
        public async Task<bool> SignUpAsync(string userName, string password)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Login == userName);
            if (user != null)
                return false;
            await dbContext.Users.AddAsync(new User
            {
                Login = userName
            });
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SignInAsync(string userName, string password)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Login == userName);
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