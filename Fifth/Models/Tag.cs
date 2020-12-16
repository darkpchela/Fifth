using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Fifth.Models
{
    public partial class Tag
    {
        [Key]
        public int Id { get; set; }

        public string Value { get; set; }
    }
}