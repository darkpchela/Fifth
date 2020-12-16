using Fifth.Interfaces;
using Fifth.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifth.Services.BasicCRUD
{
    public class TagCrudService : ITagCrudService
    {
        private readonly IUnitOfWork unitOfWork;

        public TagCrudService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<int> CreateAsync(Tag tag)
        {
            if (await unitOfWork.DbContext.Tags.AnyAsync(t => t.Value == tag.Value))
                return -1;
            unitOfWork.DbContext.Tags.Add(tag);
            await unitOfWork.DbContext.SaveChangesAsync();
            return tag.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var tag = await unitOfWork.DbContext.Tags.FindAsync(id);
            if (tag is null)
                return;
                unitOfWork.DbContext.Tags.Remove(tag);
                await unitOfWork.DbContext.SaveChangesAsync();
        }

        public async Task<Tag> GetAsync(int id)
        {
            return await unitOfWork.DbContext.Tags.FindAsync(id);
        }

        public async Task<Tag> GetAsync(string value)
        {
            var tag = await unitOfWork.DbContext.Tags.FirstOrDefaultAsync(t => t.Value == value);
            return tag;
        }

        public Task<IEnumerable<Tag>> GetAll()
        {
            return Task.FromResult(unitOfWork.DbContext.Tags.AsNoTracking().AsEnumerable());
        }
    }
}