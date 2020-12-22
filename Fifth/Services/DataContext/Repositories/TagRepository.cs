using Fifth.Interfaces.DataAccess;
using Fifth.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Fifth.Services.DataContext.Repositories
{
    public class TagRepository : ITagRepository
    {
        private AppDbContext dbContext;

        public TagRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Create(Tag entity)
        {
            dbContext.Tags.Add(entity);
        }

        public void Delete(int key)
        {
            var entity = dbContext.Tags.Find(key);
            if (entity != null)
                dbContext.Tags.Remove(entity);
        }

        public Tag Get(int key)
        {
            var entity = dbContext.Tags.Find(key);
            return entity;
        }

        public Tag Get(string value)
        {
            var entity = dbContext.Tags.FirstOrDefault(e => e.Value == value);
            return entity;
        }

        public IEnumerable<Tag> GetAll()
        {
            return dbContext.Tags;
        }

        public void Update(Tag entity)
        {
            dbContext.Tags.Update(entity);
        }
    }
}