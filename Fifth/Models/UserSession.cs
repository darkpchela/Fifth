using System;
using System.Collections.Generic;

#nullable disable

namespace Fifth.Models
{
    public partial class UserSession
    {
        public int UserId { get; set; }
        public int SessionId { get; set; }

        public virtual SessionData Session { get; set; }
        public virtual SessionData User { get; set; }
    }
}
