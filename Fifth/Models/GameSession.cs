using System.ComponentModel.DataAnnotations;

namespace Fifth.Models
{
    public class GameSession
    {
        [Key]
        public int SessionId { get; set; }

        public int OwnerId { get; set; }

        public int OpponentId { get; set; }

        public string SessionName { get; set; }
    }
}