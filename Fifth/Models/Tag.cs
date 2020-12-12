using System.ComponentModel.DataAnnotations;

namespace Fifth.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        public string Text { get; set; }
    }
}