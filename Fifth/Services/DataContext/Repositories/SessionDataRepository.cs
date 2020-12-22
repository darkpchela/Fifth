using Fifth.Interfaces.DataAccess;
using Fifth.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Fifth.Services.DataContext.Repositories
{
    public class SessionDataRepository : ISessionDataRepository
    {
        private AppDbContext dbContext;

        public SessionDataRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Create(SessionData entity)
        {
            dbContext.SessionDatas.Add(entity);
        }

        public void Delete(int key)
        {
            var entity = dbContext.SessionDatas.Find(key);
            if (entity != null)
                dbContext.SessionDatas.Remove(entity);
        }

        public SessionData Get(int key)
        {
            var entity = dbContext.SessionDatas.Find(key);
            return entity;
        }

        public IEnumerable<SessionData> GetAll()
        {
            return dbContext.SessionDatas;
        }

        public void Update(SessionData entity)
        {
            dbContext.SessionDatas.Update(entity);
        }
    }
}