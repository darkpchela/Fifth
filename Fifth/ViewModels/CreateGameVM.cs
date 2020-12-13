using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fifth.ViewModels
{
    public class CreateGameVM
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        public IList<string> Tags { get; set; }
    }
}