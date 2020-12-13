using System.Collections.Generic;

#nullable disable

namespace Fifth.Models
{
    public partial class User
    {
        public User()
        {
            GameSessions = new HashSet<GameInfoData>();
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public virtual ICollection<GameInfoData> GameSessions { get; set; }
    }
}