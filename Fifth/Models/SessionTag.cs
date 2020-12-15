#nullable disable

namespace Fifth.Models
{
    public partial class SessionTag
    {
        public int SessionId { get; set; }

        public int TagId { get; set; }

        public virtual SessionData Session { get; set; }
    }
}