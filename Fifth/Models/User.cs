using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fifth.Models
{
    public class User
    {   
        [Key]
        public int Id { get; set; }

        public string Login { get; set; }
    }
}