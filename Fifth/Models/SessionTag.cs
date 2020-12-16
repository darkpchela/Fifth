#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Fifth.Models
{
    public partial class SessionTag
    {
        [Key]
        public int Id { get; set; }

        public int SessionId { get; set; }

        public int TagId { get; set; }

        public virtual SessionData Session { get; set; }
    }
}