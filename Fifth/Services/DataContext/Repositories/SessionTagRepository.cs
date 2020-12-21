using Fifth.Interfaces.DataAccess;
using Fifth.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Fifth.Services.DataContext.Repositories
{
    public class SessionTagRepository : ISessionTagRepository
    {
        private AppDbContext dbContext;

        public SessionTagRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Create(SessionTag entity)
        {
            dbContext.SessionTags.Add(entity);
        }

        public void Delete(int key)
        {
            var entity = dbContext.SessionTags.Find(key);
            if (entity != null)
                dbContext.SessionTags.Remove(entity);
        }

        public SessionTag Get(int key)
        {
            var entity = dbContext.SessionTags.Find(key);
            return entity;
        }

        public IEnumerable<SessionTag> GetAll()
        {
            return dbContext.SessionTags.AsNoTracking();
        }

        public void Update(SessionTag entity)
        {
            dbContext.SessionTags.Update(entity);
        }
    }
}