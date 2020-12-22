using Fifth.Interfaces.DataAccess;
using Fifth.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Fifth.Services.DataContext.Repositories
{
    public class UserRepository : IUserRepository
    {
        private AppDbContext dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Create(User entity)
        {
            dbContext.Add(entity);
        }

        public void Delete(string key)
        {
            var entity = dbContext.Users.FirstOrDefault(u => u.Login == key);
            if (entity != null)
                dbContext.Users.Remove(entity);
        }

        public User Get(string key)
        {
            var entity = dbContext.Users.FirstOrDefault(u => u.Login == key);
            return entity;
        }

        public IEnumerable<User> GetAll()
        {
            return dbContext.Users;
        }

        public void Update(User entity)
        {
            dbContext.Users.Update(entity);
        }
    }
}