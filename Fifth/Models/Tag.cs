using System;
using System.Collections.Generic;

#nullable disable

namespace Fifth.Models
{
    public partial class Tag
    {
        public Tag()
        {
            SessionTags = new HashSet<SessionTag>();
        }

        public int Id { get; set; }
        public string Value { get; set; }

        public virtual ICollection<SessionTag> SessionTags { get; set; }
    }
}
