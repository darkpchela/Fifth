using System;
using System.Collections.Generic;

#nullable disable

namespace Fifth.Models
{
    public partial class User
    {
        public User()
        {
            SessionData = new HashSet<SessionData>();
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public virtual ICollection<SessionData> SessionData { get; set; }
    }
}
