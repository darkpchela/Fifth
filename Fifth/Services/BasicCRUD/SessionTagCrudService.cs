using Fifth.Interfaces;
using Fifth.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifth.Services.BasicCRUD
{
    public class SessionTagCrudService : ISessionTagCrudService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IGamesCrudService gamesCrudService;

        public SessionTagCrudService(IUnitOfWork unitOfWork, IGamesCrudService gamesCrudService)
        {
            this.unitOfWork = unitOfWork;
            this.gamesCrudService = gamesCrudService;
        }

        public async Task CreateAsync(int gameId, int tagId)
        {
            var sessionData = (await gamesCrudService.GetGameAsync(gameId)).GameData;
            if (sessionData != null && await unitOfWork.DbContext.Tags.AnyAsync(t => t.Id == tagId))
            {
                var entity = new SessionTag
                {
                    Session = sessionData,
                    TagId = tagId
                };
                unitOfWork.DbContext.SessionTags.Add(entity);
                await unitOfWork.DbContext.SaveChangesAsync();
            }
        }

        public async Task<IQueryable<SessionTag>> GetAll()
        {
            return unitOfWork.DbContext.SessionTags.AsNoTracking();
        }

        public async Task<IEnumerable<Tag>> GetTagsBySessionAsync(int sessionId)
        {
            var sessionData = await unitOfWork.DbContext.Sessions.FindAsync(sessionId);
            if (sessionData == null)
                return new List<Tag>();
            var tagsId = unitOfWork.DbContext.SessionTags.Where(s => s.SessionId == sessionId).Select(st => st.TagId);
            var tags = unitOfWork.DbContext.Tags.Where(t => tagsId.Contains(t.Id));
            return tags.AsNoTracking();
        }

        public async Task<IEnumerable<SessionData>> GetSessionsByTagAsync(IEnumerable<int> tagIds)
        {
            var sessionTags = unitOfWork.DbContext.SessionTags.AsNoTracking();
            var session = unitOfWork.DbContext.SessionTags.Select(s => s.Session);
            foreach (var id in tagIds)
            {
                var s = sessionTags.Where(st => st.TagId == id).Select(s => s.Session);
                session = session.Intersect(s);
            }
            //var grouped = sessionTags
            //    .Select(st => new
            //    {
            //        id = st.SessionId,
            //        tag = st.TagId,
            //        session = st.Session
            //    })
            //    .GroupBy(n => n.id)
            //    .Where(g => tagIds.Except(g.Select(g => g.tag)).Count() == 0);
            //var session = new List<SessionData>();
            //foreach (var g in grouped)
            //{
            //    g.
            //}
            return await session.ToListAsync();
        }
    }
}