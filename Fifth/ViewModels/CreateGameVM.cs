using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fifth.ViewModels
{
    public class GameSessionVM
    {
        [Required]
        [MinLength(3)]
        public string GameName { get; set; }

        [Required]
        public string UserName { get; set; }

        public IList<string> Tags { get; set; }
    }
}