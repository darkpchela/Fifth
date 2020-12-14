using System.Collections.Generic;

#nullable disable

namespace Fifth.Models
{
    public partial class User
    {
        public User()
        {
            GameSessions = new HashSet<GameData>();
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public virtual ICollection<GameData> GameSessions { get; set; }
    }
}