using System;
using System.Collections.Generic;

#nullable disable

namespace Fifth.Models
{
    public partial class User
    {
        public User()
        {
            Sessions = new HashSet<SessionData>();
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public virtual ICollection<SessionData> Sessions { get; set; }
    }
}
