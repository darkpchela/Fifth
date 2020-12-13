#nullable disable

namespace Fifth.Models
{
    public partial class GameSession
    {
        public int Id { get; set; }
        public string ConnectionString { get; set; }
        public int OwnerId { get; set; }
        public int? OpponentId { get; set; }
        public string Name { get; set; }
        public bool Started { get; set; }

        public virtual User Owner { get; set; }
    }
}