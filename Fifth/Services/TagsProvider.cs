using Fifth.Interfaces;
using Fifth.Interfaces.DataAccess;
using Fifth.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifth.Services
{
    public class TagsProvider : ITagsProvider
    {
        private IUnitOfWork unitOfWork;

        public TagsProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task AssignTags(int sessionId, string tagsJson)
        {
            if (!TryDeserializeTags(tagsJson, out Tag[] tags))
                return;
            foreach (var v in tags.Select(t=>t.Value))
            {
                Tag tag = unitOfWork.TagRepository.Get(v);
                if (tag is null)
                {
                    tag = new Tag
                    {
                        Value = v
                    };
                    unitOfWork.TagRepository.Create(tag);
                    await unitOfWork.SaveChanges();
                }
                SessionTag sessionTag = new SessionTag
                {
                    SessionId =sessionId,
                    Tag = tag
                };
                unitOfWork.SessionTagRepository.Create(sessionTag);
            }
        }

        public Task<IEnumerable<SessionData>> GetSessionsByTag(IEnumerable<int> tagIds)
        {
            var allSessionsTags = unitOfWork.SessionTagRepository.GetAll();
            var sessions = allSessionsTags.Select(e => e.Session);
            foreach (var id in tagIds)
            {
                var filtered = allSessionsTags.Where(st => st.TagId == id).Select(s => s.Session);
                sessions = sessions.Intersect(filtered);
            }
            return Task.FromResult(sessions.Distinct());
        }

        public Task<IEnumerable<Tag>> GetTagsBySession(int sessionId)
        {
            var allSessionTags = unitOfWork.SessionTagRepository.GetAll();
            var allTags = unitOfWork.TagRepository.GetAll();
            var tagsId = allSessionTags.Where(s => s.SessionId == sessionId).Select(st => st.TagId);
            var tags = allTags.Where(t => tagsId.Contains(t.Id));
            return Task.FromResult(tags);
        }

        private bool TryDeserializeTags(string tagsJson, out Tag[] tags)
        {
            try
            {
                tags = JsonConvert.DeserializeObject<Tag[]>(tagsJson);
                return true;
            }
            catch
            {
                tags = null;
                return false;
            }
        }
    }
}
