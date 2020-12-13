using System.Collections.Generic;

#nullable disable

namespace Fifth.Models
{
    public partial class User
    {
        public User()
        {
            GameSessions = new HashSet<GameSession>();
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public virtual ICollection<GameSession> GameSessions { get; set; }
    }
}