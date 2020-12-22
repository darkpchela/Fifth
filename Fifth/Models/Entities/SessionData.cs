using System;
using System.Collections.Generic;

#nullable disable

namespace Fifth.Models
{
    public partial class SessionData
    {
        public SessionData()
        {
            SessionTags = new HashSet<SessionTag>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int CreatorId { get; set; }
        public bool Started { get; set; }

        public virtual User Creator { get; set; }
        public virtual ICollection<SessionTag> SessionTags { get; set; }
    }
}
