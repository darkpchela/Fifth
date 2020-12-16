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
                try
                {
                unitOfWork.DbContext.SessionTags.Add(entity);
                await unitOfWork.DbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {

                }
            }
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

        public async Task<IEnumerable<SessionData>> GetSessionsByTagAsync(int tagId)
        {
            if (!await unitOfWork.DbContext.Tags.AnyAsync(t => t.Id == tagId))
                return new List<SessionData>();
            var sessions = unitOfWork.DbContext.SessionTags.Where(st => st.TagId == tagId).Include(e => e.Session).Select(r => r.Session);
            return sessions;
        }
    }
}