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

        public Task<IEnumerable<Tag>> GetAllTags()
        {
            return Task.FromResult(unitOfWork.TagRepository.GetAll());
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
            await unitOfWork.SaveChanges();
        }

        public Task<IEnumerable<SessionData>> GetSessionsByTag(IEnumerable<Tag> tags)
        {
            var allSessionsTags = unitOfWork.SessionTagRepository.GetAll();
            var sessions = unitOfWork.SessionDataRepository.GetAll();
            foreach (var tag in tags)
            {
                var filtered = allSessionsTags.Where(st => st.TagId == tag.Id).ToList().Select(s => s.Session);
                sessions = sessions.Intersect(filtered);
            }
            return Task.FromResult(sessions.Distinct());
        }

        public Task<IEnumerable<SessionData>> GetSessionsByTag(string tagsJson)
        {
            if (string.IsNullOrEmpty(tagsJson) || !TryDeserializeTags(tagsJson, out Tag[] tags))
                return Task.FromResult(unitOfWork.SessionDataRepository.GetAll());
            var sessions = GetSessionsByTag(tags);
            return sessions;
        }

        public Task<IEnumerable<Tag>> GetTagsBySession(int sessionId)
        {
            var stags = unitOfWork.SessionTagRepository.GetAll().Where(st => st.SessionId == sessionId).ToList();
            var tags = stags.Select(st => st.Tag);
            return Task.FromResult(tags.AsEnumerable());
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
